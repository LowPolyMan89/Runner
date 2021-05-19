using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTrigger : MonoBehaviour
{
    public Color32 color;
    [SerializeField] private Transform turnPoint;

    public Transform TurnPoint { get => turnPoint; set => turnPoint = value; }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x * 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, turnPoint.position);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(turnPoint.position, 1f);
    }
}
