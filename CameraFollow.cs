using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 camOffset;
    [SerializeField] private float smoothedSpeedConstant = 0.125f;

    //void Start()
    //{
    //    camOffset = target.transform.position - transform.position;
    //    print("Offset: " + camOffset);
    //}

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.transform.position + camOffset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothedSpeedConstant);

        transform.position = new Vector3(transform.position.x, smoothedPosition.y, smoothedPosition.z);

       // transform.LookAt(target);

    }

}
