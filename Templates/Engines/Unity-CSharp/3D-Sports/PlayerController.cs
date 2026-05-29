/// <summary>
/// Sports player with movement and kick input, interacts with BallController.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float kickForce = 10f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
        characterController.Move(move * Time.deltaTime);

        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            BallController ball = FindAnyObjectByType<BallController>();
            if (ball != null)
            {
                Vector3 direction = (ball.transform.position - transform.position).normalized;
                ball.Kick(direction, kickForce);
            }
        }
    }
}
