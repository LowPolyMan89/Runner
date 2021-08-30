using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMoveController : MonoBehaviour
{
    private Transform moveTransform;
    private float playerSpeed;
    private float xAxis;
    private float yAxis;
    private int positionIndx = 1;
    private Transform body;
    private Vector3 EulerAngles;
    [SerializeField] private float chunkAngleTest;
    [SerializeField] private int navPointIndx = 0;
    [SerializeField] private PathChunk currentPathChunk;
    [SerializeField] private bool isJump = false;
    [SerializeField] private bool isSlide = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isCanMove = true;
    [SerializeField] private Animator MoveAnimator;
    [SerializeField] private Animator JumpAnimator;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Player player;
    [SerializeField] private Transform[] points = new Transform[3];

    private float oldJumpGravity = 0f;

    public Transform MoveTransform { get => moveTransform; set => moveTransform = value; }
    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
    public float XAxis { get => xAxis; set => xAxis = value; }
    public float YAxis { get => yAxis; set => yAxis = value; }
    public int PositionIndx { get => positionIndx; set => positionIndx = value; }
    public bool IsCanMoveBool { get => isCanMove; set => isCanMove = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public bool IsSlide { get => isSlide; set => isSlide = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public Player Player { get => player; set => player = value; }
    public Transform Body { get => body; set => body = value; }
    public PathChunk CurrentPathChunk { get => currentPathChunk; set => currentPathChunk = value; }

    private void Start()
    {
        MoveAnimator.SetInteger("Move", positionIndx);
    }

    private void Update()
    {
        playerSpeed = player.Speed;

        if (playerSpeed > 0)
        {
            PlayerAnimator.SetFloat("Speed", playerSpeed);
            if (XAxis > 0.8f && !isJump)
            {

                int nextIndx = positionIndx;
                nextIndx++;

                if (points.Length - 1 >= nextIndx)
                {
                    if (IsCanMove(Vector3.left))
                    {
                        if (isCanMove)
                        {
                            StartCoroutine(IsMove());
                            positionIndx = nextIndx;
                            MoveAnimator.SetInteger("Move", positionIndx);
                            PlayerAnimator.SetInteger("Move", 1);
                        }
                    }
                }

            }

            if (XAxis < -0.8f && !isJump)
            {

                int nextIndx = positionIndx;
                nextIndx--;

                if (nextIndx >= 0)
                {
                    if (IsCanMove(Vector3.right))
                    {
                        if (isCanMove)
                        {
                            StartCoroutine(IsMove());
                            positionIndx = nextIndx;
                            MoveAnimator.SetInteger("Move", positionIndx);
                            PlayerAnimator.SetInteger("Move", -1);
                        }
                    }

                }

            }

            if (YAxis > 0.8f)
            {
                if (!isJump && isGrounded)
                {
                    isGrounded = false;
                    isJump = true;
                    PlayerAnimator.SetBool("Jump", true);
                    StartCoroutine(StopJump());
                }
            }

            if (YAxis < -0.8f)
            {
                if (!isJump && isGrounded)
                {
                    IsSlide = true;
                    PlayerAnimator.SetBool("Slide", true);
                    player.CollisionChecker.CapsuleCollider.height = 0f;
                    player.CollisionChecker.CapsuleCollider.center = new Vector3(0, 0, 0);
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

                    if (currentPathChunk != null)
                    {
                        waypoint = currentPathChunk.navPointsList[navPointIndx].position;
                    }
                }

                var moveDistance = Time.deltaTime * playerSpeed;
                transform.position = Vector3.MoveTowards(transform.position, waypoint, moveDistance);

                if (!IsInRange(transform.position, waypoint, 0.15f))
                {
                    EulerAngles = Quaternion.LookRotation(waypoint - transform.position).eulerAngles;
                }

                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(EulerAngles), chunkAngleTest * Time.deltaTime);
            }
        }

        player.SetNewCameraPosition();
        Camera.main.transform.LookAt(player.CameraLookAtPoint.position);
    }

    private IEnumerator IsMove()
    {
        isCanMove = false;
        yield return new WaitForSeconds(0.5f);
        PlayerAnimator.SetFloat("Strafe", 0);
        PlayerAnimator.SetInteger("Move", 0);
        isCanMove = true;
    }

    private bool IsCanMove(Vector3 dir)
    {
        bool check = true;

        RaycastHit hit;

        if (Physics.Raycast(body.transform.position, dir, out hit, 2f, player.LayerMaskGround))
        {
            if (hit.transform.gameObject.GetComponent<Obstacle>() && !hit.transform.gameObject.GetComponent<Coin>())
            {
                check = false;
            }
        }

        return check;
    }

    private IEnumerator StopJump()
    {
        yield return new WaitForSeconds(player.JumpPower);
        isJump = false;

    }

    private IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(player.SlidePower);
        PlayerAnimator.SetBool("Slide", false);
        player.CollisionChecker.CapsuleCollider.height = 1.5f;
        player.CollisionChecker.CapsuleCollider.center = new Vector3(0, 0.3f, 0);
        IsSlide = false;
    }

    public void PodJump(JumpPod src)
    {
        StartCoroutine(PodJumRoutine(src.JumpTime, src.JumpGravity));
    }

    private IEnumerator PodJumRoutine(float jumpTime, float gravity)
    {
        isGrounded = false;
        isJump = true;
        PlayerAnimator.SetBool("Jump", true);
        oldJumpGravity = player.JumpGravity;
        player.JumpGravity = gravity;

        yield return new WaitForSeconds(jumpTime);

        player.JumpGravity = oldJumpGravity;
        isJump = false;
    }

    private PathChunk GetNextPath()
    {
        if (PositionIndx == 0)
        {
            return currentPathChunk.LeftChunk;
        }
        else if (PositionIndx == 2)
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
}
