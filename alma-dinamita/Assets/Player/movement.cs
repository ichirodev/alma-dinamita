using UnityEngine;
using System;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    // UI Metrics
    public Text movementSpeedMultiplierUIText;
    public Text movementSpeedUnitsUIText;
    public Text inHandWeaponWeightUIText;
    // The speed at which we can move, in units per second.
    [SerializeField] float moveSpeed = 5;
    [Range(-2, 0), SerializeField] float inHandWeaponWeight = 0;
    // The height of a jump, in units.
    [SerializeField] float jumpHeight = 2;
    // The rate at which our vertical speed will be reduced, in units
    // per second.
    [SerializeField] float gravity = 20;
    // The degree to which we can control our movement while in midair.
    [Range(0, 10), SerializeField] float airControl = 5;
    // Our current movement direction. If we're on the ground, we have
    // direct control over it, but if we're in the air, we only have
    // partial control over it.
    Vector3 moveDirection = Vector3.zero;
    // A cached reference to the character controller, which we'll be
    // using often.

    struct movementMultiplier
    {
        public const float crouch = 0.4f;
        public const float slowWalk = 0.6f;
        public const float walk = 1.0000f;
        public const float sprint = 1.21f;
        public const float slide = 1.3f;
    }

    [SerializeField] float moveSpeedMultiplier = movementMultiplier.walk;
    public float sprintStartTime = 0.0f;
    public float sprintEndTime = 0.0f;
    public float sprintTimeLimit = 6f;
    public bool sprinting;
    public bool sprintFirstFrame;
    public bool canSprint;
    public float endsFreezeTimeSprint;
    void UpdateMovementSpeedUnitsText()
    {
        String s = "MSU: " + (moveSpeed * moveSpeedMultiplier + inHandWeaponWeight).ToString();
        movementSpeedUnitsUIText.text = s;
    }
    void UpdateMovementSpeedMultiplierText()
    {
        String s = "MSM: " + moveSpeedMultiplier.ToString();
        movementSpeedMultiplierUIText.text = s;
    }

    void UpdateInHandWeaponWeightText()
    {
        String s = "IHWW: " + inHandWeaponWeight.ToString() + " units";
        inHandWeaponWeightUIText.text = s;
    }

    CharacterController controller;

    void Start()
    {
        sprinting = false;
        sprintFirstFrame = true;
        canSprint = true;
        endsFreezeTimeSprint = 0.00000000001f;
        controller = GetComponent<CharacterController>();
        movementSpeedMultiplierUIText.text = "MSM: 0.0 Started but not updated";
        movementSpeedUnitsUIText.text = "MSU: 0.0 Started but not updated";
        inHandWeaponWeightUIText.text = "IHWW: Hope is not an anvil...";
    }

    // We do our movement logic in FixedUpdate so that our movement
    // can happen at the same pace as physics updates. If it didn't,
    // we'd see jitter when we interact with physics objects that can
    // move around.
    void FixedUpdate()
    {
        // The input vector describes the user's desired local-space
        // movement; if we're on the ground, this will immediately
        // become our movement, but if we're in the air, we'll
        // interpolate between our current movement and this vector, to
        // simulate momentum.
        var input = new Vector3(
        Input.GetAxis("Horizontal"),
        0,
        Input.GetAxis("Vertical")
        );
        // Multiply this movement by our desired movement speed
        input *= (moveSpeed * moveSpeedMultiplier + inHandWeaponWeight);
        // The controller's Move method uses world-space directions, so
        // we need to convert this direction to world space
        input = transform.TransformDirection(input);
        // Is the controller's bottommost point touching the ground?
        if (controller.isGrounded)
        {
            // Figure out how much movement we want to apply in local
            // space.
            moveDirection = input;
            // Is the user pressing the jump button right now?
            if (Input.GetButton("Jump"))
            {
                // Calculate the amount of upward speed we need,
                // considering that we add moveDirection.y to our height
                // every frame, and we reduce moveDirection.y by gravity
                // every frame.
                moveDirection.y = Mathf.Sqrt(2 * gravity * jumpHeight);
            }
            else
            {
                // We're on the ground, but not jumping. Set our
                // downward movement to 0 (otherwise, because we're
                // continuously reducing our y-movement, if we walk off
                // a ledge, we'd suddenly have a huge amount of
                // downwards momentum).
                moveDirection.y = 0;
            }
        }
        else
        {
            // Slowly bring our movement towards the user's desired
            // input, but preserve our current y-direction (so that the
            // arc of the jump is preserved)
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input,
            airControl * Time.deltaTime);
        }
        // Bring our movement down by applying gravity over time
        moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller. The controller will refuse to move into
        // other colliders, which means that we won't clip through the
        // ground or other colliders. (However, this doesn't stop other
        // colliders from moving into us. For that, we'd need to detect
        // when we're overlapping another collider, and move away from
        // them. We'll cover this in another recipe!)
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Update()
    {
        if (endsFreezeTimeSprint < Time.time)
        {
            if (Input.GetAxis("Vertical") < 0.01f)
            {
                canSprint = false;
            } else
            {
                canSprint = true;
            }
        } else
        {
            canSprint = false;
        }
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            if (sprintFirstFrame)
            {
                sprintFirstFrame = false;
                sprintStartTime = Time.time;
                sprintEndTime = Time.time + sprintTimeLimit;
                sprinting = true;
            } else
            {
                if (Time.time < sprintEndTime)
                {
                    moveSpeedMultiplier = movementMultiplier.sprint;
                } else
                {
                    sprintFirstFrame = true;
                    sprinting = false;
                    sprintStartTime = 0.0f;
                    sprintEndTime = 0.0f;
                    moveSpeedMultiplier = movementMultiplier.walk;
                    endsFreezeTimeSprint = Time.time + 8;
                    canSprint = false;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && canSprint)
        {
            sprintFirstFrame = true;
            sprintStartTime = 0.0f;
            sprintEndTime = 0.0f;
            sprinting = false;
            moveSpeedMultiplier = movementMultiplier.walk;
        }
        if (!sprinting && Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeedMultiplier = movementMultiplier.crouch;
        } else if (!sprinting && Input.GetKey(KeyCode.LeftAlt))
        {
            moveSpeedMultiplier = movementMultiplier.slowWalk;
        }
        if (!sprinting && Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveSpeedMultiplier = movementMultiplier.walk;
        }
        else if (!sprinting && Input.GetKeyUp(KeyCode.LeftAlt))
        {
            moveSpeedMultiplier = movementMultiplier.walk;
        }
        UpdateMovementSpeedMultiplierText();
        UpdateMovementSpeedUnitsText();
        UpdateInHandWeaponWeightText();
    }
}