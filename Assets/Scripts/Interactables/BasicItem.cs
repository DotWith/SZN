using Com.Dot.SZN.Interfaces;
using Com.Dot.SZN.InventorySystem;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;

namespace Com.Dot.SZN.Interactables
{
    public class BasicItem : NetworkBehaviour, IInteractable
    {
        public SimpleItem itemInfo;

        public void Interact() => CmdInteract();

        [Command(requiresAuthority = false)]
        void CmdInteract(NetworkConnectionToClient sender = null)
        {
            if (sender.identity.GetComponent<Inventory>().AddItem(itemInfo.id))
                NetworkServer.Destroy(gameObject);
        }
    }
}
