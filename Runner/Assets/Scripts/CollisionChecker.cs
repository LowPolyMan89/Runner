using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(player)
         player.Contactcollision = collision.gameObject;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (player)
            player.Contactcollision = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player)
            player.Contactcollider = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (player)
            player.Contactcollider = null;
    }
}
