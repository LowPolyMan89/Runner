using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraPositionTrigger : MonoBehaviour
{
    public Color32 color;
    [SerializeField] private Vector3 camOffset;

    [SerializeField] private bool isReset = false;

    public Vector3 CamOffset { get => camOffset; set => camOffset = value; }
    public bool IsReset { get => isReset; set => isReset = value; }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z));
    }

}
