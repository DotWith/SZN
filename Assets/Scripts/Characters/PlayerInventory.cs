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
        public Inventory inventory;

        [Header("Properties")]
        public Transform viewModelTransform;
        public Transform worldModelTransform;
        public int viewModelLayer;

        GameObject currentViewModel;
        GameObject currentWorldModel;

        public void Start()
        {
            if (!hasAuthority)
            {
                inventory.onSyncItems += OnOthersSyncItems;
                inventory.onRemoveItem += OnOthersRemoveItem;
            }

            inventory.onRemoveItem += OnBothRemoveItem;
        }

        public override void OnStartAuthority()
        {
            inventory.onSyncItems += OnClientSyncItems;
            inventory.onRemoveItem += OnClientRemoveItem;
        }

        public override void OnStopAuthority()
        {
            inventory.onSyncItems -= OnClientSyncItems;
            inventory.onRemoveItem -= OnClientRemoveItem;
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

            var item = inventory.GetItem(inventory.activeItem);

            if (item == null) { return; }

            inventory.RemoveItem(item.id);
        }
        #endregion // Input Actions

        #region Item
        void OnClientSyncItems(IReadOnlyCollection<string> items, int activeItem)
        {
            Debug.Log($"Items: {items} ActiveItem: {activeItem}");

            if (currentViewModel != null)
                Destroy(currentViewModel);

            var item = inventory.GetItem(activeItem);

            if (item == null) { return; }

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
            if (currentWorldModel != null)
                Destroy(currentWorldModel);

            var item = inventory.GetItem(activeItem);

            if (item == null) { return; }

            currentWorldModel = Instantiate(item.prefab, worldModelTransform);
        }

        void OnClientRemoveItem(string id)
        {
            if (currentViewModel != null)
                Destroy(currentViewModel);
        }

        void OnOthersRemoveItem(string id)
        {
            if (currentWorldModel != null)
                Destroy(currentWorldModel);
        }

        void OnBothRemoveItem(string id)
        {
            var singleton = NetworkManager.singleton as SZNNetworkManager;
            var item = Instantiate(singleton.FindItem(id), player.playerCamera.transform.position, player.playerCamera.transform.rotation);
            NetworkServer.Spawn(item);
        }

        void ModifyViewModelObject(GameObject obj)
        {
            obj.layer = viewModelLayer;

            var rend = obj.GetComponent<Renderer>();

            if (rend == null) { return; }

            rend.shadowCastingMode = ShadowCastingMode.Off;
        }
        #endregion // Item
    }
}
