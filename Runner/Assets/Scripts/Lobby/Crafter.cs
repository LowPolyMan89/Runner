using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
    [SerializeField] private Building building;
    [SerializeField] private List<CraftComponentsPerLevel> craftComponentConfigs = new List<CraftComponentsPerLevel>();
    [SerializeField] private List<CraftComponentConfig> upgradeComponentConfigs = new List<CraftComponentConfig>();
    [SerializeField] private DataProvider dataProvider;
    private List<RemoveResources> removeResources = new List<RemoveResources>();

    public List<CraftComponentConfig> UpgradeComponentConfigs { get => upgradeComponentConfigs;}
    public List<CraftComponentsPerLevel> CraftComponentConfigs { get => craftComponentConfigs;}

    private void Start()
    {
        dataProvider = DataProvider.Instance;
    }

    [ContextMenu("CraftPlanks")]
    public void CraftPlanks()
    {
        StartCraft("Planks");
    }

    [ContextMenu("Upgrde_1")]
    public void Upgrade()
    {
        StartUpgrage("Main_Building_Level_1");
    }

    public void StartUpgrage(string upgradeID)
    {
        bool isCanCraft = true;
        bool isHaveResources = true;
        bool isHaveMoney = true;

        float craftTime = 0f;
        removeResources.Clear();

        foreach (var c in UpgradeComponentConfigs)
        {
            if (c.ComponentID != upgradeID)
            {
                isCanCraft = false;
                print("Cant find ID in Configs");
                return;
            }
            else
            {
                if (c.Money > dataProvider.Profile.GetResource("Money"))
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
                    building.CreateUpgradeEvent(upgradeID, craftTime);
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


    public void StartCraft(string craftcomponentId)
    {

        bool isCanCraft = true;
        bool isHaveResources = true;
        bool isHaveMoney = true;

        float craftTime = 0f;
        removeResources.Clear();

        foreach(var c in CraftComponentConfigs[building.BuildingLevel].craftComponentConfigs)
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

[System.Serializable]
public class CraftComponentsPerLevel
{
    public List<CraftComponentConfig> craftComponentConfigs = new List<CraftComponentConfig>();
}
