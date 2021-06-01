using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string BuildingId;
    public int BuildingLevel;
    public List<TimelineEvent> timelineEvents = new List<TimelineEvent>();


    private void Start()
    {
        DataProvider.Instance.AddBuilding(this);
        DataProvider.Instance.Timeline.LoadBuildingTimeline(this);
    }

    [ContextMenu("TestEvent")]
    public void CreateProductionEvent()
    {
        TimelineEvent ev = DataProvider.Instance.Timeline.AddNewTimelineEvent("ProductMainTest", 30f, this);
        //timelineEvents.Add(ev);
    } 

   
}
