using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimelineEvent : MonoBehaviour
{
    public string EventID;
    public DateTime EventEndDate;
    public float Seconds;
    public bool isActive = true;
    public bool isComplited = false;
    public Building building;
    public EventAtionType ActionType;

    private void Start()
    {
        gameObject.name = EventID + " / " + EventEndDate.ToString("yyyy-MM-dd HH:mm:ssZ");
    }

    public virtual void CompliteEvent()
    {

        if(building)
        {
            //EventAction();
            isComplited = true;
            print("Event " + EventID + " in Building: " + building.BuildingId + " Complited");
        }
        else
        {
            //EventAction();
            isComplited = true;
            print("Event " + EventID + " Complited");
        }

       // Destroy(this.gameObject, 1f);
    }

    public void Drop()
    {
        Destroy(this.gameObject, 0.1f);
    }

    [ContextMenu("EventAction")]
    public void EventAction()
    {
        switch(ActionType)
        {
            case EventAtionType.AddResources:
                AddResourceAction();
                break;
            case EventAtionType.UpgrageBuilding:
                UpgradeBuildingAction();
                break;
            default:
                break;
        }

        Drop();
    }

    private void UpgradeBuildingAction()
    {
        print("Upgrade building: " + EventID);

        foreach (var rec in DataProvider.Instance.upgradeComponentConfigs)
        {
            if (rec.ComponentID == EventID)
            {
                DataProvider.Instance.Profile.UpgradeBuilding(rec.BuildingId, rec.ComponentOutValue);
            }
        }
    }


    private void AddResourceAction()
    {
        foreach(var rec in DataProvider.Instance.craftComponentConfigs)
        {
            if(rec.ComponentID == EventID)
            {
                DataProvider.Instance.Profile.AddResource(EventID, rec.ComponentOutValue);
            }
        }
    }

}



public enum EventAtionType
{
    AddResources = 0, UpgrageBuilding
}

