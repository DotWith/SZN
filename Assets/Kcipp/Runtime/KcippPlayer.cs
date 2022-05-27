using UnityEngine;
using Kcipp.Settings;
using Kcipp.Packets;

namespace Kcipp
{
    [AddComponentMenu("Kcipp/Kcipp Player")]
    public class KcippPlayer : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        
        int lastSamplePlayed;

        public void UpdateSoundSamples(AudioPacket sound)
        {
            if (!audioSource.isPlaying)
            {
                InitialiseAudioSource();
            }

            audioSource.clip.SetData(sound.samples, lastSamplePlayed);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayDelayed(0.1f);
            }

            lastSamplePlayed = (lastSamplePlayed + sound.samples.Length) % MicrophoneSettings.MaxAudioClipSamples;
        }

        void InitialiseAudioSource()
        {
            lastSamplePlayed = 0;
            audioSource.clip = AudioClip.Create("TransmittedAudio", MicrophoneSettings.MaxAudioClipSamples,
                MicrophoneSettings.AudioTransmissionChannels, MicrophoneSettings.Frequency, false);
        }
    }
}