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
        [SyncVar]
        public PlayerRole playerRole = PlayerRole.Spectator;

        public override void OnStartServer()
        {
            ServerSelectRandomRoleForAll(1);
        }

        [Server]
        public void ServerSelectRandomRoleForAll(int traitorsCount = 2)
        {
            foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
            {
                Debug.Log("Player: " + ps.name);

                int role = Random.Range(0, 2);

                if (traitorsCount == 0)
                    role = 0;

                if ((PlayerRole)role == PlayerRole.Traitor)
                    traitorsCount--;

                ps.playerRole = (PlayerRole)role;
            }
        }
    }
}
