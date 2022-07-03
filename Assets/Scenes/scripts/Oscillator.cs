using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 maxMovement;
    private Vector3 startPosition;
    private readonly int period = 2; //in seconds


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float degree = Time.time / period * Mathf.PI * 2f;
        float percentage = (Mathf.Sin(degree) + 1) / 2;
        Vector3 movement = maxMovement * percentage;
        transform.position = startPosition + movement;
    }
}
