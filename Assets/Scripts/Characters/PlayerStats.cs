using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public enum PlayerRole : byte
    {
        Innocent,
        Traitor,
        Spectator
    }

    public class PlayerStats : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SetPlayerRole))]
        public PlayerRole playerRole = PlayerRole.Spectator;

        [SyncVar(hook = nameof(SetTransformed))]
        public bool isTransformed = false;

        public GameObject innocentVisuals;
        public GameObject traitorVisuals;

        void SetPlayerRole(PlayerRole oldRole, PlayerRole newRole)
        {
        }

        void SetTransformed(bool oldTrans, bool newTrans)
        {
            innocentVisuals.SetActive(!newTrans);
            traitorVisuals.SetActive(newTrans);
        }

        public override void OnStartServer()
        {
            ServerSelectRandomRoleForAll(1);
        }

        [Server]
        public void ServerSelectRandomRoleForAll(int traitorsCount = 2)
        {
            foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
            {
                PlayerRole role = PlayerRole.Spectator;

                if (traitorsCount == 0)
                {
                    role = PlayerRole.Innocent;
                }
                else
                {
                    role = PlayerRole.Traitor;
                    traitorsCount--;
                }

                ps.playerRole = role;
            }
        }
    }
}
