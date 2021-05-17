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

    private void Start()
    {
        gameObject.name = EventID + " / " + EventEndDate.ToString("yyyy/MM/dd hh:mm:ss");
    }

    public virtual void CompliteEvent()
    {
        print("Event " + EventID + " Complited");
        Destroy(this.gameObject, 1f);
    }

    public void Drop()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
