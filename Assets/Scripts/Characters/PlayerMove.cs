using UnityEngine;
using Mirror;
using System.Collections;

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
        Vector2 previousInput;

        public override void OnStartAuthority()
        {
            enabled = true;

            player.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            player.Controls.Player.Move.canceled += ctx => ResetMovement();
            player.Controls.Player.Jump.performed += ctx => Jump();
        }

        [ClientCallback]
        public void Update() => Move();

        [Client]
        void SetMovement(Vector2 movement) => previousInput = movement;

        [Client]
        void ResetMovement() => previousInput = Vector2.zero;

        [Client]
        void Move()
        {
            Vector3 right = player.controller.transform.right;
            Vector3 forward = player.controller.transform.forward;
            right.y = 0;
            forward.y = 0;

            if (player.controller.isGrounded)
                velocityY = 0.0f;

            velocityY += gravity * Time.deltaTime;

            Vector3 movement = (right.normalized * previousInput.x + forward.normalized * previousInput.y) * movementSpeed + Vector3.up * velocityY;

            player.controller.Move(movement * Time.deltaTime);
        }

        [Client]
        void Jump()
        {
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

        public void SetMovementSpeedForTime(float speed, float time)
        {
            Debug.Log("How did we get here");
            movementSpeed *= speed;
        }
    }
}
