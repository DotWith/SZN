using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Com.Dot.SZN.Characters
{
    public class PlayerLook : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] float mouseSensitivity = 3.5f;

        float cameraPitch = 0.0f;

        public override void OnStartLocalPlayer()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public override void OnStopLocalPlayer()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Look(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (player.playerCamera == null) { return; }

            Vector2 lookAxis = ctx.ReadValue<Vector2>();

            if (!ctx.performed) { return; }

            cameraPitch -= lookAxis.y * mouseSensitivity;

            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

            player.playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;

            transform.Rotate(Vector3.up * lookAxis.x * mouseSensitivity);
        }
    }
}
