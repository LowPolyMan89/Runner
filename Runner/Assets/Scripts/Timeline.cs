using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private List<TimelineEvent> timelineEvents = new List<TimelineEvent>();
    private TimelineEvent removeevent;
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
        ev.EventEndDate = DateTime.Now.AddSeconds(time);
        TimelineEvents.Add(ev);
    }

    public void AddOldTimelineEvent(string ID, float seconds, string time)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(gameObject.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)seconds;
        ev.EventEndDate = DateTime.Parse(time);
        TimelineEvents.Add(ev);
    }

    public void RemoveNull()
    {
        // Find Fist Null Element in O(n)
        var count = timelineEvents.Count;
        for (var i = 0; i < count; i++)
        {
            if (timelineEvents[i] == null)
            {
                // Current Position
                int newCount = i++;
                // Copy non-empty elements to current position in O(n)
                for (; i < count; i++)
                {
                    if (timelineEvents[i] != null)
                    {
                        timelineEvents[newCount++] = timelineEvents[i];
                    }
                }
                // Remove Extra Positions O(n)
                timelineEvents.RemoveRange(newCount, count - newCount);
                break;
            }
        }
    }

    [ContextMenu("AddTestTimeline")]
    public void AddTestTimeline()
    {
        AddNewTimelineEvent(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), 20);
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
        RemoveNull();

        if (removeevent)
        {
            TimelineEvents.Remove(removeevent);
            removeevent = null;
        }

        yield return new WaitForSeconds(1f);

        if (TimelineEvents.Count > 0)
        {
            foreach(var v in TimelineEvents)
            {
                EventChecker(v);
            }
        }

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

            if(timelineEvent.EventEndDate <= DateTime.Now)
            {
                timelineEvent.CompliteEvent();
                removeevent = timelineEvent;
            }
        }
    }
}
