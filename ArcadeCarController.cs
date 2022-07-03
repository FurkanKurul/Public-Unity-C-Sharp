using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarController : MonoBehaviour
{
    private float m_verticalInput, m_horizontalInput;

    private bool isCarGrounded;

    [SerializeField] private GameObject FRW, FLW, RRW, RLW;

    [SerializeField] private Transform FW_Default, FW_Turning_Left, FW_Turning_Right;

    [SerializeField] private Rigidbody sphereRB;

    [SerializeField]
    private float forwardSpeed = 50.0f, backwardSpeed = 25.0f,
    turningSpeed = 150.0f, wheelTurningSpeed = 350.0f, alignToGroundTime, alignToWheelTime;

    [SerializeField] private float modifiedDrag = 0.1f, normalDrag;

    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        sphereRB.transform.parent = null;

        normalDrag = sphereRB.drag;
    }

    void Update()
    {
        // Getting input
        m_verticalInput = Input.GetAxis("Vertical");
        m_horizontalInput = Input.GetAxis("Horizontal");

        // Calculate Turning Rotation
        float newRotation = m_horizontalInput * turningSpeed * Time.deltaTime * m_verticalInput;

        if (isCarGrounded)
        {
            transform.Rotate(0, newRotation, 0, Space.World);
        }

        // Set Cars Position to Our Sphere (Motor)
        transform.position = sphereRB.transform.position;

        // Raycast to the ground and get normal to align car with it.
        isCarGrounded = Physics.Raycast(sphereRB.transform.position, -transform.up, out RaycastHit hit, 1f, groundLayer);

        // Rotate Car to align with ground
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);

        // Calculate Movement Direction
        m_verticalInput *= m_verticalInput > 0 ? forwardSpeed : backwardSpeed;

        // Calculate Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;

        FrontWheelsRotation();

        // HandleRotationOfWheels();
    }

    void FixedUpdate()
    {
        if (isCarGrounded)
        {
            sphereRB.AddForce(transform.forward * m_verticalInput, ForceMode.Acceleration);
        }
        else
        {
            // sphereRB.AddForce(transform.up * -200.0f); --> This can cause a bug
            sphereRB.AddForce(Vector3.up * -200.0f);
        }
    }

    void FrontWheelsRotation()
    {
        if (m_horizontalInput == 0)
        {
            FLW.transform.rotation = Quaternion.Slerp(FLW.transform.rotation, FW_Default.transform.rotation, alignToWheelTime * Time.deltaTime);
            FRW.transform.rotation = Quaternion.Slerp(FRW.transform.rotation, FW_Default.transform.rotation, alignToWheelTime * Time.deltaTime);
        }
        else
        {
            FLW.transform.rotation = m_horizontalInput > 0 ? Quaternion.Slerp(FLW.transform.rotation, FW_Turning_Right.rotation, alignToWheelTime * Time.deltaTime) : Quaternion.Slerp(FLW.transform.rotation, FW_Turning_Left.rotation, alignToWheelTime * Time.deltaTime);
            FRW.transform.rotation = m_horizontalInput > 0 ? Quaternion.Slerp(FRW.transform.rotation, FW_Turning_Right.rotation, alignToWheelTime * Time.deltaTime) : Quaternion.Slerp(FLW.transform.rotation, FW_Turning_Left.rotation, alignToWheelTime * Time.deltaTime);
        }
    }

    //void HandleRotationOfWheels()
    //{
    //    float wheelRotation = wheelTurningSpeed * Time.deltaTime * Input.GetAxis("Vertical");
    //    FLW.transform.Rotate(wheelRotation, 0, 0, Space.Self);
    //    FRW.transform.Rotate(wheelRotation, 0, 0, Space.Self);
    //    RLW.transform.Rotate(wheelRotation, 0, 0, Space.Self);
    //    RRW.transform.Rotate(wheelRotation, 0, 0, Space.Self);
    //}
}
