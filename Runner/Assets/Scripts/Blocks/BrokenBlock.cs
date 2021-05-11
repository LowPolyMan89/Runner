using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBlock : Collectable
{
    public int DamageValue = -1;

    public override GameObject Collide(Player player)
    {
        if (!player.IsSlide)
        {
            DataProvider.Instance.EventManager.TakeDamageAction(DamageValue);
        }
        //LevelStats.Instance.AddHitPoint(DamageValue);
        Collect();
        Activate();
        return this.gameObject;
    }
}
