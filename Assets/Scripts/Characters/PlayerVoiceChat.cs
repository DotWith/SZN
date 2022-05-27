using Kcipp;
using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerVoiceChat : NetworkBehaviour
    {
        [SerializeField] Player player;

        [SerializeField] KcippRecorder recorder;

        public override void OnStartAuthority()
        {
            enabled = true;

            //player.Controls.Player.VoiceChat.performed += ctx => recorder.transmitData = true;
            //player.Controls.Player.VoiceChat.canceled += ctx => recorder.transmitData = false;
        }
    }
}
