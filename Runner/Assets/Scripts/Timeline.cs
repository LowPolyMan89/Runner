using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private List<TimelineEvent> timelineEvents = new List<TimelineEvent>();
    [SerializeField] private List<TimelineEvent> removeevents = new List<TimelineEvent>();
    [SerializeField] private GameObject timelineEventPrefab;
    public TimelineEventsConfig TimelineEventsConfig;

    public List<TimelineEvent> TimelineEvents { get => timelineEvents; set => timelineEvents = value; }

    public void AddNewTimelineEvent(string ID, double time)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(gameObject.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)time;
        ev.EventEndDate = DateTime.UtcNow.AddSeconds(time);
        TimelineEvents.Add(ev);
    }

    public TimelineEvent AddNewTimelineEvent(string ID, double time, Building building)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(building.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)time;
        ev.EventEndDate = DateTime.UtcNow.AddSeconds(time);

        foreach(var b in DataProvider.Instance.buildings)
        {
            if(b.BuildingId == building.BuildingId)
            {
                b.timelineEvents.Add(ev);
                break;
            }
        }

        return ev;
    }

    public void AddOldTimelineEvent(string ID, float seconds, string time)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(gameObject.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)seconds;

        string inp = time;
        string format = "yyyy-MM-dd HH:mm:ssZ";
        DateTime dt;

        if (!DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        {
            Console.WriteLine("Nope!");
        }
 
        ev.EventEndDate = dt.ToUniversalTime();

        TimelineEvents.Add(ev);
    }

    public void LoadBuildingTimeline(Building selfBuilding)
    {
        foreach(var ev in DataProvider.Instance.Profile.buildingsDatas)
        {
            if(ev.buildingID == selfBuilding.BuildingId)
            {
                if(ev.buildingTimeline.Count > 0)
                {
                    foreach(var evb in ev.buildingTimeline)
                    {
                        AddOldTimelineEvent(evb.EventID, evb.Seconds, evb.EventEndDate, selfBuilding);
                    }
                }
            }
        }
    }

    public void AddOldTimelineEvent(string ID, float seconds, string time, Building building)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(building.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)seconds;

        string inp = time;
        string format = "yyyy-MM-dd HH:mm:ssZ";
        DateTime dt;

        if (!DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        {
            Console.WriteLine("Nope!");
        }

        ev.EventEndDate = dt.ToUniversalTime();

        foreach (var b in DataProvider.Instance.buildings)
        {
            if (b.BuildingId == building.BuildingId)
            {
                b.timelineEvents.Add(ev);
                break;
            }
        }
    }

    public List<TimelineEvent> RemoveNull(List<TimelineEvent> list)
    {
        List<TimelineEvent> newList = new List<TimelineEvent>();
        newList.AddRange(list);

        // Find Fist Null Element in O(n)
        var count = newList.Count;
        for (var i = 0; i < count; i++)
        {
            if (newList[i] == null)
            {
                // Current Position
                int newCount = i++;
                // Copy non-empty elements to current position in O(n)
                for (; i < count; i++)
                {
                    if (newList[i] != null)
                    {
                        newList[newCount++] = newList[i];
                    }
                }
                // Remove Extra Positions O(n)
                newList.RemoveRange(newCount, count - newCount);
                break;
            }
        }

        return newList;
    }

    [ContextMenu("AddTestTimeline")]
    public void AddTestTimeline()
    {
        AddNewTimelineEvent(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ssZ"), 20);
    }

    private void Start()
    {
        StartCoroutine(UpdateTimeline());
    }

    public void DropTimeline()
    {
        foreach(var v in timelineEvents)
        {
            v.Drop();
        }

        timelineEvents.Clear();
    }

    private IEnumerator UpdateTimeline()
    {
        timelineEvents = RemoveNull(timelineEvents);

        foreach (var v in DataProvider.Instance.buildings)
        {
            if (v.timelineEvents.Count > 0)
            {
                v.timelineEvents = RemoveNull(v.timelineEvents);
            }
        }


        yield return new WaitForSeconds(1f);

        foreach (var v in DataProvider.Instance.buildings)
        {
            if (v.timelineEvents.Count > 0)
            {
                foreach(var ev in v.timelineEvents)
                {
                    EventChecker(ev);
                }
            }
        }

        if (TimelineEvents.Count > 0)
        {
            foreach(var v in TimelineEvents)
            {
                EventChecker(v);
            }
        }

        foreach (var rem in removeevents)
        {
            TimelineEvents.Remove(rem);
        }
        removeevents.Clear();

        StartCoroutine(UpdateTimeline());
    }

    private void EventChecker(TimelineEvent timelineEvent)
    {
        if(!timelineEvent)
        {
            return;
        }

        if(timelineEvent.isActive)
        {
            if (timelineEvent.EventEndDate <= DateTime.UtcNow)
            {
                timelineEvent.CompliteEvent();
                removeevents.Add(timelineEvent);
            }
        }
    }
}
