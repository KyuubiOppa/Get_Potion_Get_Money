using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public bool spin = false;

    [Header("====== Spin ======")]
    public float spinSpeed = 100f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = initialPosition + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;

        if (spin)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        transform.position = initialPosition;
    }
}
