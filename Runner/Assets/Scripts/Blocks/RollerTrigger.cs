using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerTrigger : MonoBehaviour
{
    [SerializeField] private Roller roller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CollisionChecker>())
        {
            roller.StartRoll();
        }
    }
}
