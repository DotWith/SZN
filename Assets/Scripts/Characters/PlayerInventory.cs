using Com.Dot.SZN.InventorySystem;
using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Com.Dot.SZN.Characters
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] Inventory inventory;

        [Header("Properties")]
        [SerializeField] Transform viewModelTransform;
        [SerializeField] Transform worldModelTransform;
        [SerializeField] int viewModelLayer;

        GameObject currentViewModel;
        GameObject currentWorldModel;

        public void Start()
        {
            if (!hasAuthority)
            {
                inventory.onSyncItems += OnOthersSyncItems;
            }
        }

        public override void OnStartAuthority()
        {
            inventory.onSyncItems += OnClientSyncItems;
        }

        public override void OnStopAuthority()
        {
            inventory.onSyncItems -= OnClientSyncItems;
        }

        #region Input Actions
        public void ChangeItemToFirst(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            inventory.ChangeItem(0);
        }

        public void ChangeItemToSeconded(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            inventory.ChangeItem(1);
        }

        public void UseActiveItem(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            inventory.UseActiveItem();
        }

        public void RemoveActiveItem(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            RemoveItem();
        }
        #endregion // Input Actions

        #region Item
        void OnClientSyncItems(IReadOnlyCollection<string> items, int activeItem)
        {
            Debug.Log($"Items: {items} ActiveItem: {activeItem}");
            var item = inventory.GetItem(activeItem);

            if (item == null) { return; }

            if (currentViewModel != null)
                Destroy(currentViewModel);

            currentViewModel = Instantiate(item.prefab, viewModelTransform);

            ModifyViewModelObject(currentViewModel.gameObject);

            for (int i = 0; i < currentViewModel.transform.childCount; i++)
            {
                var child = currentViewModel.transform.GetChild(i);
                ModifyViewModelObject(child.gameObject);
            }
        }

        void OnOthersSyncItems(IReadOnlyCollection<string> items, int activeItem)
        {
            var item = inventory.GetItem(activeItem);

            if (item == null) { return; }

            if (currentWorldModel != null)
            {
                Destroy(currentWorldModel);
            }

            currentWorldModel = Instantiate(item.prefab, worldModelTransform);
        }

        void RemoveItem()
        {
            var item = inventory.GetItem(inventory.activeItem);

            if (item == null) { return; }

            inventory.RemoveItem(item.id);

            if (currentViewModel != null)
                Destroy(currentViewModel);
        }

        void ModifyViewModelObject(GameObject obj)
        {
            obj.layer = viewModelLayer;
            obj.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
        }
        #endregion // Item
    }
}
