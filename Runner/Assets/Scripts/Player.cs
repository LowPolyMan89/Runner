using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private float speed;
    private float _casheSpeed;
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
    [SerializeField] private float currentPlayerSpeed;
    private float acceleration;
    public float Velocity;
    private float calculatedVelocity;
    private CollisionChecker collisionChecker;
    private Vector3 startpos;

    public GameObject Contactcollision { get => contactcollision; set => contactcollision = value; }
    public GameObject Contactcollider { get => contactcollider; set => contactcollider = value; }
    public bool IsSlide { get => isSlide; set => isSlide = value; }

    void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        MoveAnimator.SetInteger("Move", currIndx);
        offsetY = Vector3.Distance(raycastPoint.position, bodyBase.transform.position);
        acceleration = agent.acceleration;
        collisionChecker = GameObject.FindObjectOfType<CollisionChecker>();
        _casheSpeed = speed;
    }
    private void Start()
    {
        startpos = transform.position;
        DataProvider.Instance.EventManager.OnPlayerDeadAction += PlayerDeath;
    }

    private void CalculateVelocity()
    {
        Vector3 newpos = transform.position;
        calculatedVelocity = Vector3.Distance(newpos, startpos);
        startpos = transform.position;
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
                if (obstacle)
                {
                    obstacle.Collide(this);
                    contactcollider = null;
                }
            }
        }
    }

    public void SppedStop(float value, float time)
    {
        StartCoroutine(SpeedStop(value, time));
    }

    private IEnumerator SpeedStop(float value, float time)
    {
        speed = value;
        agent.acceleration = 1000f;
        
        yield return new WaitForSeconds(time);
        speed = _casheSpeed;
        agent.acceleration = 5f;


    }

    private IEnumerator IsMove()
    {
        isCanMove = false;
        yield return new WaitForSeconds(moveDelay);
        PlayerAnimator.SetFloat("Strafe", 0);
        PlayerAnimator.SetInteger("Move", 0);
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
                        PlayerAnimator.SetBool("Jump", false);
                        bodyBase.transform.position = new Vector3(bodyBase.transform.position.x, hit.point.y + offsetY, bodyBase.transform.position.z);
                    }
                }
            }
            else
            {
                PlayerAnimator.SetBool("Jump", true);
                isGrounded = false;
            }
        }
        else
        {
            PlayerAnimator.SetBool("Jump", true);
        }

        Debug.DrawRay(body.transform.position, -bodyBase.transform.up * offsetY);

        Gravity();
        CollideCheck();
        TriggerCheck();

    }

    public void PlayerDeath()
    {
        PlayerAnimator.SetBool("Death", true);
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

        agent.speed = speed;

        if (!agent.isStopped)
        {
            PlayerAnimator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
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
                            PlayerAnimator.SetInteger("Move", 1);
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
                            PlayerAnimator.SetInteger("Move", -1);
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
                    PlayerAnimator.SetBool("Jump", true);
                    StartCoroutine(StopJump());
                }
            }

            if (Input.GetAxis("Vertical") < -0.8f)
            {
                if (!isJump && isGrounded)
                {
                    IsSlide = true;
                    PlayerAnimator.SetBool("Slide", true);
                    collisionChecker.CapsuleCollider.height = 0f;
                    collisionChecker.CapsuleCollider.center = new Vector3(0, 0, 0);
                    StartCoroutine(StopSlide());
                }
            }
        }
        else
            currentPlayerSpeed = 0f;
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
        collisionChecker.CapsuleCollider.height = 1.5f;
        collisionChecker.CapsuleCollider.center = new Vector3(0, 0.3f, 0);
        IsSlide = false;
    }

    private void OnDestroy()
    {
        DataProvider.Instance.EventManager.OnPlayerDeadAction -= PlayerDeath;
    }
}
