using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] points = new Transform[3];
    [SerializeField] private int currIndx = 1;
    [SerializeField] private float moveDelay = 0.5f;
    [SerializeField] private Transform body;
    [SerializeField] private Animator MoveAnimator;
    [SerializeField] private Animator JumpAnimator;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private GameObject bodyBase;
    [SerializeField] private LayerMask layerMaskGround;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private bool isJump = false;
    [SerializeField] private bool isSlide = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isCanMove = true;
    [SerializeField] private float slidePower;
    [SerializeField] private float jumpPower;
    [SerializeField] private float offsetY;
   [SerializeField] private GameObject contactcollision;
   [SerializeField] private GameObject contactcollider;
    [SerializeField] private bool isPause = false;
    [SerializeField] private List<string> ignoredIsGrowndObjects = new List<string>();
    private float acceleration;

    public GameObject Contactcollision { get => contactcollision; set => contactcollision = value; }
    public GameObject Contactcollider { get => contactcollider; set => contactcollider = value; }

    void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        MoveAnimator.SetInteger("Move", currIndx);
        offsetY = Vector3.Distance(raycastPoint.position, bodyBase.transform.position);
        acceleration = agent.acceleration;

    }

    public void PlayerPause(bool value)
    {
        if(value)
        {
            acceleration = agent.acceleration;
            agent.acceleration = 1000;
        }
        else
        {
            agent.acceleration = acceleration;
        }

        agent.isStopped = value;
    }

    private bool CheckignoredIsGrowndObjects(string id)
    {
        bool _check = false;
        foreach(var v in ignoredIsGrowndObjects)
        {
            if (v.Contains(id))
                _check = true;
        }
        return _check;
    }

    private void CollideCheck()
    {
        if(contactcollision)
        {
            if(contactcollision.tag == "Obstacle")
            {
                Obstacle obstacle = contactcollision.GetComponent<Obstacle>();
                obstacle.Collide(this);
                contactcollision = null;
            }
        }
    }

    private void TriggerCheck()
    {
        if (contactcollider)
        {
            if (contactcollider.tag == "Obstacle")
            {
                Obstacle obstacle = contactcollider.GetComponent<Obstacle>();
                obstacle.Collide(this);
                contactcollider = null;
            }
        }
    }

    public void SppedStop(float value, float time)
    {
        StartCoroutine(SpeedStop(value, time));
    }

    private IEnumerator SpeedStop(float value, float time)
    {
        agent.speed = value;
        agent.acceleration = 1000f;
        
        yield return new WaitForSeconds(time);
        agent.speed = speed;
        agent.acceleration = 5f;


    }

    private IEnumerator IsMove()
    {
        isCanMove = false;
        yield return new WaitForSeconds(moveDelay);
        isCanMove = true;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;    

        if (!isJump)
        {
            if (Physics.Raycast(body.transform.position, -Vector3.up, out hit, offsetY + 0.1f, layerMaskGround))
            {
                
                if (!hit.transform.gameObject.GetComponent<Coin>())
                {
                    if (!CheckignoredIsGrowndObjects(hit.transform.name))
                    {
                        isGrounded = true;
                        bodyBase.transform.position = new Vector3(bodyBase.transform.position.x, hit.point.y + offsetY, bodyBase.transform.position.z);
                    }
                }
            }
            else
            {
                isGrounded = false;
            }
        }

        Debug.DrawRay(body.transform.position, -bodyBase.transform.up * offsetY);

        Gravity();
        CollideCheck();
        TriggerCheck();

    }

    private bool IsCanMove(Vector3 dir)
    {
        bool check = true;

        RaycastHit hit;

        if (Physics.Raycast(body.transform.position, dir, out hit, 2f, layerMaskGround))
        {
            if(hit.transform.gameObject.GetComponent<Obstacle>() && !hit.transform.gameObject.GetComponent<Coin>())
            {
                check = false;
            }
        }

        return check;
    }

    private void Gravity()
    {
        if(!isJump)
        {
            if(!isGrounded)
                bodyBase.transform.position += -Vector3.up * Time.deltaTime * 10f;
        }
        else
        {
            bodyBase.transform.position += Vector3.up * Time.deltaTime * 10f;
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Horizontal") > 0.8f)
        {

            int nextIndx = currIndx;
            nextIndx++;

            if (points.Length - 1 >= nextIndx)
            {
                if (IsCanMove(Vector3.left))
                {
                    if (isCanMove)
                    {
                        StartCoroutine(IsMove());
                        currIndx = nextIndx;
                        MoveAnimator.SetInteger("Move", currIndx);
                    }
                }
            } 
            
        }

        if (Input.GetAxis("Horizontal") < -0.8f)
        {

            int nextIndx = currIndx;
            nextIndx--;

            if (nextIndx >= 0)
            {
                if (IsCanMove(Vector3.right))
                {
                    if (isCanMove)
                    {
                        StartCoroutine(IsMove());
                        currIndx = nextIndx;
                        MoveAnimator.SetInteger("Move", currIndx);
                    }
                }

            } 
            
        }

        if (Input.GetAxis("Vertical") > 0.8f)
        {
            if (!isJump && isGrounded)
            {
                isGrounded = false;
                isJump = true;
                StartCoroutine(StopJump());
            }
        }

        if (Input.GetAxis("Vertical") < -0.8f)
        {
            if (!isJump && isGrounded)
            {
                isSlide = true;
                PlayerAnimator.SetBool("Slide", true);
                StartCoroutine(StopSlide());
            }
        }

        agent.SetDestination(target.position);

    }

    private IEnumerator StopJump()
    {
        yield return new WaitForSeconds(jumpPower);
        isJump = false;
    }

    private IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(slidePower);
        PlayerAnimator.SetBool("Slide", false);
        isSlide = false;
    }
}
