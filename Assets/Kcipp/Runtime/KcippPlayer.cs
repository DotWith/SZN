using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        int lastSample;

        int micPosition;
        AudioClip micAudioClip;

        float timedMicLevel;

        public override void OnStartServer()
        {
            KcippManager.singleton.voiceChatChannels.Add(this, 1);
        }

        public override void OnStartLocalPlayer()
        {
            micAudioClip = Microphone.Start(null, true,
                KcippManager.singleton.micRecordLenght, KcippManager.singleton.micFrequency);
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
                if (micPosition > KcippManager.singleton.micRecordLenght)
                {
                    float[] sa = new float[(micPosition - KcippManager.singleton.micRecordLenght) * micAudioClip.channels];
                    micAudioClip.GetData(sa, micPosition - KcippManager.singleton.micRecordLenght);

                    float levelMax = 0;
                    for (int i = 0; i < KcippManager.singleton.micRecordLenght; i++)
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

            if (KcippManager.singleton.proximity)
            {
                SendProximityVoice();
            }
            else
            {
                SendChanneledVoice();
            }
        }

        void SendChanneledVoice()
        {
            int diff = micPosition - lastSample;

            if (diff >= KcippManager.singleton.micSamplePacketSize)
            {
                float[] samples = new float[diff * micAudioClip.channels];
                micAudioClip.GetData(samples, lastSample);
                byte[] ba = Utils.ToByteArray(samples);

                CmdSendVoice(ba);

                lastSample = micPosition;
            }
        }

        public void SendProximityVoice()
        {
            int diff = micPosition - lastSample;
            if (diff >= KcippManager.singleton.micSamplePacketSize)
            {
                float[] samples = new float[diff * micAudioClip.channels];
                micAudioClip.GetData(samples, lastSample);
                byte[] ba = Utils.ToByteArray(samples);

                CmdSendProximityVoice(ba);

                lastSample = micPosition;
            }
        }

        [Command]
        public void CmdSendProximityVoice(byte[] ba)
        {
            foreach (var vckp in KcippManager.singleton.voiceChatChannels)
            {
                if (vckp.Key)
                {
                    if (vckp.Key != this && (transform.position - vckp.Key.transform.position).sqrMagnitude < KcippManager.singleton.maxDistance)
                    {
                        TargetRpcReciveVoice(vckp.Key.connectionToClient, ba);
                    }
                }
                else
                {
                    KcippManager.singleton.voiceChatChannels.Remove(vckp.Key);
                }
            }
        }

        [Command]
        public void CmdChangeChannel(byte channel)
        {
            KcippManager.singleton.voiceChatChannels[this] = channel;
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
            Dictionary<KcippPlayer, byte> voiceChatChannels = KcippManager.singleton.voiceChatChannels;
            byte channel = voiceChatChannels[this];

            foreach (var pl in voiceChatChannels)
            {
                if (pl.Key)
                {
                    if (pl.Value == channel && pl.Key != this)
                    {
                        TargetRpcReciveVoice(pl.Key.connectionToClient, ba);
                    }
                }
                else
                {
                    KcippManager.singleton.voiceChatChannels.Remove(pl.Key);
                }
            }
        }

        void ParseVoiceData(byte[] ba)
        {
            float[] f = Utils.ToFloatArray(ba);
            AudioClip ac = AudioClip.Create("voice", f.Length, 1, KcippManager.singleton.micFrequency, false);
            ac.SetData(f, 0);
            audioSource.clip = ac;
            if (!audioSource.isPlaying) audioSource.Play();
        }
    }
}
