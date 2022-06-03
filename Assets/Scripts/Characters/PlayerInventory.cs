using Com.Dot.SZN.InventorySystem;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] Transform viewModelTransform;
        [SerializeField] int viewModelLayer;

        GameObject currentViewModel;

        public override void OnStartAuthority()
        {
            InventoryManager.singleton.onSyncItems += OnSyncItems;
            InventoryManager.singleton.onRemoveItem += OnDropItem;
        }

        public override void OnStopAuthority()
        {
            InventoryManager.singleton.onSyncItems -= OnSyncItems;
            InventoryManager.singleton.onRemoveItem -= OnDropItem;
        }

        #region Input Actions
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

        public void RemoveActiveItem(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            InventoryManager.singleton.RemoveItem();
        }
        #endregion // Input Actions

        void OnSyncItems(InventoryList<string> items, int activeItem)
        {
            if (currentViewModel != null)
                Destroy(currentViewModel);

            var item = InventoryManager.singleton.GetItem(items.GetValue(activeItem));
            
            if (item == null) { return; }

            currentViewModel = Instantiate(item.prefab, viewModelTransform);

            ModifyViewModelObject(currentViewModel.gameObject);

            for (int i = 0; i < currentViewModel.transform.childCount; i++)
            {
                var child = currentViewModel.transform.GetChild(i);
                ModifyViewModelObject(child.gameObject);
            }
        }

        void OnDropItem()
        {
            Debug.Log("Drop item");

            if (currentViewModel != null)
                Destroy(currentViewModel);


        }

        void ModifyViewModelObject(GameObject obj)
        {
            obj.layer = viewModelLayer;
            obj.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
        }
    }
}
