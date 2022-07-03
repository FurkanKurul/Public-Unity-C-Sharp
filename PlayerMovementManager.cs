using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager Instance { get; private set; }

    public bool CanWalkPositiveZ
    {
        get 
        {
            return canWalkPositiveZ;
        }

        set
        {
            canWalkPositiveZ = value;
        }
    }

    public bool CanWalkNegativeZ
    {
        get
        {
            return canWalkNegativeZ;
        }

        set
        {
            canWalkNegativeZ = value;
        }
    }

    public bool CanWalkPositiveX
    {
        get
        {
            return canWalkPositiveX;
        }

        set
        {
            canWalkPositiveX = value;
        }
    }

    public bool CanWalkNegativeX
    {
        get
        {
            return canWalkNegativeX;
        }

        set
        {
            canWalkNegativeX = value;
        }
    }

    public bool CanWalkPositiveY
    {
        get
        {
            return canWalkPositiveY;
        }

        set
        {
            canWalkPositiveY = value;
        }
    }

    public bool CanWalkNegativeY
    {
        get
        {
            return canWalkNegativeY;
        }

        set
        {
            canWalkNegativeY = value;
        }
    }

    public bool Grounding
    {
        get
        {
            return grounding;
        }

        set
        {
            grounding = value;
        }
    }

    public bool IsClimbingZoneReached
    {
        get
        {
            return isClimbingZoneReached;
        }

        set
        {
            isClimbingZoneReached = value;
        }
    }

    public bool IsClimbing
    {
        get
        {
            return isClimbing;
        }

        set
        {
            isClimbing = value;
        }
    }

    [SerializeField] private bool grounding = true;               // when player is moving
    [SerializeField] private bool isClimbingZoneReached = false;  // when player reached a climbing zone
    [SerializeField] private bool isClimbing = false;             // when player is climbing by a stair

    [SerializeField] private bool canWalkPositiveZ = true;        // moving along +Z-axis direction
    [SerializeField] private bool canWalkNegativeZ = true;        // moving along -Z-axis direction
    [SerializeField] private bool canWalkPositiveX = true;        // moving along +X-axis direction
    [SerializeField] private bool canWalkNegativeX = true;        // moving along -X-axis direction

    [SerializeField] private bool canWalkPositiveY = false;        // moving along +Y-axis direction
    [SerializeField] private bool canWalkNegativeY = false;        // moving along -Y-axis direction

    [SerializeField] private float horizontalSpeed = 2.2f;
    [SerializeField] private float upDownSpeed = 2.2f;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponentInChildren<Rigidbody>();

        anim.SetBool("walk", false);
        spriteRenderer.flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        // getting horizontal input from player
        float horizontalMovement = Input.GetAxis("Horizontal");

        // getting horizontal input from player
        float VerticalMovement = Input.GetAxis("Vertical");
        
        if (grounding == true)
        {
            GroundMovement(horizontalMovement);
            // Delete line below "CheckPlayerWalkingAnimation"
            // CheckPlayerWalkingAnimation(horizontalMovement);
        }
        else
        {
            CheckPlayerClimbingAnimation(VerticalMovement);
        }
        
        // Checking pre-climbing behaivor
        if(VerticalMovement != 0 && isClimbingZoneReached == true)
        {
            ClimbingMovement(VerticalMovement);
        }

        // when player is NOT climbing
        if (isClimbing == false)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            grounding = true;
        }
        // when player is climbing
        else
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            grounding = false;
        }

        // Checking if player jumps from a height towards to stair
        if (VerticalMovement == 0 && isClimbing == true && isClimbingZoneReached == true)
        {
            anim.SetBool("walk", false);
            anim.SetBool("climbUp", true);
            anim.SetBool("climbDown", false);
            anim.speed = 0.0f;
        }
    }

    public void GroundMovement(float horizontalMovement)
    {
        // checking if player can move in the positive z direction
        if (canWalkPositiveZ == true && horizontalMovement > 0)
        {
            WalkInPositiveZDirection(horizontalMovement);
            CheckPlayerWalkingAnimation(horizontalMovement);
        }
        // checking if player can move in the negative z direction
        else if (canWalkNegativeZ == true && horizontalMovement < 0)
        {
            WalkInNegativeZDirection(horizontalMovement);
            CheckPlayerWalkingAnimation(horizontalMovement);
        }
        else
        {
            CheckPlayerWalkingAnimation(0.0f);
        }

        // checking if player can move in the positive x direction
        if (canWalkPositiveX == true && Input.GetKeyDown(KeyCode.S))
        {
            WalkInPositiveXDirection();
        }
        // checking if player can move in the negative x direction
        else if (canWalkNegativeX == true && Input.GetKeyDown(KeyCode.W))
        {
            WalkInNegativeXDirection();
        }

    }

    public void ClimbingMovement(float VerticalMovement)
    {
        if (canWalkPositiveY == true && VerticalMovement > 0)
        {
            WalkInPositiveYDirection(VerticalMovement);

            CheckPlayerClimbingAnimation(VerticalMovement);
        }
        else if (canWalkNegativeY == true && VerticalMovement < 0)
        {
            WalkInNegativeYDirection(VerticalMovement);

            CheckPlayerClimbingAnimation(VerticalMovement);
        }
        else
        {
            CheckPlayerClimbingAnimation(0.0f);
        }
    }

    public void WalkInPositiveZDirection(float zDirectionInput)
    {
        transform.Translate(horizontalSpeed * Time.deltaTime * zDirectionInput * Vector3.forward);
    }

    public void WalkInNegativeZDirection(float zDirectionInput)
    {
        transform.Translate(horizontalSpeed * Time.deltaTime * zDirectionInput * Vector3.forward);
    }

    public void WalkInPositiveXDirection()
    {
        transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
    }

    public void WalkInNegativeXDirection()
    {
        transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
    }

    public void WalkInPositiveYDirection(float yDirectionInput)
    {
        transform.Translate(Time.deltaTime * upDownSpeed * yDirectionInput * yDirectionInput * Vector3.up);
    }

    public void WalkInNegativeYDirection(float yDirectionInput)
    {
        transform.Translate(Time.deltaTime * upDownSpeed * yDirectionInput * yDirectionInput * Vector3.down);
    }

    public void CheckPlayerWalkingAnimation(float horizontalMovement)
    {
        anim.speed = 1.0f;

        if (horizontalMovement > 0)
        {
            anim.SetBool("walk", true);
            spriteRenderer.flipX = true;

            anim.SetBool("climbUp", false);
            anim.SetBool("climbDown", false);
        }
        else if (horizontalMovement < 0)
        {
            anim.SetBool("walk", true);
            spriteRenderer.flipX = false;

            anim.SetBool("climbUp", false);
            anim.SetBool("climbDown", false);
        }
        else
        {
            anim.SetBool("walk", false);
            spriteRenderer.flipX = false;

            anim.SetBool("climbUp", false);
            anim.SetBool("climbDown", false);
        }
    }

    public void CheckPlayerClimbingAnimation(float VerticalMovement)
    {
        if (VerticalMovement > 0)
        {
            // climbUp animation is playings
            anim.SetBool("walk", false);
            anim.SetBool("climbUp", true);
            anim.SetBool("climbDown", false);
            anim.speed = Mathf.Abs(VerticalMovement);

        }
        else if (VerticalMovement < 0)
        {
            // climbDown animation is playings
            anim.SetBool("walk", false);
            anim.SetBool("climbUp", false);
            anim.SetBool("climbDown", true);
            anim.speed = Mathf.Abs(VerticalMovement);
        }
        else
        {
            // IDLE animation is playings
            anim.SetBool("walk", false);
            anim.SetBool("climbUp", false);
            anim.SetBool("climbDown", false);
            anim.speed = 1.0f;
        }
    }
}
