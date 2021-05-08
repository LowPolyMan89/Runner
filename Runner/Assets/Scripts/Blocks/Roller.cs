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
    private bool isRoll = false;
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
        if(isRoll)
            agent.destination = navTarget.position;
    }
}
