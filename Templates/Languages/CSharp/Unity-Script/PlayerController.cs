using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private LayerMask _groundLayer;
    
    private Rigidbody2D? _rb;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var movement = new Vector2(horizontal * _speed, _rb?.linearVelocity.y ?? 0);
        
        if (_rb is not null)
            _rb.linearVelocity = movement;

        if (Input.GetButtonDown("Jump") && _isGrounded && _rb is not null)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) != 0)
            _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((_groundLayer.value & (1 << collision.gameObject.layer)) != 0)
            _isGrounded = false;
    }
}
