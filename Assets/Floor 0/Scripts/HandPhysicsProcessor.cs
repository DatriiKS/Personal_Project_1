using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysicsProcessor : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Rigidbody m_rigidbody;
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        m_rigidbody.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        if (angleInDegree > 180.0f)
        {
            angleInDegree -= 360.0f;
        }

        var result = (rotationAxis * (angleInDegree * Mathf.Deg2Rad)) / Time.fixedDeltaTime;
        if (result.magnitude > 0.01 || result.magnitude < -0.01)
        {
            m_rigidbody.angularVelocity = result;
        }
    }
}
