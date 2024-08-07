using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float _moveSpeed = 5f;
    public float _rotationSpeed = 50f;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);

        // Rotate player
        if (_moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(_moveInput.y, _moveInput.x) * Mathf.Rad2Deg - 90f;

            float targetRotation = Mathf.LerpAngle(_rb.rotation, angle, _rotationSpeed * Time.fixedDeltaTime);
            _rb.rotation = targetRotation;
        }
    }

    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
}
