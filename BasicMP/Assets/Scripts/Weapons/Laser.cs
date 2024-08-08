using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 12f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

    }
}
