using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathChunk : MonoBehaviour
{
    public List<Transform> navPointsList;
    public PathChunk LeftChunk;
    public PathChunk RightChunk;
    public PathChunk CenterChunk;
    public bool isFinal;
    private void Awake()
    {
        if(LeftChunk == null && RightChunk == null && CenterChunk == null)
        {
            isFinal = true;
        }

        foreach (Transform t in transform)
        {
            navPointsList.Add(t);
        }
    }
}
