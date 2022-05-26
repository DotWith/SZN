using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kcipp
{
    [AddComponentMenu("Kcipp/Kcipp Player")]
    [RequireComponent(typeof(NetworkIdentity))]
    public class KcippPlayer : NetworkBehaviour
    {
        public AudioSource audioSource;
        public AnimationCurve VoiceRollOff = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 1f),
        });
        public bool AlwaysTalk = false;

        [Header("Voice Chat Settings")]
        [Tooltip("Its frequency of input (mic) in khz")]
        public int micFrequency = 44100;

        [Tooltip("If its lower you might hear \"clipping\"")]
        public int micSamplePacketSize = 7350;

        int lastSample;
        int localChannel;

        int micPosition;
        AudioClip micAudioClip;

        float timedMicLevel;

        public override void OnStartServer()
        {
            KcippClient newClient = new KcippClient()
            {
                plr = this,
                channel = 1
            };

            KcippManager.singleton.voiceChatClients.Add(newClient);
        }

        public override void OnStartLocalPlayer()
        {
            micAudioClip = AudioClip.Create("mic", 1, 1, 24000, false);

            micAudioClip = Microphone.Start(null, true, 120, 24000);
        }

        public void Update()
        {
            if (!isLocalPlayer) { return; }

            if (AlwaysTalk)
            {
                lastSample = micPosition;
            }

            micPosition = Microphone.GetPosition(null);

            if (timedMicLevel >= 0.07f)
            {
                if (micPosition > 128)
                {
                    float[] sa = new float[(micPosition - 128) * micAudioClip.channels];
                    micAudioClip.GetData(sa, micPosition - 128);

                    float levelMax = 0;
                    for (int i = 0; i < 128; i++)
                    {
                        float wavePeak = sa[i] * sa[i];
                        if (levelMax < wavePeak)
                        {
                            levelMax = wavePeak;
                        }
                    }
                    timedMicLevel = 0;
                }
            }
            else
            {
                timedMicLevel += Time.deltaTime;
            }
        }

        public void Talk()
        {
            if (AlwaysTalk) { return; }

            if (KcippManager.singleton.proximity) SendProximityVoice();
            else SendChanneledVoice();
        }

        void SendChanneledVoice()
        {
            int diff = micPosition - lastSample;

            if (diff >= micSamplePacketSize)
            {
                float[] samples = new float[diff * micAudioClip.channels];
                micAudioClip.GetData(samples, lastSample);
                byte[] ba = ToByteArray(samples);

                CmdSendVoice(ba);

                lastSample = micPosition;
            }
        }

        public void SendProximityVoice()
        {
            int diff = micPosition - lastSample;
            if (diff >= micSamplePacketSize)
            {
                float[] samples = new float[diff * micAudioClip.channels];
                micAudioClip.GetData(samples, lastSample);
                byte[] ba = ToByteArray(samples);

                CmdSendProximityVoice(ba);

                lastSample = micPosition;
            }
        }

        [Command]
        public void CmdSendProximityVoice(byte[] ba)
        {
            foreach (var vckp in KcippManager.singleton.voiceChatClients)
            {
                if (vckp.plr)
                {
                    if (vckp.plr != this && (transform.position - vckp.plr.transform.position).sqrMagnitude < KcippManager.singleton.maxDistance)
                    {
                        TargetRpcReciveVoice(vckp.plr.connectionToClient, ba);
                    }
                }
                else
                {
                    KcippManager.singleton.voiceChatClients.Remove(vckp);
                }
            }
        }

        [Command]
        public void CmdChangeChannel(byte channel)
        {
            // TODO: Redo
            //KcippManager.singleton.voiceChatClients[this] = channel;
        }

        [TargetRpc]
        public void TargetRpcReciveVoice(NetworkConnection conn, byte[] ba)
        {
            if (KcippManager.singleton.proximity)
            {
                audioSource.volume = VoiceRollOff.Evaluate(-((transform.position - conn.identity.transform.position).magnitude / KcippManager.singleton.maxDistance) + 1);
            }
            else if (audioSource.volume != 1)
            {
                audioSource.volume = 1;
            }

            ParseVoiceData(ba);
        }

        [Command]
        public void CmdSendVoice(byte[] ba)
        {
            foreach (var pl in KcippManager.singleton.voiceChatClients)
            {
                if (pl.plr)
                {
                    if (pl.channel == localChannel && pl.plr != this)
                    {
                        TargetRpcReciveVoice(pl.plr.connectionToClient, ba);
                    }
                }
                else
                {
                    KcippManager.singleton.voiceChatClients.Remove(pl);
                }
            }
        }

        void ParseVoiceData(byte[] ba)
        {
            float[] f = ToFloatArray(ba);
            AudioClip ac = AudioClip.Create("voice", f.Length, 1, micFrequency, false);
            ac.SetData(f, 0);
            audioSource.clip = ac;
            if (!audioSource.isPlaying) audioSource.Play();
        }

        byte[] ToByteArray(float[] floatArray)
        {
            int len = floatArray.Length * 4;
            byte[] byteArray = new byte[len];
            int pos = 0;
            foreach (float f in floatArray)
            {
                byte[] data = System.BitConverter.GetBytes(f);
                System.Array.Copy(data, 0, byteArray, pos, 4);
                pos += 4;
            }
            return byteArray;
        }

        float[] ToFloatArray(byte[] byteArray)
        {
            int len = byteArray.Length / 4;
            float[] floatArray = new float[len];
            for (int i = 0; i < byteArray.Length; i += 4)
            {
                floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
            }
            return floatArray;
        }
    }
}
