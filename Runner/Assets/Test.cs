using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    [SerializeField] private int mask = 0;
    NavMeshAgent agent;
   [SerializeField] private float speed;
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    [SerializeField] private Transform[] points = new Transform[3];
    [SerializeField]private int currIndx = 1;
    [SerializeField] private Transform body;
    [SerializeField] private Transform currentPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animationJump;
    private bool isJump = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentPosition = points[1];
        animator.SetInteger("Move", currIndx);
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.D))
        {
            int nextIndx = currIndx;
            nextIndx++;

           if(points.Length - 1 >= nextIndx)
            {
                MovePlayer(nextIndx);
                currIndx = nextIndx;
                animator.SetInteger("Move", currIndx);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            int nextIndx = currIndx;
            nextIndx--;

            if (nextIndx >= 0)
            {
                MovePlayer(nextIndx);
                currIndx = nextIndx;
                animator.SetInteger("Move", currIndx);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                isJump = true;
                animationJump.SetBool("Jump", true);
                StartCoroutine(StopJump());
            }
        }

        agent.SetDestination(target.position);
        
    }

    private IEnumerator StopJump()
    {
        yield return new WaitForSeconds(1f);
        animationJump.SetBool("Jump", false);
        isJump = false;
    }
    private void MovePlayer(int movePosition)
    {
        //switch(movePosition)
        //{
        //    case 1:
        //        body.position = points[1].position;
        //        break;
        //    case 0:
        //        body.position = points[0].position;
        //        break;
        //    case 2:
        //        body.position = points[2].position;
        //        break;
        //}
    }

    private enum MovePosition
    {
        L,M,R
    }
}
