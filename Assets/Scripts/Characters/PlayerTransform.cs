using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerTransform : NetworkBehaviour
    {
        [SerializeField] Player player;

        public override void OnStartAuthority()
        {
            enabled = true;

            player.Controls.Player.Transform.performed += ctx => CmdTransform();
        }

        [Command]
        void CmdTransform()
        {
            if (player.stats.playerRole != PlayerRole.Traitor) { return; }

            player.stats.isTransformed = !player.stats.isTransformed;
        }
    }
}
