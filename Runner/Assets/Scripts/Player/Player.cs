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
    [SerializeField] private Transform body;
    [SerializeField] private Animator MoveAnimator;
    [SerializeField] private Animator JumpAnimator;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private GameObject bodyBase;
    [SerializeField] private LayerMask layerMaskGround;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private float slidePower;
    [SerializeField] private float jumpPower;
    [SerializeField] private float offsetY;
    [SerializeField] private GameObject contactcollision;
    [SerializeField] private GameObject contactcollider;
    [SerializeField] private bool isPause = false;
    [SerializeField] private List<string> ignoredIsGrowndObjects = new List<string>();
    [SerializeField] private Transform cameraLookAtPoint;
    [SerializeField] private float cameraTurnSpeed;

    private float acceleration;
    public float Velocity;
    private float calculatedVelocity;
    private CollisionChecker collisionChecker;
    private Vector3 startpos;
    private Vector3 startTargetLocalPosition;
    private Vector3 startCamPosition;
    private Vector3 newCameraPosition;
    private LevelController levelController;
    private PlayerMoveController moveController;
    public GameObject Contactcollision { get => contactcollision; set => contactcollision = value; }
    public GameObject Contactcollider { get => contactcollider; set => contactcollider = value; }
    public Transform Body { get => body; set => body = value; }
    public float JumpPower { get => jumpPower; set => jumpPower = value; }
    public float SlidePower { get => slidePower; set => slidePower = value; }
    public CollisionChecker CollisionChecker { get => collisionChecker; set => collisionChecker = value; }
    public LayerMask LayerMaskGround { get => layerMaskGround; set => layerMaskGround = value; }
    public PlayerMoveController MoveController { get => moveController; set => moveController = value; }
    public Transform CameraLookAtPoint { get => cameraLookAtPoint; set => cameraLookAtPoint = value; }
    public float Speed { get => speed; set => speed = value; }

    void Awake()
    {
        moveController = GameObject.FindObjectOfType<PlayerMoveController>();
        moveController.Player = this;
        moveController.Body = body;
        levelController = GameObject.FindObjectOfType<LevelController>();
        offsetY = Vector3.Distance(raycastPoint.position, bodyBase.transform.position);
        collisionChecker = GameObject.FindObjectOfType<CollisionChecker>();
        _casheSpeed = speed;
        moveController.CurrentPathChunk = levelController.FirstChunk;
    }
    private void Start()
    {
        startpos = transform.position;
        startTargetLocalPosition = target.position;
        startCamPosition = Camera.main.transform.localPosition;
        newCameraPosition = startCamPosition;
        DataProvider.Instance.EventManager.OnPlayerDeadAction += PlayerDeath;
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


    public void SetNewCameraPosition()
    {
        if(Vector3.Distance(newCameraPosition, Camera.main.transform.localPosition) > 0.2f)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition , newCameraPosition, Time.deltaTime * cameraTurnSpeed);
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


    private void FixedUpdate()
    {

        CollideCheck();
        TriggerCheck();

    }

    public void PlayerDeath()
    {
        PlayerAnimator.SetBool("Death", true);
    }

    private void Gravity()
    {
        if(!moveController.IsJump)
        {
            if(!moveController.IsGrounded)
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
        moveController.XAxis = Input.GetAxis("Horizontal");
        moveController.YAxis = Input.GetAxis("Vertical");

        RaycastHit hit;

        if (!moveController.IsJump)
        {
            if (Physics.Raycast(body.transform.position, -Vector3.up, out hit, offsetY + 0.1f, layerMaskGround))
            {

                if (!hit.transform.gameObject.GetComponent<Coin>())
                {
                    if (!CheckignoredIsGrowndObjects(hit.transform.name))
                    {
                        moveController.IsGrounded = true;
                        PlayerAnimator.SetBool("Jump", false);
                        bodyBase.transform.position = new Vector3(bodyBase.transform.position.x, hit.point.y + offsetY, bodyBase.transform.position.z);
                    }
                }
            }
            else
            {
                PlayerAnimator.SetBool("Jump", true);
                moveController.IsGrounded = false;
            }
        }
        else
        {
            PlayerAnimator.SetBool("Jump", true);
        }

        Debug.DrawRay(body.transform.position, -bodyBase.transform.up * offsetY);

        Gravity();
    }


    private void OnDestroy()
    {
        DataProvider.Instance.EventManager.OnPlayerDeadAction -= PlayerDeath;
    }
}
