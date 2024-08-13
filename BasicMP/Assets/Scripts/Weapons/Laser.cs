using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Laser : NetworkBehaviour
{
    [SerializeField] private float _speed = 12f;
    [SerializeField] private float _timeUntilDestroy = 3f;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Continues movement 
        _rb.velocity = transform.up * _speed;
    }

    void Update()
    {
        // Despawn after a period of time
        if (IsServer)
        {
            _timeUntilDestroy -= Time.deltaTime;
            if (_timeUntilDestroy <= 0)
            {
                DestroyObjectRPC();
            }
        }
    }

    [Rpc(SendTo.Server)]
    void DestroyObjectRPC()
    {
        NetworkObject.Despawn(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Despawn if hit on player
        if (collision.gameObject.tag == "Player" && IsServer)
        {
            NetworkObject.Despawn(gameObject);
        }
    }
}
