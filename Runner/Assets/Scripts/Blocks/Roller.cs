using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roller : MonoBehaviour
{
    [SerializeField] private Transform navTarget;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float rollSpeed;
    [SerializeField] private Transform rollSphere;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRoll = false;
    public void StartRoll()
    {
        isRoll = true;
    }

    private void Start()
    {
        meshRenderer.enabled = false;
        agent.speed = rollSpeed;
    }
    private void Update()
    {
        if (rollSphere && isRoll)
            rollSphere.transform.Rotate(Vector3.left, Time.deltaTime * rotationSpeed);
        if(isRoll)
            agent.destination = navTarget.position;
    }
}
