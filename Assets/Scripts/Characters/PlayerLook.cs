using Mirror;
using UnityEngine;

namespace Com.Dot.SZN.Characters
{
    public class PlayerLook : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] float mouseSensitivity = 3.5f;

        float cameraPitch = 0.0f;

        public override void OnStartAuthority()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            enabled = true;

            player.Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }

        void Look(Vector2 lookAxis)
        {
            cameraPitch -= lookAxis.y * mouseSensitivity;

            cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

            player.playerCamera.transform.localEulerAngles = Vector3.right * cameraPitch;

            transform.Rotate(Vector3.up * lookAxis.x * mouseSensitivity);
        }
    }
}
