using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    [Header("Player settings")]
    public float _moveSpeed = 5f;
    public float _rotationSpeed = 10f;
    public float _maxHealth = 3f;

    [Header("Prefabs")]
    [SerializeField] GameObject _laser;

    NetworkVariable<Vector2> _moveInput = new NetworkVariable<Vector2>();
    NetworkVariable<float> _health = new NetworkVariable<float>();

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _health.Value = _maxHealth;
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput.Value * _moveSpeed * Time.fixedDeltaTime);

        // Rotate player
        if (_moveInput.Value != Vector2.zero)
        {
            float angle = Mathf.Atan2(_moveInput.Value.y, _moveInput.Value.x) * Mathf.Rad2Deg - 90f;

            float targetRotation = Mathf.LerpAngle(_rb.rotation, angle, _rotationSpeed * Time.fixedDeltaTime);
            _rb.rotation = targetRotation;
        }
    }

    void OnMove(InputValue value)
    {
        if (IsLocalPlayer)
        {
            MoveRPC(value.Get<Vector2>());
        }
    }

    void OnFire()
    {
        if (IsLocalPlayer)
        {
            FireRPC();
        }
    }

    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 value)
    {
        _moveInput.Value = value;
    }

    [Rpc(SendTo.Server)]
    private void FireRPC()
    {
        NetworkObject obj = Instantiate(_laser, transform.position, transform.rotation).GetComponent<NetworkObject>();
        obj.Spawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected");

        if (IsServer && collision.gameObject.tag == "Laser")
        {
            NetworkBehaviour.Destroy(collision.gameObject);
            _health.Value -= 1f;

            if (_health.Value <= 0)
            {
                Debug.Log("You Died");
            }
        }
    }
}
