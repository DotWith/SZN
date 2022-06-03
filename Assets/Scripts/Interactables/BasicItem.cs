using Com.Dot.SZN.Interfaces;
using Com.Dot.SZN.InventorySystem;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;

namespace Com.Dot.SZN.Interactables
{
    public class BasicItem : NetworkBehaviour, IInteractable
    {
        public SimpleItem itemInfo;

        [Command(requiresAuthority = false)]
        public void Interact()
        {
            InventoryManager.singleton.AddItem(itemInfo.id, gameObject);
        }
    }
}
