using Com.Dot.SZN.Input;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Dot.SZN.Characters
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

        PlayerInventory inventory;
        public PlayerInventory Inventory
        {
            get
            {
                if (inventory != null) { return inventory; }
                return inventory = new PlayerInventory(2); // TODO: Add more items depending on the class
            }
        }

        public override void OnStartAuthority()
        {
            playerCamera.gameObject.SetActive(true);
            playerVisuals.gameObject.SetActive(false);

            enabled = true;

            Controls.Player.Debug.performed += ctx => ListAllItems();
        }

        [ClientCallback]
        public void OnEnable() => Controls.Enable();
        [ClientCallback]
        public void OnDisable() => Controls.Disable();

        void ListAllItems()
        {
            foreach (var item in inventory.items)
            {
                Debug.Log("Item: " + item.name);
            }
        }
    }
}
