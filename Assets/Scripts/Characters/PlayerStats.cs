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

    [System.Serializable]
    public class RoleVisuals
    {
        public PlayerRole role;
        public GameObject visuals;
    }

    public class PlayerStats : NetworkBehaviour
    {
        [SyncVar]
        public PlayerRole playerRole = PlayerRole.Spectator;

        [SyncVar]
        public bool isTransformed = false;

        public RoleVisuals[] roleVisuals;

        public override void OnStartServer()
        {
            ServerSelectRandomRoleForAll(1);
        }

        public void Update()
        {
            for (int i = 0; i < roleVisuals.Length; i++)
            {
                var roleVisual = roleVisuals[i];
                switch (roleVisual.role)
                {
                    case PlayerRole.Traitor:
                        roleVisual.visuals.SetActive(roleVisual.role == PlayerRole.Traitor && isTransformed);
                        break;
                    default:
                        roleVisual.visuals.SetActive(roleVisual.role == playerRole);
                        break;
                }
                //roleVisual.visuals.SetActive(roleVisual.role == playerRole);
            }
        }

        [Server]
        public void ServerSelectRandomRoleForAll(int traitorsCount = 2)
        {
            foreach (PlayerStats ps in FindObjectsOfType<PlayerStats>())
            {
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
