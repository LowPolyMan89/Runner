using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangerTrigger : MonoBehaviour
{
    [SerializeField] private MeshRenderer changRenderer;
    [SerializeField] private Material old;
    [SerializeField] private Material change;

    private void Start()
    {
        old = changRenderer.materials[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CollisionChecker>())
        {
            changRenderer.sharedMaterial = change;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CollisionChecker>())
        {
            changRenderer.materials[0] = old;
        }
    }
}
