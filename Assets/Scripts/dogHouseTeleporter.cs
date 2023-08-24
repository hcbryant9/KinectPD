using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class dogHouseTeleporter : MonoBehaviour
{

   
    private void OnTriggerEnter(Collider collision)
    {
        //(collision.gameObject.tag == "Player")
        if ((Collect.canTeleport))
        {
            SceneManager.LoadScene("Test");
        }
    }
}
