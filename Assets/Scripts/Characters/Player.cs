using Com.Dot.SZN.Input;
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

        Controls controls;
        public Controls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            playerCamera.gameObject.SetActive(true);
            playerVisuals.gameObject.SetActive(false);

            enabled = true;
        }

        [ClientCallback]
        public void OnEnable() => Controls.Enable();
        [ClientCallback]
        public void OnDisable() => Controls.Disable();
    }
}
