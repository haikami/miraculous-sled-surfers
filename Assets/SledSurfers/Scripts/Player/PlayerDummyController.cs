using UnityEngine;

public class PlayerDummyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] private float _accelerationForce = 200f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 8f;
    [SerializeField] private float _groundCheckDistance = 5f;
    [SerializeField] private LayerMask _groundMask;

    private bool _isGrounded;
    private Vector3 _groundNormal = Vector3.up;

    private void Awake()
    {
        // Let the Rigidbody handle translation only.
        _rb.freezeRotation = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _rb.AddForce(transform.forward * _accelerationForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        UpdateGroundInfo();
        UpdateRotation();
    }

    private void UpdateGroundInfo()
    {
        RaycastHit hit;

        _isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            out hit,
            _groundCheckDistance,
            _groundMask);

        if (_isGrounded)
        {
            _groundNormal = hit.normal;
        }
    }

    private void UpdateRotation()
    {
        Vector3 velocity = _rb.velocity;

        // Ignore tiny velocities
        if (velocity.sqrMagnitude < 0.1f)
            return;

        Vector3 forward = velocity.normalized;
        Vector3 up;

        if (_isGrounded)
        {
            // Follow the actual ground.
            up = _groundNormal;
        }
        else
        {
            // Predict the landing surface.
            RaycastHit hit;

            if (Physics.Raycast(
                    transform.position,
                    Vector3.down,
                    out hit,
                    50f,
                    _groundMask))
            {
                up = hit.normal;
            }
            else
            {
                up = Vector3.up;
            }
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(forward, up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.fixedDeltaTime);
    }
}