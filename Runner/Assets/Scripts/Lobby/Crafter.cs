using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
    [SerializeField] private Building building;
    [SerializeField] private List<CraftComponentConfig> craftComponentConfigs = new List<CraftComponentConfig>();
    [SerializeField] private DataProvider dataProvider;
    private List<RemoveResources> removeResources = new List<RemoveResources>();

    private void Start()
    {
        dataProvider = DataProvider.Instance;
    }

    [ContextMenu("CraftPlanks")]
    public void CraftPlanks()
    {
        StartCraft("Planks");
    }


    public void StartCraft(string craftcomponentId)
    {

        bool isCanCraft = true;
        bool isHaveResources = true;
        bool isHaveMoney = true;

        float craftTime = 0f;
        removeResources.Clear();

        foreach(var c in craftComponentConfigs)
        {
            if(c.ComponentID != craftcomponentId)
            {
                isCanCraft = false;
                print("Cant find ID in Configs");
                return;
            }
            else
            {
                if(c.Money > dataProvider.Profile.GetResource("Money"))
                {
                    isHaveMoney = false;
                    print("No money");
                    return;
                }

                foreach (var d in c.needcraftComponents)
                {
                    int needvalue = 0;
                    int havevalue = dataProvider.Profile.GetResource(d.ComponentID);
                    needvalue = d.Value;

                    if (havevalue < needvalue)
                    {
                        isHaveResources = false;
                        print("Resource name: " + d.ComponentID + " less then " + needvalue);
                    }
                    else
                    {
                        removeResources.Add(new RemoveResources(d.ComponentID, d.Value));
                    }
                }

                craftTime = c.CraftTime;

                if (isCanCraft && isHaveResources && isHaveMoney)
                {
                    dataProvider.Profile.SubstractResource("Money", c.Money);
                    building.CreateProductionEvent(craftcomponentId, craftTime);
                    foreach (var res in removeResources)
                    {
                        dataProvider.Profile.SubstractResource(res.Id, res.Count);
                    }
                    dataProvider.SaveLoad(SaveLoadEnum.Save);
                }

                return;
            }
        }
    }

    public class RemoveResources
    {
        public string Id;
        public int Count;
        public RemoveResources(string id, int value)
        {
            Id = id;
            Count = value;
        }
    }
}
