using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
 
{
    public int coinCount = 0;
    public int numCoins = 1;
    public static bool canTeleport = false;
    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
        coinCount++;
        canTeleport = true;
        if(coinCount >= numCoins)
        {
            canTeleport = true;
        }
    }
}
