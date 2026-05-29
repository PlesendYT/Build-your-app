using UnityEngine;

/// <summary>Controls the player character with horizontal movement and jumping.</summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private LayerMask _groundLayer;
    
    private Rigidbody2D? _rb;
    private bool _isGrounded;

    /// <summary>Cache the Rigidbody2D component on awake.</summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>Reads input each frame and applies horizontal velocity and jumping.</summary>
    private void Update()
    {
        // Horizontal movement
        var horizontal = Input.GetAxis("Horizontal");
        var movement = new Vector2(horizontal * _speed, _rb?.linearVelocity.y ?? 0);
        
        if (_rb is not null)
            _rb.linearVelocity = movement;

        // Jump when grounded and jump button pressed
        if (Input.GetButtonDown("Jump") && _isGrounded && _rb is not null)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>Sets grounded flag when touching a ground-layer collider.</summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) != 0)
            _isGrounded = true;
    }

    /// <summary>Clears grounded flag when leaving a ground-layer collider.</summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) != 0)
            _isGrounded = false;
    }
}
