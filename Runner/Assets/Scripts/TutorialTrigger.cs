using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public SwipeEnum NeedAction;
    private EventManager eventManager;
    private Player player;

    private void Start()
    {
        eventManager = GameObject.FindObjectOfType<EventManager>();
    }

    private SwipeEnum SwipeAction(SwipeEnum arg)
    {
        if(arg == NeedAction)
        {
            Action();
        }
        return arg;
    }

    public void Contact()
    {
        player = GameObject.FindObjectOfType<Player>();
        player.PlayerPause(true);
        eventManager.OnSwipeAction += SwipeAction;
    }

    private void Action()
    {
        player.PlayerPause(false);
        eventManager.OnSwipeAction -= SwipeAction;
        Destroy(this.gameObject);
    }


}
