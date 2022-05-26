using Kcipp;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerVoiceChat : NetworkBehaviour
    {
        [SerializeField] Player player;

        [SerializeField] KcippPlayer voiceChat;

        public override void OnStartAuthority()
        {
            enabled = true;

            player.Controls.Player.VoiceChat.performed += ctx => voiceChat.Talk();
        }
    }
}
