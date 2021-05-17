using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timeline : MonoBehaviour
{
    [SerializeField] private List<TimelineEvent> timelineEvents = new List<TimelineEvent>();
    private TimelineEvent removeevent;
    [SerializeField] private GameObject timelineEventPrefab;
    public TimelineEventsConfig TimelineEventsConfig;

    public void AddNewTimelineEvent(string ID, double time)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(gameObject.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.EventEndDate = DateTime.Now.AddSeconds(time);
        timelineEvents.Add(ev);
    }

    private void Start()
    {
        AddNewTimelineEvent("Test 1", 5);
        AddNewTimelineEvent("Test 2", 10);
        StartCoroutine(UpdateTimeline());
    }

    private IEnumerator UpdateTimeline()
    {
        if (removeevent)
        {
            timelineEvents.Remove(removeevent);
            removeevent = null;
        }

        yield return new WaitForSeconds(1f);

        if (timelineEvents.Count > 0)
        {
            foreach(var v in timelineEvents)
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
