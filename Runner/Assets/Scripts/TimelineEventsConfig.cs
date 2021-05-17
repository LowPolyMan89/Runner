using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Configs", menuName = "Configs/TimelineEventsConfig", order = 1)]
public class TimelineEventsConfig : ScriptableObject
{
    public List<TimleniEventConfigElement> timleniEventConfigElements = new List<TimleniEventConfigElement>();

    [System.Serializable]
    public class TimleniEventConfigElement
    {
        public string ID;
        public TimelineEvent timelineEvent;
    }
}


