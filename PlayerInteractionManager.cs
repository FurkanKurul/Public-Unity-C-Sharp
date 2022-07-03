using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementManager playerMovementManager;

    private void Start()
    {
        if(playerMovementManager == null)
        {
            playerMovementManager = GetComponent<PlayerMovementManager>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != null)
        {
            //print("Name: " + other.name);

            LockPlayerInputAbility(other.gameObject);
        }
        else
        {
            print("ERROR!!!");
        }

        if (other.CompareTag("stair"))
        {
            playerMovementManager.IsClimbing = true;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, other.transform.position, 0.5f);
            transform.position = new Vector3(transform.position.x, transform.position.y, smoothedPosition.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != null)
        {
            UnlockPlayerInputAbility(other.gameObject);
        }
        else
        {
            print("ERROR!!!");
        }

        if (other.CompareTag("stair"))
        {
            playerMovementManager.IsClimbing = false;
        }
    }

    public void LockPlayerInputAbility(GameObject otherGO)
    {
        // NOTE: Let's assume that we are looking game environment from the YZ surface,
        // so directions below are with respect to this surface.

        // Think a cube in the Unity's Coordinate System:

        // This cube has a RIGHT-SIDE. Hence, players cannot move towards LEFT when s/he stays the RIGHT-SIDE.
        if (otherGO.name == "positiveZ")
        {
            playerMovementManager.CanWalkNegativeZ = false;
        }

        // This cube has a LEFT-SIDE. Hence, players cannot move towards RIGHT when s/he stays the LEFT-SIDE.
        if (otherGO.name == "negativeZ")
        {
            playerMovementManager.CanWalkPositiveZ = false;
        }

        // This cube has a FRONT-SIDE. Hence, players cannot move towards BACK when s/he stays the FRONT-SIDE.
        if (otherGO.name == "positiveX")
        {
            playerMovementManager.CanWalkNegativeX = false;
        }

        // This cube has a BACK-SIDE. Hence, players cannot move towards FRONT when s/he stays the BACK-SIDE.
        if (otherGO.name == "negativeX")
        {
            playerMovementManager.CanWalkPositiveX = false;
        }

        if (otherGO.name == "positiveXandUP")
        {
            playerMovementManager.Grounding = true;
            playerMovementManager.IsClimbingZoneReached = true;

            playerMovementManager.CanWalkPositiveY = true;
            playerMovementManager.CanWalkNegativeY = true;
        }
    }

    public void UnlockPlayerInputAbility(GameObject otherGO)
    {
        // NOTE: Let's assume that we are looking game environment from the YZ surface,
        // so directions below are with respect to this surface.

        // Think a cube in the Unity's Coordinate System:

        // This cube has a RIGHT-SIDE. Hence, players cannot move towards LEFT when s/he lefts the RIGHT-SIDE.
        if (otherGO.name == "positiveZ")
        {
            playerMovementManager.CanWalkNegativeZ = true;
        }

        // This cube has a LEFT-SIDE. Hence, players cannot move towards RIGHT when s/he lefts the LEFT-SIDE.
        if (otherGO.name == "negativeZ")
        {
            playerMovementManager.CanWalkPositiveZ = true;
        }

        // This cube has a FRONT-SIDE. Hence, players cannot move towards BACK when s/he lefts the FRONT-SIDE.
        if (otherGO.name == "positiveX")
        {
            playerMovementManager.CanWalkNegativeX = true;
        }

        // This cube has a BACK-SIDE. Hence, players cannot move towards FRONT when s/he lefts the BACK-SIDE.
        if (otherGO.name == "negativeX")
        {
            playerMovementManager.CanWalkPositiveX = true;
        }


        if (otherGO.name == "FinishClimbing")
        {
            playerMovementManager.Grounding = true;
            playerMovementManager.IsClimbingZoneReached = false;
        }

        if (otherGO.name == "positiveXandUP")
        {
            playerMovementManager.Grounding = true;
            playerMovementManager.IsClimbingZoneReached = false;

            playerMovementManager.CanWalkPositiveY = false;
            playerMovementManager.CanWalkNegativeY = false;
        }

    }
}
