using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTest : MonoBehaviour
{
    public Figther Figther1;
    public Figther Figther2;

    public int RoundsMax;
    [SerializeField]private int round = 0;

    [ContextMenu("Start")]
    public void Figth()
    {
        for(int i = 0; i < 100000; i++)
        {
            FighterRoutine();
            if(round >= RoundsMax)
            {
                return;
            }
        }
    }

    public void FighterRoutine()
    {

        if (Figther1.HP > 0 || Figther2.HP > 0)
        {

            Figther2.Attack(Figther1);
            Figther1.Attack(Figther2);
            
        }
        else
        {
            Figther1.HP = 1200;
            Figther2.HP = 1000;
            round++;
        }
       
    }

}

[System.Serializable]
public class Figther
{
    public string FighterName;
    public int HP;
    public int MinDamage;
    public int MaxDamage;
    public float CritChance;
    public float MissChance;
    public int died;

    public void Attack(Figther target)
    {
        int dooDamage = 0;
        float missChance = Random.Range(0, 100);

        if( missChance <= MissChance)
        {
            Debug.Log(FighterName + " промахнулся с шансом + " +  missChance + " / " + MissChance);
            return;
        }

        dooDamage = Random.Range(MinDamage, MaxDamage + 1);

        float critChance = Random.Range(0, 100);

        if (critChance <= CritChance)
        {
            Debug.Log(FighterName + " кританул с шансом + " + critChance + " / " + CritChance + "и нанес " + dooDamage * 2  + " урона");
            target.HP -= dooDamage * 2;
        }
        else
        {
            Debug.Log(FighterName + " нанес " + dooDamage + " урона");
            target.HP -= dooDamage;
        }

        if(target.HP <= 0)
        {
            Debug.Log(target.FighterName + " умер!");
            target.died++;
        }
    }
}