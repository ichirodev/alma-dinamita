using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    [SerializeField] bool invertVerticalLook = true;
    float invertVLook = 1f;
    // The speed at which we turn. In other words, mouse sensitivity.
    [SerializeField] float turnSpeed = 90f;

    // How far up the head can tilt, measured in angles from dead-
    // level. Must be higher than headLowerAngleLimit.
    [SerializeField] float headUpperAngleLimit = 85f;
    // How far down the head can tilt, measured in angles from dead-
    // level. Must be lower than headLowerAngleLimit.
    [SerializeField] float headLowerAngleLimit = -80f;
    // Our current rotation from our start, in degrees
    float yaw = 0f;
    float pitch = 0f;

    // Stores the orientations of the head and body when the game
    // started. We'll derive new orientations by combining these
    // with our yaw and pitch.
    Quaternion bodyStartOrientation;
    Quaternion headStartOrientation;

    // A reference to the head object�the object to rotate up and down.
    // (The body is the current object, so we don't need a variable to
    // store a reference to it.) Not exposed in the interface; instead,
    // we'll figure out what to use by looking for a Camera child object
    // at game start.
    Transform head;

    void Start()
    {
        // Find our head object
        head = GameObject.Find("Camera").GetComponent<Camera>().transform;
        // Cache the orientation of the body and head
        bodyStartOrientation = transform.localRotation;
        headStartOrientation = head.transform.localRotation;
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (invertVerticalLook) invertVLook = -1f;
    }

    void FixedUpdate()
    {
        // Read the current horizontal movement, and scale it based on
        // the amount of time that's elapsed and the movement speed.
        var horizontal = Input.GetAxis("Mouse X")
        * Time.deltaTime * turnSpeed;
        // Same for vertical.
        var vertical = Input.GetAxis("Mouse Y")
        * Time.deltaTime * turnSpeed * invertVLook;
        // Update our yaw and pitch values.
        yaw += horizontal;
        pitch += vertical;
        // Clamp pitch so that we can't look directly down or up.
        pitch =
        Mathf.Clamp(pitch, headLowerAngleLimit, headUpperAngleLimit);
        // Compute a rotation for the body by rotating around the y-axis
        // by the number of yaw degrees, and for the head around the
        // x-axis by the number of pitch degrees.
        var bodyRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(pitch, Vector3.right);
        // Create new rotations for the body and head by combining them
        // with their start rotations.
        transform.localRotation = bodyRotation * bodyStartOrientation;
        head.localRotation = headRotation * headStartOrientation;
    }
}

