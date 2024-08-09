using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Laser : NetworkBehaviour
{
    [SerializeField] private float _speed = 12f;
    [SerializeField] private float _timeUntilDestroy = 3f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

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
        NetworkBehaviour.Destroy(gameObject);
    }
}
