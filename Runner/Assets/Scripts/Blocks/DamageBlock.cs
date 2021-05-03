using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : Collectable
{
    public int DamageValue = -1;

    public override GameObject Collide(Player player)
    {
        DataProvider.Instance.EventManager.TakeDamageAction(DamageValue);
        //LevelStats.Instance.AddHitPoint(DamageValue);
        Collect();
        Activate();
        return this.gameObject;
    }
}
