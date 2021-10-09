using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    public EventManager eventManager;
    public float XAxis;
    public float YAxis;
    public bool isTutorial;
    public TutorialTrigger currentTutorialTrigger;
    public Player player;

    private void Start()
    {
        eventManager = GameObject.FindObjectOfType<EventManager>();
       
    }

    private void Update()
    {
        XAxis = Input.GetAxis("Horizontal");
        YAxis = Input.GetAxis("Vertical");

        if(isTutorial)
        {
            if(player == null)
            {
                player = GameObject.FindObjectOfType<Player>();
            }
            else if(player.Contactcollider)
            {
                currentTutorialTrigger = player.Contactcollider.GetComponent<TutorialTrigger>();
            }
        }

        if (!isTutorial)
        {
            if (XAxis > 0.8f)
            {
                MovePlayer(SwipeEnum.Right);
            }
            if (XAxis < -0.8f)
            {
                MovePlayer(SwipeEnum.Left);
            }
            if (YAxis > 0.8f)
            {
                MovePlayer(SwipeEnum.Up);
            }
            if (YAxis < -0.8f)
            {
                MovePlayer(SwipeEnum.Down);
            }
        }
        else if(currentTutorialTrigger != null)
        {
            if (XAxis > 0.8f && currentTutorialTrigger.NeedAction == SwipeEnum.Right)
            {
                MovePlayer(SwipeEnum.Right);
            }
            if (XAxis < -0.8f && currentTutorialTrigger.NeedAction == SwipeEnum.Left)
            {
                MovePlayer(SwipeEnum.Left);
            }
            if (YAxis > 0.8f && currentTutorialTrigger.NeedAction == SwipeEnum.Up)
            {
                MovePlayer(SwipeEnum.Up);
            }
            if (YAxis < -0.8f && currentTutorialTrigger.NeedAction == SwipeEnum.Down)
            {
                MovePlayer(SwipeEnum.Down);
            }
        }
    }

    public void MovePlayer(SwipeEnum swipeEnum)
    {
        switch (swipeEnum)
        {
            case SwipeEnum.Down:
                eventManager.SwipeAction(SwipeEnum.Down);
                break;
            case SwipeEnum.Up:
                eventManager.SwipeAction(SwipeEnum.Up);
                break;
            case SwipeEnum.Left:
                eventManager.SwipeAction(SwipeEnum.Left);
                break;
            case SwipeEnum.Right:
                eventManager.SwipeAction(SwipeEnum.Right);
                break;
        }
    }
}
