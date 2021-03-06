using Com.Dot.SZN.Interfaces;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Dot.SZN.Characters
{
    public class PlayerInteract : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] float scanDistance;
        [SerializeField] float scanRadius;
        [SerializeField] LayerMask interactableLayer;
        [SerializeField] LayerMask excludeLayer;

        List<IInteractable> nearInteractables = new List<IInteractable>();
        GameObject selectedInteractable = null;

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (ctx.performed)
            {
                PerformInteract();
            }
            else
            {
                CancelInteract();
            }
        }

        void PerformInteract()
        {
            SearchInteractables();

            if (selectedInteractable == null) { return; }

            IInteractable target = selectedInteractable.GetComponent<IInteractable>();
            if (target == null) { return; }

            target.Interact();
        }

        void CancelInteract()
        {
            nearInteractables.Clear();
            selectedInteractable = null;
        }

        void SearchInteractables()
        {
            if (GetCameraRay(player.playerCamera, out Ray ray))
            {
                float scanDistanceLocal = scanDistance * Mathf.Clamp(Vector3.Angle(Vector3.up, ray.direction) / 90f, 0.5f, 1f);

                selectedInteractable = GetNearbyObject(ray, scanDistanceLocal, scanRadius);
            }

            if (selectedInteractable == null) { return; }

            nearInteractables = selectedInteractable.GetComponents<IInteractable>().ToList();
        }

        bool GetCameraRay(Camera cam, out Ray outRay)
        {
            outRay = new Ray(cam.transform.position, cam.transform.forward);
            return cam != null;
        }

        GameObject Raycast(Ray ray, float distance)
        {
            int mask = 1 << interactableLayer.value | excludeLayer.value;
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, distance, mask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject.layer == interactableLayer.value)
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        GameObject SphereCast(Ray ray, float distance, float radius)
        {
            RaycastHit[] interactables = Physics.SphereCastAll(ray, radius, distance, interactableLayer.value, QueryTriggerInteraction.Ignore);

            if (interactables.Length < 0) { return null; }

            RaycastHit[] hits = (from interactable in interactables
                    where interactable.collider != null
                    select interactable into nearInteractable
                    orderby Vector2.Distance(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f),
                    player.playerCamera.WorldToScreenPoint(nearInteractable.point))
                    select nearInteractable).ToArray();

            foreach (RaycastHit hit in hits)
            {
                if (Physics.Linecast(ray.origin, hit.point, out RaycastHit hitInfo, excludeLayer, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider.gameObject.layer == hit.collider.gameObject.layer)
                    {
                        return hitInfo.collider.gameObject;
                    }
                    continue;
                }
                return hit.collider.gameObject;
            }

            return null;
        }

        public GameObject GetNearbyObject(Ray ray, float distance, float radius)
        {
            GameObject scannedObject = Raycast(ray, distance);
            return scannedObject != null ? scannedObject : SphereCast(ray, distance, radius);
        }
    }
}
