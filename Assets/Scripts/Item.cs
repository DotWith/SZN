using Com.Dot.SZN.Characters;
using Com.Dot.SZN.Interfaces;
using Mirror;
using UnityEngine;

public class Item : NetworkBehaviour, IInteractable
{
    [SerializeField] GameObject itemPrefab;
    public Collider[] colliders;

    [SyncVar]
    public Transform holder;

    public void Interact() => CmdInteract();

    [Command(requiresAuthority = false)]
    void CmdInteract(NetworkConnectionToClient sender = null)
    {
        sender.identity.GetComponent<PlayerInventory>().AddItem(itemPrefab);
        NetworkServer.Destroy(gameObject);
    }

    public void Update()
    {
        if (holder == null) { return; }

        transform.position = holder.position;
        transform.rotation = holder.rotation;
    }
}
