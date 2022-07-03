using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingFinisherGuide : MonoBehaviour
{
    [SerializeField] private bool moveUp = true;

    [SerializeField] private Transform finalDesiredPosition;
    [SerializeField] private Transform stairDesiredPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && moveUp == true)
        {
            print("Player has been detected!");

            moveUp = !moveUp;
            other.transform.position = finalDesiredPosition.position;
        }
        else if(other.CompareTag("Player") && moveUp == false)
        {
            moveUp = !moveUp;
            other.transform.position = stairDesiredPosition.position;
        }
    }
}
