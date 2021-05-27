using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{

    public Animator MoveAnimator;
    public Animator BodyAnimator;
    [SerializeField] private float moveDelay = 3f;
    private const float IdleTime = 0.5f;
    private int positionIndx = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Mover());
    }

    private void RandomMove()
    {
        int rnd = Random.Range(1, 3);
        int indx = positionIndx;

        if(rnd > 1)
        {
            if(indx < 2)
            {
                indx++;
            }
        }
        else
        {
            if (indx > 0)
            {
                indx--;
            }
        }

        if(positionIndx != indx)
        {
            positionIndx = indx;

            MoveAnimator.SetInteger("Move", positionIndx);
        }

    }

    private IEnumerator Mover()
    {
        yield return new WaitForSeconds(moveDelay);
        RandomMove();
        StartCoroutine(IdleTimeRoutine());
        StartCoroutine(Mover());
    }

    private IEnumerator IdleTimeRoutine()
    {
        BodyAnimator.SetBool("Move", true);
        yield return new WaitForSeconds(IdleTime);
        BodyAnimator.SetBool("Move", false);
    }
}
