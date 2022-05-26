using Com.Dot.SZN.Characters;
using Com.Dot.SZN.Interfaces;
using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.Interactables
{
    public class BasicItem : NetworkBehaviour, IInteractable
    {
        public Collider[] colliders;

        [SyncVar]
        public Transform holder;

        public void Interact() => CmdInteract();

        [Command(requiresAuthority = false)]
        void CmdInteract(NetworkConnectionToClient sender = null)
        {
            sender.identity.GetComponent<PlayerInventory>().AddItem();
            NetworkServer.Destroy(gameObject);
        }

        public void Update()
        {
            if (holder == null) { return; }

            transform.position = holder.position;
            transform.rotation = holder.rotation;
        }
    }
}
