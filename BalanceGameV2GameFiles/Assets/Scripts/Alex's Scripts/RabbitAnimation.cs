using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class RabbitAnimation : MonoBehaviour
{
    [Header("Pathfinding Input")]
    [SerializeField] private LayerMask _checkGround;
    //UnityEngine.AI.NavMeshAgent rAgent;


    [Header("Animation Input")]
    [SerializeField] Animator rabbitAnimator;

    public bool isIdling, isLooking, isHopping, isEating, isEscaping;

    public State _animationState;
    public enum State
    {
        Idling, Looking, Hopping, Escaping, Eating, Hiding
    }


    [Header("Movement Input")]
    public float movementSpeed = 20f;
    public float runSpeed = 40f;
    [SerializeField] Rigidbody rabbitRB;


    [Header("Grounding Input")]
    [SerializeField] private Transform _rightFrontFootTransform;
    [SerializeField] private Transform _rightHindFootTransform;
    [SerializeField] private Transform _leftFrontFootTransform;
    [SerializeField] private Transform _leftHindFootTransform;
    private Transform[] _allFootTransforms;
    [SerializeField] private Transform _rightFrontTargetTransform;
    [SerializeField] private Transform _rightHindTargetTransform;
    [SerializeField] private Transform _leftFrontTargetTransform;
    [SerializeField] private Transform _leftHindTargetTransform;
    private Transform[] _allTargetTransforms;
    [SerializeField] private GameObject _rFrontFootRig;
    [SerializeField] private GameObject _rHindFootRig;
    [SerializeField] private GameObject _lFrontFootRig;
    [SerializeField] private GameObject _lHindFootRig;
    private TwoBoneIKConstraint[] _allFootIkConstraints;
    private float _maxHitDistance = 7f;
    private float _addHeight = 5f;
    private bool[] _allGroundSpherecastHits;
    private LayerMask _hitMask;
    private Vector3[] _allHitNormals;
    private float _angleAboutX;
    private float _angleAboutZ;
    private float _yOffset;
    private float[] _allAnimFootWeights;
    private Vector3 _averageHitNormal;
    [SerializeField] private LayerMask _groundingMask;

    [SerializeField, Range(-0.5f, 2)] private float _upperFootLimit = 0.3f;
    [SerializeField, Range(-2, 0.5f)] private float _lowerFootLimit = -0.1f;
    private int[] _checkLocalTargetY;
    private CapsuleCollider _capsuleCollider;


    // Start is called before the first frame update
    void Start()
    {
        _allFootTransforms = new Transform[4];
        _allFootTransforms[0] = _rightFrontFootTransform;
        _allFootTransforms[1] = _rightHindFootTransform;
        _allFootTransforms[2] = _leftFrontFootTransform;
        _allFootTransforms[3] = _leftHindFootTransform;

        _allTargetTransforms = new Transform[4];
        _allTargetTransforms[0] = _rightFrontTargetTransform;
        _allTargetTransforms[1] = _rightHindTargetTransform;
        _allTargetTransforms[2] = _leftFrontTargetTransform;
        _allTargetTransforms[3] = _leftHindTargetTransform;

        _allFootIkConstraints = new TwoBoneIKConstraint[4];
        _allFootIkConstraints[0] = _rFrontFootRig.GetComponent<TwoBoneIKConstraint>();
        _allFootIkConstraints[1] = _rHindFootRig.GetComponent<TwoBoneIKConstraint>();
        _allFootIkConstraints[2] = _lFrontFootRig.GetComponent<TwoBoneIKConstraint>();
        _allFootIkConstraints[3] = _lHindFootRig.GetComponent<TwoBoneIKConstraint>();

        _groundingMask = LayerMask.NameToLayer("Ground");

        _allGroundSpherecastHits = new bool[5];

        _allHitNormals = new Vector3[4];

        _allAnimFootWeights = new float[4];

        _checkLocalTargetY = new int[4];

        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_animationState)
        {
            case State.Idling:
                rabbitAnimator.SetBool("isIdling", true);
                rabbitAnimator.SetBool("isLooking", false);
                rabbitAnimator.SetBool("isHopping", false);
                rabbitAnimator.SetBool("isEscaping", false);
                rabbitAnimator.SetBool("isEating", false);
                break;
            case State.Looking:
                rabbitAnimator.SetBool("isIdling", false);
                rabbitAnimator.SetBool("isLooking", true);
                rabbitAnimator.SetBool("isHopping", false);
                rabbitAnimator.SetBool("isEscaping", false);
                rabbitAnimator.SetBool("isEating", false);
                break;
            case State.Hopping:
                rabbitAnimator.SetBool("isIdling", false);
                rabbitAnimator.SetBool("isLooking", false);
                rabbitAnimator.SetBool("isHopping", true);
                rabbitAnimator.SetBool("isEscaping", false);
                rabbitAnimator.SetBool("isEating", false);
                break;
            case State.Escaping:
                rabbitAnimator.SetBool("isIdling", false);
                rabbitAnimator.SetBool("isLooking", false);
                rabbitAnimator.SetBool("isHopping", false);
                rabbitAnimator.SetBool("isEscaping", true);
                rabbitAnimator.SetBool("isEating", false);
                break;
            case State.Eating:
                rabbitAnimator.SetBool("isIdling", false);
                rabbitAnimator.SetBool("isLooking", false);
                rabbitAnimator.SetBool("isHopping", false);
                rabbitAnimator.SetBool("isEscaping", false);
                rabbitAnimator.SetBool("isEating", true);
                break;
            case State.Hiding:
                rabbitAnimator.SetBool("isIdling", false);
                rabbitAnimator.SetBool("isLooking", false);
                rabbitAnimator.SetBool("isHopping", false);
                rabbitAnimator.SetBool("isEscaping", false);
                rabbitAnimator.SetBool("isEating", false);
                break;
        }
    }

    private void FixedUpdate()
    {
        //RotFeet();
        //RotateBody();
        //AdjustHeight();
    }

    private void CheckGBelow(out Vector3 _hitTarget, out bool _groundSpherecastHit, out Vector3 _hitNormal, out LayerMask _hitMask,
        out float _currentHitDistance, Transform _objectTransform, int _checkForLayerMask, float _maxHitDistance, float _addHeight)
    {
        RaycastHit hit;
        Vector3 _startSpherecast = _objectTransform.position + new Vector3(0f, _addHeight, 0f);

        if (_checkForLayerMask == -1)
        {
            Debug.LogError("Layer does not exist");
            _groundSpherecastHit = false;
            _currentHitDistance = 0f;
            _hitMask = LayerMask.NameToLayer("Not hitting anything");
            _hitNormal = Vector3.up;
            _hitTarget = _objectTransform.position;

        }
        else
        {
            int _layerMask = (1 << _checkForLayerMask);
            if (Physics.SphereCast(_startSpherecast, .2f, Vector3.down, out hit, _maxHitDistance, _layerMask, QueryTriggerInteraction.UseGlobal))
            {
                _hitMask = hit.transform.gameObject.layer;
                _currentHitDistance = hit.distance - _addHeight;
                _hitNormal = hit.normal;
                _groundSpherecastHit = true;
                _hitTarget = hit.point;
            }
            else
            {
                _groundSpherecastHit = false;
                _currentHitDistance = 0f;
                _hitMask = LayerMask.NameToLayer("Not hitting anything");
                _hitNormal = Vector3.up;
                _hitTarget = _objectTransform.position;
            }
        }
    }


    Vector3 ProjectOnContactPlane(Vector3 _vector, Vector3 _hitNormal)
    {
        return _vector - _hitNormal * Vector3.Dot(_vector, _hitNormal);
    }

    private void ProjectedAxisAngles(out float _angleAboutX, out float _angleAboutZ, Transform _footTargetTransform, Vector3 _hitNormal)
    {
        Vector3 _xAxisProjected = ProjectOnContactPlane(_footTargetTransform.forward, _hitNormal).normalized;
        Vector3 _zAxisProjected = ProjectOnContactPlane(_footTargetTransform.right, _hitNormal).normalized;

        _angleAboutX = Vector3.SignedAngle(_footTargetTransform.forward, _xAxisProjected, _footTargetTransform.right);
        _angleAboutZ = Vector3.SignedAngle(_footTargetTransform.right, _zAxisProjected, _footTargetTransform.forward);
    }

    private void RotFeet()
    {
        _allAnimFootWeights[0] = rabbitAnimator.GetFloat("RFLeg");
        _allAnimFootWeights[1] = rabbitAnimator.GetFloat("RHLeg");
        _allAnimFootWeights[2] = rabbitAnimator.GetFloat("LFLeg");
        _allAnimFootWeights[3] = rabbitAnimator.GetFloat("LHLeg");

        _allAnimFootWeights[0] = rabbitAnimator.GetFloat("RFLeg");
        _allAnimFootWeights[1] = rabbitAnimator.GetFloat("RHLeg");
        _allAnimFootWeights[2] = rabbitAnimator.GetFloat("LFLeg");
        _allAnimFootWeights[3] = rabbitAnimator.GetFloat("LHLeg");

        for (int i = 0; i < 4; i++)
        {
            _allFootIkConstraints[i].weight = _allAnimFootWeights[i];

            CheckGBelow(out Vector3 _hitTarget, out _allGroundSpherecastHits[i], out Vector3 _hitNormal, out _hitMask, out _,
                _allFootTransforms[i], _groundingMask, _maxHitDistance, _addHeight);
            _allHitNormals[i] = _hitNormal;

            if (_allGroundSpherecastHits[i] == true)
            {
                _yOffset = 0.01f;

                if (_allFootTransforms[i].position.y < _allTargetTransforms[i].position.y - 0.1f)
                {
                    _yOffset += _allTargetTransforms[i].position.y - _allFootTransforms[i].position.y;
                }

                ProjectedAxisAngles(out _angleAboutX, out _angleAboutZ, _allFootTransforms[i], _allHitNormals[i]);
                _allTargetTransforms[i].position = new Vector3(_allFootTransforms[i].position.x, _hitTarget.y + _yOffset, _allFootTransforms[i].position.z);
                _allTargetTransforms[i].rotation = _allFootTransforms[i].rotation;
                _allTargetTransforms[i].localEulerAngles = new Vector3(_allTargetTransforms[i].localEulerAngles.x + _angleAboutX,
                    _allTargetTransforms[i].localEulerAngles.y, _allTargetTransforms[i].localEulerAngles.z + _angleAboutZ);
            }
            else
            {
                _allTargetTransforms[i].position = _allFootTransforms[i].position;
                _allTargetTransforms[i].rotation = _allFootTransforms[i].rotation;
            }
        }
    }

    private void RotateBody()
    {
        float _maxRotationStep = 1f;
        float _averageHitNormalX = 0f;
        float _averageHitNormalZ = 0f;
        float _averageHitNormalY = 0f;

        for (int i = 0; i < 4; i++)
        {
            _averageHitNormalX += _allHitNormals[i].x;
            _averageHitNormalZ += _allHitNormals[i].z;
            _averageHitNormalY += _allHitNormals[i].y;
        }
        _averageHitNormal = new Vector3(_averageHitNormalX / 4, _averageHitNormalY / 4, _averageHitNormalZ / 4).normalized;

        ProjectedAxisAngles(out _angleAboutX, out _angleAboutZ, transform, _averageHitNormal);


        float _maxRotationX = 50;
        float _maxRotationZ = 20;

        float rabbitRotationX = transform.eulerAngles.x;
        float rabbitRotationZ = transform.eulerAngles.z;

        if (rabbitRotationX > 180)
        {
            rabbitRotationX -= 360;
        }
        if (rabbitRotationZ > 180)
        {
            rabbitRotationZ -= 360;
        }

        if (rabbitRotationX + _angleAboutX < -_maxRotationX)
        {
            _angleAboutX = _maxRotationX + rabbitRotationX;
        }
        else if (rabbitRotationX + _angleAboutX > _maxRotationX)
        {
            _angleAboutX = _maxRotationX - rabbitRotationX;
        }

        if (rabbitRotationZ + _angleAboutZ < -_maxRotationZ)
        {
            _angleAboutZ = _maxRotationZ + rabbitRotationZ;
        }
        else if (rabbitRotationZ + _angleAboutZ > _maxRotationZ)
        {
            _angleAboutZ = _maxRotationZ - rabbitRotationZ;
        }

        float _bodyEulerX = Mathf.MoveTowardsAngle(0, _angleAboutX, _maxRotationStep);
        float _bodyEulerZ = Mathf.MoveTowardsAngle(0, _angleAboutZ, _maxRotationStep);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x + _bodyEulerX, transform.eulerAngles.y, transform.eulerAngles.z + _angleAboutZ);

    }

    private void AdjustHeight()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_allTargetTransforms[i].localPosition.y < _upperFootLimit && _allTargetTransforms[i].localPosition.y > _lowerFootLimit)
            {
                _checkLocalTargetY[i] = 0;
            }
            else if (_allTargetTransforms[i].localPosition.y > _upperFootLimit)
            {
                _checkLocalTargetY[i] = 1;
            }
            else
            {
                _checkLocalTargetY[i] = -1;
            }
        }

        if (_checkLocalTargetY[0] == 1 && _checkLocalTargetY[2] == 1 || _checkLocalTargetY[1] == 1 && _checkLocalTargetY[3] == 1)
        {
            if (_capsuleCollider.center.y > -1.4)
            {
                _capsuleCollider.center -= new Vector3(0f, 0.05f, 0f);
            }
            else
            {
                _capsuleCollider.center = new Vector3(0f, 3.4f, 0f);
            }


        }
        else if (_checkLocalTargetY[0] == -1 && _checkLocalTargetY[2] == -1 || _checkLocalTargetY[1] == -1 && _checkLocalTargetY[3] == -1)
        {
            if (_capsuleCollider.center.y < 1.5)
            {
                _capsuleCollider.center += new Vector3(0f, 0.05f, 0f);
            }


        }

    }




}
