using Com.Dot.SZN.Characters;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN
{
    [RequireComponent(typeof(AudioSource))]
    public class VoiceChat : NetworkBehaviour
    {
        [SerializeField] Player player;

        AudioSource audioSource;
        AudioClip microphoneClip;

        int lastSample;

        public void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.spatialBlend = 1.0f;
        }

        public override void OnStartAuthority()
        {
            enabled = true;

            string microphoneName = Microphone.devices[0];
            microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);

            player.Controls.Player.VoiceChat.performed += ctx => Talk();
        }

        [Client]
        void Talk()
        {
            string microphoneName = Microphone.devices[0];
            int pos = Microphone.GetPosition(microphoneName);
            int diff = pos - lastSample;

            if (diff > 0)
            {
                float[] samples = new float[diff * microphoneClip.channels];
                microphoneClip.GetData(samples, lastSample);
                byte[] audioArrayByte = ToByteArray(samples);

                CmdSendBytes(audioArrayByte, microphoneClip.channels);
            }

            lastSample = pos;
        }

        [Command(requiresAuthority = false)]
        void CmdSendBytes(byte[] voice, int channels)
        {
            RpcSendBytes(voice, channels);
        }

        [ClientRpc(includeOwner = true)]
        void RpcSendBytes(byte[] voice, int channels)
        {
            AudioClip voiceClip = AudioClip.Create("test", voice.Length, channels, AudioSettings.outputSampleRate, false);
            voiceClip.SetData(ToFloatArray(voice), 0);
            audioSource.clip = voiceClip;
            audioSource.Play();
        }

        public byte[] ToByteArray(float[] floatArray)
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

        public float[] ToFloatArray(byte[] byteArray)
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
