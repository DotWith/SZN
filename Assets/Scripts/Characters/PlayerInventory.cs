using Com.Dot.SZN.InventorySystem;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;

        public void ChangeItemToFirst(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            InventoryManager.singleton.ChangeItem(0);
        }

        public void ChangeItemToSeconded(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            InventoryManager.singleton.ChangeItem(1);
        }

        public void UseActiveItem(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            InventoryManager.singleton.UseItem();
        }
    }
}
