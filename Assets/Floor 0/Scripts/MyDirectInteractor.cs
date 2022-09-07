using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MyDirectInteractor : MonoBehaviour
{
    [SerializeField] private ActionBasedController controller;
    [SerializeField] private Transform palm;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Rigidbody handBody;
    [SerializeField] private float jointDistance;
    [SerializeField] private float sphereRadius;

    private Transform _grabPoint;

    private Rigidbody _targetBody;

    private FixedJoint fixedJoint1;
    private FixedJoint fixedJoint2;

    private bool _isGrabbing = false;
    private bool _isHolding = false;
    
    private void Start()
    {
        controller.selectAction.action.started += GrabObject;
        controller.selectAction.action.canceled += ReleaseObject;
    }

    private void GrabObject(InputAction.CallbackContext context)
    {
        if (_isGrabbing || _isHolding)
            return;

        Collider[] colliders = Physics.OverlapSphere(palm.position, sphereRadius, layerMask);

        if (colliders.Length < 1)
            return;
        var objectToGrab = colliders[0].gameObject;

        var objectBody = objectToGrab.GetComponent<Rigidbody>();

        if (objectBody != null)
        {
            _targetBody = objectBody;
        }
        else
        {
            objectBody.GetComponentInParent<Rigidbody>();
            if (objectBody != null)
            {
                _targetBody = objectBody;
            }
            else
            {
                return;
            }
        }

        StartCoroutine(Grab(_targetBody, colliders[0]));
    }

    private IEnumerator Grab(Rigidbody target, Collider targetCollider)
    {
        _isGrabbing = true;

        _grabPoint = new GameObject().transform;
        _grabPoint.position = targetCollider.ClosestPoint(palm.position);
        _grabPoint.parent = target.transform;


        target.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        target.interpolation = RigidbodyInterpolation.Interpolate;
        target.velocity = (palm.position - target.position) /Time.deltaTime;

        while (_grabPoint != null && Vector3.Distance(palm.position, _grabPoint.position) >= jointDistance && _isGrabbing)
        { 
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Grabbed");
        handBody.velocity = Vector3.zero;
        handBody.angularVelocity = Vector3.zero;
        target.velocity = Vector3.zero;
        target.angularVelocity = Vector3.zero;

        if(_grabPoint != null)
        {
            fixedJoint1 = _grabPoint.gameObject.AddComponent<FixedJoint>();
            fixedJoint1.connectedBody = handBody;
            fixedJoint1.enableCollision = false;
            fixedJoint1.breakForce = float.PositiveInfinity;
            fixedJoint1.breakTorque = float.PositiveInfinity;
            fixedJoint1.massScale = 1;
            fixedJoint1.connectedMassScale = 1;
            fixedJoint1.enablePreprocessing = false;

            fixedJoint2 = handBody.gameObject.AddComponent<FixedJoint>();
            fixedJoint2.connectedBody = target;
            fixedJoint2.enableCollision = false;
            fixedJoint2.breakForce = float.PositiveInfinity;
            fixedJoint2.breakTorque = float.PositiveInfinity;
            fixedJoint2.massScale = 1;
            fixedJoint2.connectedMassScale = 1;
            fixedJoint2.enablePreprocessing = false;
        }     

        Debug.Log("Joints Placed");

    }

    private void ReleaseObject(InputAction.CallbackContext context)
    {
        if (fixedJoint1 != null)
        {
            Destroy(fixedJoint1);
        }
        if (fixedJoint2 != null)
        {
            Destroy(fixedJoint2);
        }
        if (_grabPoint != null)
        {
            Destroy(_grabPoint.gameObject);
        }

        _isGrabbing = false;
    }
}
