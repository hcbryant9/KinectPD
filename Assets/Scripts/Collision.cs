using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    //script that makes collision with bullet reset the players pos
    private Vector3 originalPosition;
    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet")
        {
            transform.position = originalPosition;
            Debug.Log("hit, now we are teleporting you");
        }
    }
}
