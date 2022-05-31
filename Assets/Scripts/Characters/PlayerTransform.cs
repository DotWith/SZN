using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Dot.SZN.Characters
{
    public class PlayerTransform : NetworkBehaviour
    {
        [SerializeField] Player player;

        public void Transform(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            CmdTransform();
        }

        [Command]
        void CmdTransform()
        {
            if (player.stats.playerRole != PlayerRole.Traitor) { return; }

            player.stats.isTransformed = !player.stats.isTransformed;
        }
    }
}
