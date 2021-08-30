using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPod : Obstacle
{
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpGravity;

    public float JumpTime { get => jumpTime; set => jumpTime = value; }
    public float JumpGravity { get => jumpGravity; set => jumpGravity = value; }

    public override GameObject Collide(Player player)
    {
       return player.PodJump(this.gameObject);
    }
}
