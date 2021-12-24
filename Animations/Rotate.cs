using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public bool antiClockWise;

    public float rotateSpeed;

    void Update()
    {
        transform.Rotate(0, 0, antiClockWise?-rotateSpeed:rotateSpeed);
    }
}
