using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string BuildingId;
    public int BuildingLevel;
    public List<TimelineEvent> timelineEvents = new List<TimelineEvent>();
    public List<GameObject> buildingLevelPrefab = new List<GameObject>();
    public Crafter CrafterComponent;


    private void Start()
    {
        DataProvider.Instance.AddBuilding(this);
        DataProvider.Instance.Timeline.LoadBuildingTimeline(this);
    }

    [ContextMenu("TestEvent")]
    public void CreateProductionEvent(string productId, float time)
    {
        TimelineEvent ev = DataProvider.Instance.Timeline.AddNewTimelineEvent(productId, time, this, EventAtionType.AddResources);
        //timelineEvents.Add(ev);
    }

    [ContextMenu("TestUpgrdeEvent")]
    public void CreateUpgradeEvent(string productId, float time)
    {
        TimelineEvent ev = DataProvider.Instance.Timeline.AddNewTimelineEvent(productId, time, this, EventAtionType.UpgrageBuilding);
        //timelineEvents.Add(ev);
    }

    internal void Init()
    {
        foreach (var v in buildingLevelPrefab)
        {
            v.SetActive(false);
        }
        print("Init " + BuildingId + " building");
        buildingLevelPrefab[BuildingLevel].SetActive(true);
    }
}
