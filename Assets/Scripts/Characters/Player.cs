using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class Player : NetworkBehaviour
    {
        public PlayerStats stats;
        public PlayerMove move;

        [Space]
        public Camera playerCamera = null;
        public CharacterController controller = null;
        public GameObject playerVisuals = null;

        public override void OnStartAuthority()
        {
            playerCamera.gameObject.SetActive(true);
            playerVisuals.gameObject.SetActive(false);
        }
    }
}
