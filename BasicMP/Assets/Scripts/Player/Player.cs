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

    [Header("Prefabs")]
    [SerializeField] GameObject _laser;

    NetworkVariable<Vector2> _moveInput = new NetworkVariable<Vector2>(writePerm: NetworkVariableWritePermission.Owner);

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (IsServer)
        {
            _rb.MovePosition(_rb.position + _moveInput.Value * _moveSpeed * Time.fixedDeltaTime);
        }

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
        MoveRPC(value.Get<Vector2>());
    }

    void OnFire()
    {
        FireRPC();
    }

    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 value)
    {
        if (IsLocalPlayer)
        {
            _moveInput.Value = value;
        }
    }

    [Rpc(SendTo.Server)]
    private void FireRPC()
    {
        if (IsLocalPlayer && IsServer)
        {
            NetworkObject obj = Instantiate(_laser, transform.position, transform.rotation).GetComponent<NetworkObject>();
            obj.Spawn();
        }
    }
}
