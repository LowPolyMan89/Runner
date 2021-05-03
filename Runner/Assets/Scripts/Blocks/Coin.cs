using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectable
{
    public int CoinValue = 1;



    public override GameObject Collide(Player player)
    {
        DataProvider.Instance.EventManager.CoinCollectAction();
        LevelStats.Instance.AddCoin(CoinValue);
        Collect();
        Activate();
        return this.gameObject;
    }
}
