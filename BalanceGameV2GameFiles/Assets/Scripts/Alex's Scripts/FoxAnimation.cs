using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class FoxAnimation : MonoBehaviour
{
    [Header("Pathfinding Input")]
    [SerializeField] private LayerMask checkGround;
    [SerializeField] Transform wayPoints;
    UnityEngine.AI.NavMeshAgent foxAgent;


    [Header("Animation Input")]
    [SerializeField] Animator foxAnimator;

    public bool isIdling, isLooking, isWalking, isRunning, isJumping, isAttacking, isEating, isBiting;

    public State animationState;
    public enum State
    {
        Idling, Looking, Walking, Running, Jumping, Attacking, Eating, Biting
    }


    [Header("Movement Input")]
    public float movementSpeed = 20f;
    public float runSpeed = 40f;

    [SerializeField] Rigidbody foxRB;
    private Vector3 previousPosition;


    [Header("Grounding Input")]
    [SerializeField] private Transform rightFrontFootTransform;
    [SerializeField] private Transform rightHindFootTransform;
    [SerializeField] private Transform leftFrontFootTransform;
    [SerializeField] private Transform leftHindFootTransform;
    private Transform[] allFootTransforms;
    [SerializeField] private Transform rightFrontTargetTransform;
    [SerializeField] private Transform rightHindTargetTransform;
    [SerializeField] private Transform leftFrontTargetTransform;
    [SerializeField] private Transform leftHindTargetTransform;
    private Transform[] allTargetTransforms;
    [SerializeField] private GameObject rFrontFootRig;
    [SerializeField] private GameObject rHindFootRig;
    [SerializeField] private GameObject lFrontFootRig;
    [SerializeField] private GameObject lHindFootRig;
    private TwoBoneIKConstraint[] allFootIkConstraints;
    [SerializeField] private LayerMask groundingMask;
    private float maxHitDistance = 7f;
    private float addHeight = 5f;
    private bool[] allGroundSpherecastHits;
    private LayerMask hitMask;
    private Vector3[] allHitNormals;
    private float angleAboutX;
    private float angleAboutZ;
    private float yOffset;
    private float[] allAnimFootWeights;
    private Vector3 averageHitNormal;
    [SerializeField, Range(-0.5f, 2)] private float upperFootLimit = 0.3f;
    [SerializeField, Range(-2, 0.5f)] private float lowerFootLimit = -0.1f;
    private int[] checkLocalTargetY;
    private CapsuleCollider capsuleCollider;


    private void Awake()
    {
        foxAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = foxRB.position;

        allFootTransforms = new Transform[4];
        allFootTransforms[0] = rightFrontFootTransform;
        allFootTransforms[1] = rightHindFootTransform;
        allFootTransforms[2] = leftFrontFootTransform;
        allFootTransforms[3] = leftHindFootTransform;

        allTargetTransforms = new Transform[4];
        allTargetTransforms[0] = rightFrontTargetTransform;
        allTargetTransforms[1] = rightHindTargetTransform;
        allTargetTransforms[2] = leftFrontTargetTransform;
        allTargetTransforms[3] = leftHindTargetTransform;

        allFootIkConstraints = new TwoBoneIKConstraint[4];
        allFootIkConstraints[0] = rFrontFootRig.GetComponent<TwoBoneIKConstraint>();
        allFootIkConstraints[1] = rHindFootRig.GetComponent<TwoBoneIKConstraint>();
        allFootIkConstraints[2] = lFrontFootRig.GetComponent<TwoBoneIKConstraint>();
        allFootIkConstraints[3] = lHindFootRig.GetComponent<TwoBoneIKConstraint>();

        groundingMask = LayerMask.NameToLayer("Ground");

        allGroundSpherecastHits = new bool[5];

        allHitNormals = new Vector3[4];

        allAnimFootWeights = new float[4];

        checkLocalTargetY = new int[4];

        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (animationState)
        {
            case State.Walking:
                //NavMeshWalkAnimation();
                foxAnimator.SetBool("isWalking", true);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Idling:
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", true);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Running:                
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", true);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Looking:
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", true);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Jumping:
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", true);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Attacking:
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", true);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Eating:
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", true);
                foxAnimator.SetBool("isBiting", false);
                break;
            case State.Biting:
                foxAnimator.SetBool("isWalking", false);
                foxAnimator.SetBool("isIdling", false);
                foxAnimator.SetBool("isRunning", false);
                foxAnimator.SetBool("isLooking", false);
                foxAnimator.SetBool("isJumping", false);
                foxAnimator.SetBool("isAttacking", false);
                foxAnimator.SetBool("isEating", false);
                foxAnimator.SetBool("isBiting", true);
                break;
        }
    }

    public void ChangeAnimationState(State newState)
    {
        animationState = newState;
    }
    private void FixedUpdate()
    {
        //RotateFeet();
        //RotateBody();
        //AdjustHeight();
    }

    private void CheckGroundBelow(out Vector3 hitTarget, out bool groundSpherecastHit, out Vector3 hitNormal, out LayerMask hitMask,
        out float currentHitDistance, Transform objectTransform, int checkForLayerMask, float maxHitDistance, float addHeight)
    {
        RaycastHit hit;
        Vector3 startSpherecast = objectTransform.position + new Vector3(0f, addHeight, 0f);

        if (checkForLayerMask == -1)
        {
            Debug.LogError("Layer does not exist");
            groundSpherecastHit = false;
            currentHitDistance = 0f;
            hitMask = LayerMask.NameToLayer("Not hitting anything");
            hitNormal = Vector3.up;
            hitTarget = objectTransform.position;

        }
        else
        {
            int layerMask = (1 << checkForLayerMask);
            if (Physics.SphereCast(startSpherecast, .2f, Vector3.down, out hit, maxHitDistance, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                hitMask = hit.transform.gameObject.layer;
                currentHitDistance = hit.distance - addHeight;
                hitNormal = hit.normal;
                groundSpherecastHit = true;
                hitTarget = hit.point;
            }
            else
            {
                groundSpherecastHit = false;
                currentHitDistance = 0f;
                hitMask = LayerMask.NameToLayer("Not hitting anything");
                hitNormal = Vector3.up;
                hitTarget = objectTransform.position;
            }
        }
    }

    Vector3 ProjectOnContactPlane(Vector3 vector, Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal);
    }

    private void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform footTargetTransform, Vector3 hitNormal)
    {
        Vector3 xAxisProjected = ProjectOnContactPlane(footTargetTransform.forward, hitNormal).normalized;
        Vector3 zAxisProjected = ProjectOnContactPlane(footTargetTransform.right, hitNormal).normalized;

        angleAboutX = Vector3.SignedAngle(footTargetTransform.forward, xAxisProjected, footTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(footTargetTransform.right, zAxisProjected, footTargetTransform.forward);
    }

    private void RotateFeet()
    {
        allAnimFootWeights[0] = foxAnimator.GetFloat("RightFrontLeg"); 
        allAnimFootWeights[1] = foxAnimator.GetFloat("LeftFrontLeg");
        allAnimFootWeights[2] = foxAnimator.GetFloat("RightHindLeg");
        allAnimFootWeights[3] = foxAnimator.GetFloat("LeftHindLeg");

        allAnimFootWeights[0] = foxAnimator.GetFloat("RightFrontRun");
        allAnimFootWeights[1] = foxAnimator.GetFloat("LeftFrontRun");
        allAnimFootWeights[2] = foxAnimator.GetFloat("RightHindRun");
        allAnimFootWeights[3] = foxAnimator.GetFloat("LeftHindRun");

        for (int i = 0; i < 4; i++)
        {
            allFootIkConstraints[i].weight = allAnimFootWeights[i];

            CheckGroundBelow(out Vector3 hitTarget, out allGroundSpherecastHits[i], out Vector3 hitNormal, out hitMask, out _, 
                allFootTransforms[i], groundingMask, maxHitDistance, addHeight);
            allHitNormals[i] = hitNormal;

            if (allGroundSpherecastHits[i] == true)
            {
                yOffset = 0.08f;

                if(allFootTransforms[i].position.y < allTargetTransforms[i].position.y - 0.1f)
                {
                    yOffset += allTargetTransforms[i].position.y - allFootTransforms[i].position.y;
                }

                ProjectedAxisAngles(out angleAboutX, out angleAboutZ, allFootTransforms[i], allHitNormals[i]);
                allTargetTransforms[i].position = new Vector3(allFootTransforms[i].position.x, hitTarget.y + yOffset, allFootTransforms[i].position.z);
                allTargetTransforms[i].rotation = allFootTransforms[i].rotation;
                allTargetTransforms[i].localEulerAngles = new Vector3(allTargetTransforms[i].localEulerAngles.x + angleAboutX,
                    allTargetTransforms[i].localEulerAngles.y, allTargetTransforms[i].localEulerAngles.z + angleAboutZ);
            }
            else
            {
                allTargetTransforms[i].position = allFootTransforms[i].position;
                allTargetTransforms[i].rotation = allFootTransforms[i].rotation;
            }
        }
    }

    private void RotateBody()
    {
        float maxRotationStep = 1f;
        float averageHitNormalX = 0f;
        float averageHitNormalZ = 0f;
        float averageHitNormalY = 0f;

        for ( int i = 0; i < 4; i++)
        {
            averageHitNormalX += allHitNormals[i].x;
            averageHitNormalZ += allHitNormals[i].z;
            averageHitNormalY += allHitNormals[i].y;
        }
        averageHitNormal = new Vector3(averageHitNormalX / 4, averageHitNormalY / 4, averageHitNormalZ / 4).normalized;

        ProjectedAxisAngles(out angleAboutX, out angleAboutZ, transform, averageHitNormal);


        float maxRotationX = 50;
        float maxRotationZ = 20;

        float foxRotationX = transform.eulerAngles.x;
        float foxRotationZ = transform.eulerAngles.z;

        if(foxRotationX > 180)
        {
            foxRotationX -= 360;
        }
        if(foxRotationZ > 180)
        {
            foxRotationZ -= 360;
        }

        if(foxRotationX + angleAboutX < -maxRotationX)
        {
            angleAboutX = maxRotationX + foxRotationX;
        }
        else if(foxRotationX + angleAboutX > maxRotationX)
        {
            angleAboutX = maxRotationX - foxRotationX;
        }

        if (foxRotationZ + angleAboutZ < -maxRotationZ)
        {
            angleAboutZ = maxRotationZ + foxRotationZ;
        }
        else if(foxRotationZ + angleAboutZ > maxRotationZ)
        {
            angleAboutZ = maxRotationZ - foxRotationZ;
        }

        float bodyEulerX = Mathf.MoveTowardsAngle(0, angleAboutX, maxRotationStep);
        float bodyEulerZ = Mathf.MoveTowardsAngle(0, angleAboutZ, maxRotationStep);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x + bodyEulerX, transform.eulerAngles.y, transform.eulerAngles.z + angleAboutZ);

    }

    private void AdjustHeight()
    {
        for(int i = 0; i < 4; i++)
        {
            if(allTargetTransforms[i].localPosition.y < upperFootLimit && allTargetTransforms[i].localPosition.y > lowerFootLimit)
            {
                checkLocalTargetY[i] = 0;
            }
            else if(allTargetTransforms[i].localPosition.y > upperFootLimit)
            {
                checkLocalTargetY[i] = 1;
            }
            else
            {
                checkLocalTargetY[i] = -1;
            }
        }

        if(checkLocalTargetY[0] == 1 && checkLocalTargetY[2] == 1 || checkLocalTargetY[1] == 1 && checkLocalTargetY[3] == 1)
        {
            if(capsuleCollider.center.y > -1.4)
            {
                capsuleCollider.center -= new Vector3(0f, 0.05f, 0f);
            }
            else
            {
                capsuleCollider.center = new Vector3(0f, 3.4f, 0f);
            }
            
            
        }
        else if(checkLocalTargetY[0] == -1 && checkLocalTargetY[2] == -1 || checkLocalTargetY[1] == -1 && checkLocalTargetY[3] == -1)
        {
            if(capsuleCollider.center.y < 1.5)
            {
                capsuleCollider.center += new Vector3(0f, 0.05f, 0f);
            }

            
        }

    }



    void NavMeshWalkAnimation()
    {
        foxAgent.destination = wayPoints.position;
    }


    

}
