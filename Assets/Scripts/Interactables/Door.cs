using Com.Dot.SZN.Characters;
using Com.Dot.SZN.Interfaces;
using Mirror;
using System.Collections;
using UnityEngine;

namespace Com.Dot.SZN.Interactables
{
    public enum DoorState : byte { Opened, Closed, Locked }

    public class Door : NetworkBehaviour, IInteractable
    {
        [SyncVar]
        public DoorState doorState;

        [SerializeField] float speed = 1f;

        Vector3 defaultRotation;
        Vector3 forward;

        Coroutine animationCoroutine;

        public void Start()
        {
            defaultRotation = transform.eulerAngles;
            forward = transform.forward;
        }

        public void Interact() => CmdInteract();

        [Command(requiresAuthority = false)]
        void CmdInteract(NetworkConnectionToClient sender = null)
        {
            switch (doorState)
            {
                case DoorState.Opened:
                    RpcCloseDoor();
                    break;
                case DoorState.Closed:
                    RpcOpenDoor(sender.identity.transform.position);
                    break;
                case DoorState.Locked:
                    break;
            }
        }

        [ClientRpc]
        void RpcOpenDoor(Vector3 userPosition)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            float dot = Vector3.Dot(forward, (userPosition - transform.position).normalized);
            animationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }

        [ClientRpc]
        void RpcCloseDoor()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(DoRotationClose());
        }

        IEnumerator DoRotationOpen(float dot)
        {
            float targetRotation = -Mathf.Sign(dot) * 90f;

            doorState = DoorState.Opened;

            float time = 0;
            while (time < 1)
            {
                Quaternion newRotation = Quaternion.Euler(defaultRotation.x, defaultRotation.y + targetRotation, defaultRotation.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, time);
                yield return null;
                time += Time.deltaTime * speed;
            }
        }

        IEnumerator DoRotationClose()
        {
            doorState = DoorState.Closed;

            float time = 0;
            while (time < 1)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotation), time);
                yield return null;
                time += Time.deltaTime * speed;
            }
        }
    }
}
