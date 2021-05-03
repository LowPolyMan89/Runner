using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public BlockDestroyer BlockDestroyer;

    public bool IsSpeedChange = false;
    public float SpeedModifier = 2f;
    public float SppedModifierTime = 1f;
    private Player player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        BlockDestroyer = gameObject.GetComponent<BlockDestroyer>();
    }

    public virtual GameObject Collide(Player player)
    {
        return gameObject;
    }

    public void Activate()
    {
        if(IsSpeedChange)
        {
            player.SppedStop(SpeedModifier, SppedModifierTime);
        }

        if(BlockDestroyer)
        {
            BlockDestroyer.Activate();
        }
    }
}
