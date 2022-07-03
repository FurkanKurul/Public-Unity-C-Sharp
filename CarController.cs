using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float m_horizontalInput, m_verticalInput, m_steeringAngle;

    [SerializeField] private float maxSteeringAngle = 30.0f, maxMotorForce = 1200.0f,
    resistance = 3750.0f, breakPower = 250.0f, inertiaBreaker = 500.0f, maxSpeed = 150.0f;

    [SerializeField] private WheelCollider FRWC, FLWC, RRWC, RLWC, dummyRRWC, dummyRLWC;
    [SerializeField] private Transform FRW, FLW, RRW, RLW;

    [SerializeField] private bool frontTorque = false, rearTorque = true, canBoost = true;
    [SerializeField] private bool carIdling = true, forwardMovement = false, backwardMovement = false, breaking = false;
    
    [SerializeField] Rigidbody carRB;

    private void Start()
    {
        carRB = GetComponent<Rigidbody>();

        RRWC.motorTorque = 0.0f;
        RLWC.motorTorque = 0.0f;
    }
    private void Update()
    {
        print("motorTorque: " + RRWC.motorTorque);

        //~~~~~~~~~ GETTING INPUT FROM PLAYER ~~~~~~~~~//
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");


        //~~~~~~~~~ STATES OF CAR ~~~~~~~~~//
        if (m_verticalInput > 0)
        {
            carIdling = false;
            forwardMovement = true;
            backwardMovement = false;

           // print("FORWARD");
        }
        if (m_verticalInput < 0)
        {
            carIdling = false;
            forwardMovement = false;
            backwardMovement = true;

          //  print("BACKWARD");
        }
        if (m_verticalInput == 0)
        {
            carIdling = true;
            forwardMovement = false;
            backwardMovement = false;

          //  print("IDLE");
        }


        //~~~~~~~~~ BREAKING MECHANISM ~~~~~~~~~//

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    breaking = true;
        //}
        //else
        //{
        //    breaking = false;

        //}

        //~~~~~~~~~ BOOSTING MECHANISM ~~~~~~~~~//

        if (carRB.velocity.magnitude > maxSpeed)
        {
            canBoost = false;
        }
        else
        {
            canBoost = true;
        }

        //~~~~~~~~~ STEER ~~~~~~~~~//
        Steer();
    }

    private void FixedUpdate()
    {
        if (forwardMovement == true && breaking == false && backwardMovement == false)
        {
            RearWheelsTorque(1.0f);

            if (canBoost)
            {
                carRB.AddForce(carRB.velocity * inertiaBreaker);
            }
        }

        if (backwardMovement == true && breaking == false && forwardMovement == false)
        {
            RearWheelsTorque(-1.0f);

            if (canBoost)
            {
                carRB.AddForce(carRB.velocity * inertiaBreaker);
            }
        }

        if (carIdling)
        {
            RRWC.motorTorque = 0;
            RLWC.motorTorque = 0;

            if (carRB.velocity.magnitude != 0)
            {
                carRB.AddForce(-carRB.velocity * resistance);
            }

            //if car is idling, its motorTorque should be zero
            Break(500);
        }
        else
        {
            Break(0);
        }


        //  CAR BREAK 
        if (Input.GetKey(KeyCode.Space))
        {
            Break(500);
            RRWC.motorTorque = 0;
            RLWC.motorTorque = 0;
            print("BREAKING:::RRWC.motorTorque: " + RRWC.motorTorque);
        }
        else
        {
            Break(0);
        }

        // ADJUSTING WHEEL POS & ROT
        UpdateWheelPositions();
    }

    private void Steer()
    {
        m_steeringAngle = maxSteeringAngle * m_horizontalInput;

        FRWC.steerAngle = m_steeringAngle;
        FLWC.steerAngle = m_steeringAngle;
    }

    private void RearWheelsTorque(float directionInput)
    {
        RRWC.motorTorque = directionInput * maxMotorForce;
        RLWC.motorTorque = directionInput * maxMotorForce;
    }

    private void Break(float breakPow)
    {
        RRWC.brakeTorque = breakPow;
        RLWC.brakeTorque = breakPow;
    }

    private void UpdateWheelPositions()
    {
        UpdateWheelPosition(FRWC, FRW);
        UpdateWheelPosition(FLWC, FLW);
        UpdateWheelPosition(dummyRRWC, RRW);
        UpdateWheelPosition(dummyRLWC, RLW);
    }

    private void UpdateWheelPosition(WheelCollider _wheelCollider, Transform _transform)
    {
        Vector3 position;
        Quaternion quaternion;

        _wheelCollider.GetWorldPose(out position, out quaternion);

        _transform.position = position;
        _transform.rotation = quaternion;
    }
}
