using Com.Dot.SZN.Characters;
using Com.Dot.SZN.Interfaces;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : NetworkBehaviour, IInteractable
{
    [SerializeField] SimpleItem itemInfo = new SimpleItem();

    public void Interact() => CmdInteract();

    [Command(requiresAuthority = false)]
    void CmdInteract(NetworkConnectionToClient sender = null)
    {
        sender.identity.GetComponent<Player>().Inventory.AddItem(itemInfo);
        RpcDestoryActor();
    }

    [ClientRpc]
    void RpcDestoryActor()
    {
        Destroy(gameObject);
    }
}
