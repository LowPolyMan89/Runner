using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
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
    [SerializeField] private Transform cameraLookAtPoint;
    [SerializeField] private int navPointIndx = 0;
    [SerializeField] private PathChunk currentPathChunk;
    private float acceleration;
    public float Velocity;
    private float calculatedVelocity;
    private CollisionChecker collisionChecker;
    private Vector3 startpos;
    private Vector3 startTargetLocalPosition;
    private Vector3 startCamPosition;
    private Vector3 newCameraPosition;
    private Vector3 EulerAngles;
    private LevelController levelController;
    public GameObject Contactcollision { get => contactcollision; set => contactcollision = value; }
    public GameObject Contactcollider { get => contactcollider; set => contactcollider = value; }
    public bool IsSlide { get => isSlide; set => isSlide = value; }

    void Awake()
    {
        levelController = GameObject.FindObjectOfType<LevelController>();
        MoveAnimator.SetInteger("Move", currIndx);
        offsetY = Vector3.Distance(raycastPoint.position, bodyBase.transform.position);
        collisionChecker = GameObject.FindObjectOfType<CollisionChecker>();
        _casheSpeed = speed;
        currentPathChunk = levelController.FirstChunk;
    }
    private void Start()
    {
        startpos = transform.position;
        startTargetLocalPosition = target.position;
        startCamPosition = Camera.main.transform.localPosition;
        newCameraPosition = startCamPosition;
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
            speed = 0;
        }
        else
        {
            speed = _casheSpeed;
        }
    }


    private void SetNewCameraPosition()
    {
        if(Vector3.Distance(newCameraPosition, Camera.main.transform.localPosition) > 0.2f)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition , newCameraPosition, Time.deltaTime *3f);
        }
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

        if (!contactcollider)
            return;

        switch (contactcollider.tag)
        {
            case "Obstacle":
                Obstacle obstacle = contactcollider.GetComponent<Obstacle>();
                if (obstacle)
                {
                    obstacle.Collide(this);
                    contactcollider = null;
                }
                break;

            case "Turn":
                TurnTrigger trigger = contactcollider.GetComponent<TurnTrigger>();
 
                if (trigger)
                {
                    target.position = trigger.TurnPoint.position;
                    Destroy(trigger.gameObject);
                }
                break;

            case "CameraMover":
                CameraPositionTrigger triggerCam = contactcollider.GetComponent<CameraPositionTrigger>();

                if (triggerCam)
                {
                    if (!triggerCam.IsReset)
                    {
                        newCameraPosition = Camera.main.transform.localPosition + triggerCam.CamOffset;         
                    }
                    else
                    {
                        newCameraPosition = startCamPosition;
                    }
                }
                Destroy(triggerCam.gameObject);
                break;
        }
    }

    public void SppedStop(float value, float time)
    {
        StartCoroutine(SpeedStop(value, time));
    }

    private IEnumerator SpeedStop(float value, float time)
    {
        speed = value;
        
        yield return new WaitForSeconds(time);
        speed = _casheSpeed;

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
        SetNewCameraPosition();
        Camera.main.transform.LookAt(cameraLookAtPoint.position);

        if (speed > 0)
        {
            PlayerAnimator.SetFloat("Speed", speed);
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

        if (currentPathChunk)
        {
            if (currentPathChunk.navPointsList.Count >= navPointIndx)
            {
                var waypoint = currentPathChunk.navPointsList[navPointIndx].position;
                var pos = transform.position;

                while (IsInRange(pos, waypoint, 0.1f))
                {
                    navPointIndx++;
                    if (navPointIndx >= currentPathChunk.navPointsList.Count)
                    {
                        if (!currentPathChunk.isFinal)
                        {
                            currentPathChunk = GetNextPath();
                            navPointIndx = 0;
                        }

                    }
                    else
                    {
                        navPointIndx--;
                    }

                    if (currentPathChunk != null)
                    {
                        waypoint = currentPathChunk.navPointsList[navPointIndx].position;
                    }
                }

                var moveDistance = Time.deltaTime * speed;
                transform.position = Vector3.MoveTowards(transform.position, waypoint, moveDistance);

                EulerAngles = Quaternion.LookRotation(waypoint - transform.position).eulerAngles;
            }
        }
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(EulerAngles), 10 * Time.deltaTime);
    }

    private PathChunk GetNextPath()
    {
        if(currIndx == 0)
        {
            return currentPathChunk.LeftChunk;
        }
        else if (currIndx == 2)
        {
            return currentPathChunk.RightChunk;
        }
        else
        {
            return currentPathChunk.CenterChunk;
        }


    }

    static bool IsInRange(Vector3 a, Vector3 b, float range)
    {
        return (b - a).sqrMagnitude <= range * range;
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
