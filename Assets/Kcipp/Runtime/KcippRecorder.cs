using UnityEngine;
using Mirror;
using Kcipp.Settings;
using Kcipp.Packets;

namespace Kcipp
{
    [AddComponentMenu("Kcipp/Kcipp Recorder")]
    public class KcippRecorder : NetworkBehaviour
    {
        AudioClip clipToTransmit;
        int lastSampleOffset;
        KcippPlayer kcippPlayer;

        public override void OnStartLocalPlayer()
        {
            kcippPlayer = GetComponent<KcippPlayer>();
            clipToTransmit = Microphone.Start(null, true, 10, MicrophoneSettings.Frequency);
        }

        public override void OnStopLocalPlayer()
        {
            Microphone.End(null);
        }

        public void FixedUpdate()
        {
            if (!isLocalPlayer) { return; }

            int currentMicSamplePosition = Microphone.GetPosition(null);
            int samplesToTransmit = GetSampleTransmissionCount(currentMicSamplePosition);

            if (samplesToTransmit > 0)
            {
                TransmitSamples(samplesToTransmit);
                lastSampleOffset = currentMicSamplePosition;
            }
        }

        int GetSampleTransmissionCount(int currentMicrophoneSample)
        {
            int sampleTransmissionCount = currentMicrophoneSample - lastSampleOffset;
            if (sampleTransmissionCount < 0)
            {
                sampleTransmissionCount = (clipToTransmit.samples - lastSampleOffset) + currentMicrophoneSample;
            }
            return sampleTransmissionCount;
        }

        void TransmitSamples(int sampleCountToTransmit)
        {
            float[] samplesToTransmit = new float[sampleCountToTransmit * clipToTransmit.channels];
            clipToTransmit.GetData(samplesToTransmit, lastSampleOffset);
            var newAudioPacket = new AudioPacket()
            {
                samples = samplesToTransmit,
            };
            CmdSendAudio(newAudioPacket);
        }

        [Command]
        public void CmdSendAudio(AudioPacket audio)
        {
            foreach (var connection in NetworkServer.connections)
            {
                if (connection.Value != connectionToClient)
                {
                    TargetPlayAudio(audio);
                }
            }
        }

        [TargetRpc]
        public void TargetPlayAudio(AudioPacket audio)
        {
            kcippPlayer.UpdateSoundSamples(audio);
        }
    }
}
