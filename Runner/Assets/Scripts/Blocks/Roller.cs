using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roller : MonoBehaviour
{
    [SerializeField] private PathChunk rollerPathChunk;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float rollSpeed;
    [SerializeField] private Transform rollSphere;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isRoll = false;
    [SerializeField] private float rollerSpeed;
    [SerializeField] private int navPointIndx;
    public void StartRoll()
    {
        isRoll = true;
    }

    private void Start()
    {
        meshRenderer.enabled = false;
    }
    private void Update()
    {
        if (rollSphere && isRoll)
            rollSphere.transform.Rotate(Vector3.left, Time.deltaTime * rotationSpeed);

        if (!isRoll)
            return;

        if (rollerPathChunk)
        {
            if (rollerPathChunk.navPointsList.Count >= navPointIndx)
            {
                var waypoint = rollerPathChunk.navPointsList[navPointIndx].position;
                var pos = rollSphere.transform.position;

                while (IsInRange(pos, waypoint, 0.1f))
                {
                    navPointIndx++;
                    if (navPointIndx >= rollerPathChunk.navPointsList.Count)
                    {
                        rollerPathChunk = null;
                    }

                    if (rollerPathChunk != null)
                    {
                        waypoint = rollerPathChunk.navPointsList[navPointIndx].position;
                    }
                }

                var moveDistance = Time.deltaTime * rollerSpeed;
                rollSphere.transform.position = Vector3.MoveTowards(rollSphere.transform.position, waypoint, moveDistance);
            }
        }

    }

    static bool IsInRange(Vector3 a, Vector3 b, float range)
    {
        return (b - a).sqrMagnitude <= range * range;
    }
}
