using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 12f;

    void FixedUpdate()
    {
        transform.Rotate(0f, rotSpeed, 0f);
    }
}
