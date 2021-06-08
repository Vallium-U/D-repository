using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.left;
    public float rotationSpeed = 2f;

    void Start()
    {
        axis.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
