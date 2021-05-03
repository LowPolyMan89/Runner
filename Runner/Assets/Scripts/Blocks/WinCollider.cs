using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : Obstacle
{
    public override GameObject Collide(Player player)
    {
        DataProvider.Instance.EventManager.LevelWinAction(LevelStats.Instance.LevelId);
        Activate();
        return player.gameObject;
    }
}
