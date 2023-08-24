using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public int speed = 2;
 

    void Update()
    {
        transform.Rotate(0, speed, 0, Space.World);
    }
}
