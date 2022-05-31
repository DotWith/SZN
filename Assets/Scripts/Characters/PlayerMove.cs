using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.InputSystem;

namespace Com.Dot.SZN.Characters
{
    public class PlayerMove : NetworkBehaviour
    {
        [SerializeField] Player player;

        [Space]
        [SerializeField] float gravity = -13.0f;
        [SerializeField] float movementSpeed;
        [SerializeField] float jumpForce;

        bool isJumping = false;
        float velocityY = 0.0f;
        Vector2 inputMovement;

        [ClientCallback]
        public void Update() => Move();

        [Client]
        void Move()
        {
            if (!isLocalPlayer) { return; }

            Vector3 right = player.controller.transform.right;
            Vector3 forward = player.controller.transform.forward;
            right.y = 0;
            forward.y = 0;

            if (player.controller.isGrounded)
                velocityY = 0.0f;

            velocityY += gravity * Time.deltaTime;

            Vector3 movement = (right.normalized * inputMovement.x + forward.normalized * inputMovement.y) * movementSpeed + Vector3.up * velocityY;

            player.controller.Move(movement * Time.deltaTime);
        }

        public void SetMovement(InputAction.CallbackContext ctx) => inputMovement = ctx.ReadValue<Vector2>();

        [Client]
        public void Jump(InputAction.CallbackContext ctx)
        {
            if (!isLocalPlayer) { return; }

            if (!ctx.performed) { return; }

            if (isJumping) { return; }

            isJumping = true;
            StartCoroutine(JumpEvent());
        }

        IEnumerator JumpEvent()
        {
            do
            {
                player.controller.Move(Vector3.up * jumpForce * Time.deltaTime);
                yield return null;
            } while (!player.controller.isGrounded && player.controller.collisionFlags != CollisionFlags.Above);

            isJumping = false;
        }

        public void SetMovementSpeed(float speed, float time)
        {
            StartCoroutine(SetMovementSpeedForTime(speed, time));
        }

        IEnumerator SetMovementSpeedForTime(float speed, float time)
        {
            movementSpeed *= speed;
            yield return new WaitForSeconds(time);
            movementSpeed /= speed;
        }
    }
}
