using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCrystal : MonoBehaviour
{
    public int healthGain = 10;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            PlayerController playerCt = col.gameObject.GetComponent<PlayerController>();
            playerCt.SetHealth(healthGain);
            Destroy(gameObject);
        }
    }
}
