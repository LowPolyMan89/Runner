using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimelineEvent : MonoBehaviour
{
    public string EventID;
    public DateTime EventEndDate;
    public bool isActive = true;

    public virtual void CompliteEvent()
    {
        print("Event " + EventID + " Complited");
        Destroy(this.gameObject, 1f);
    }
}
