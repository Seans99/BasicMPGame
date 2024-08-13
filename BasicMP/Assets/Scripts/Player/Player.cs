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

    [Header("Ships")]
    [SerializeField] Sprite[] _ships;

    NetworkVariable<Vector2> _moveInput = new NetworkVariable<Vector2>();
    NetworkVariable<float> _health = new NetworkVariable<float>();

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Set health to max health
        if (IsServer)
        {
            _health.Value = _maxHealth;
        }

        // Change ship depending on p1 or p2
        if (_ships.Length != 0)
        {
            if (NetworkObject.OwnerClientId == 0)
            {
                GetComponent<SpriteRenderer>().sprite = _ships[0];
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = _ships[1];
            }
        }
    }

    void FixedUpdate()
    {
        // Move player
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
        // Spawn laser bullet
        Vector3 laserSpawnPosition = transform.position + transform.up;
        NetworkObject obj = Instantiate(_laser, laserSpawnPosition, transform.rotation).GetComponent<NetworkObject>();
        obj.Spawn();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reduce health if hit by laser
        if (IsServer && collision.gameObject.tag == "Laser")
        {
            _health.Value -= 1f;

            if (_health.Value <= 0)
            {
                NetworkObject.Despawn(gameObject);
            }
        }
    }
}
