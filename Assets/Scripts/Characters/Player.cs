using Com.Dot.SNC.Input;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SNC.Characters
{
    public class Player : NetworkBehaviour
    {
        public Camera playerCamera = null;
        public CharacterController controller = null;

        [Space]
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
