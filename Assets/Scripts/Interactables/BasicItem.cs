using Com.Dot.SZN.Characters;
using Com.Dot.SZN.Interfaces;
using Com.Dot.SZN.ScriptableObjects;
using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.Interactables
{
    public class BasicItem : NetworkBehaviour, IInteractable
    {
        //[SyncVar]
        public SimpleItem itemInfo;

        [SyncVar, HideInInspector]
        public Transform holder;

        public override void OnStartAuthority()
        {
            Instantiate(itemInfo.viewModel.model, transform);
        }

        public void Start()
        {
            if (hasAuthority) { return; }

            Instantiate(itemInfo.worldModel.model, transform);
        }

        public void Interact() => CmdInteract();

        [Command(requiresAuthority = false)]
        void CmdInteract(NetworkConnectionToClient sender = null)
        {
            sender.identity.GetComponent<PlayerInventory>().AddItem(itemInfo);
            NetworkServer.Destroy(gameObject);
        }

        public void LateUpdate()
        {
            if (holder == null) { return; }

            transform.position = holder.position;
            transform.rotation = holder.rotation;
        }
    }
}
