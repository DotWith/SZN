using Com.Dot.SZN.InventorySystem;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Com.Dot.SZN.Characters
{
    [RequireComponent(typeof(Inventory))]
    public class PlayerInventory : NetworkBehaviour
    {
        [SerializeField] Player player;
        [SerializeField] Transform viewModelTransform;
        [SerializeField] int viewModelLayer;

        int localActiveItem;
        Inventory inventory;

        GameObject currentViewModel;

        public void Start()
        {
            inventory = GetComponent<Inventory>();
        }

        #region Input Actions
        public void ChangeItemToFirst(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            ChangeItem(0);
        }

        public void ChangeItemToSeconded(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            ChangeItem(1);
        }

        public void UseActiveItem(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            inventory.UseItem(localActiveItem);
        }

        public void RemoveActiveItem(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            RemoveItem();
        }
        #endregion // Input Actions

        void ChangeItem(int index)
        {
            inventory.ChangeItem(index);
            localActiveItem = index;
            SyncItems();
        }

        void SyncItems()
        {
            if (currentViewModel != null)
                Destroy(currentViewModel);

            var item = InventoryManager.singleton.GetItem(localActiveItem);
            
            if (item == null) { return; }

            currentViewModel = Instantiate(item.prefab, viewModelTransform);

            ModifyViewModelObject(currentViewModel.gameObject);

            for (int i = 0; i < currentViewModel.transform.childCount; i++)
            {
                var child = currentViewModel.transform.GetChild(i);
                ModifyViewModelObject(child.gameObject);
            }
        }

        void RemoveItem()
        {
            inventory.RemoveItem(localActiveItem);

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
