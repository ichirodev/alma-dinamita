using UnityEngine;

public class slide : MonoBehaviour
{
    [SerializeField] CharacterController CharControllerComponent;
    [SerializeField] movement MovComponent;
    public Transform slideToPosition;
    public Transform slideGroundCheck;
    public Vector3 fixedSlideToPosition;
    public bool isSliding = false;
    float slideSpeed = 7.880f;
    private float maxHeightWhenSliding = 0.1f;
    RaycastHit rayHitOnGround;
    Ray groundCheckRay;

    private void Start()
    {
        CharControllerComponent = GetComponent<CharacterController>();
        MovComponent = GetComponent<movement>();
    }
    private void Update()
    {
        // Don't allow to slide if the player is walking backwards
        if (Input.GetAxis("Vertical") < -0.001000f)
        {
            return;
        }
        if (fixedSlideToPosition == transform.position || EndSlideByPlayerOrWorld())
        {
            CancelSlide();
        }
        if (CharControllerComponent.isGrounded && MovComponent.sprinting && Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            StartSlide();
        }
        if (isSliding)
        {
            // Slide the player
            transform.position = Vector3.Lerp(transform.position, fixedSlideToPosition, slideSpeed * Time.deltaTime);
            // Check if the player still grounded, if not then stop sliding and return the control
            // of the player to the Character Controller
            groundCheckRay = new Ray(slideGroundCheck.transform.position, -Vector3.up);
            if (Physics.Raycast(groundCheckRay, out rayHitOnGround))
            {
                if (rayHitOnGround.collider.tag == "World")
                {
                    if (rayHitOnGround.distance > maxHeightWhenSliding)
                    {
                        CancelSlide();
                        Debug.Log(rayHitOnGround.distance);
                    }
                }
            }
        }
    }

    void CancelSlide()
    {
        isSliding = false;
        CharControllerComponent.enabled = true;
        MovComponent.enabled = true;
        fixedSlideToPosition = new Vector3(0, 0, 0);
    }

    void StartSlide()
    {
        isSliding = true;
        fixedSlideToPosition = slideToPosition.position;
        CharControllerComponent.enabled = false;
        MovComponent.enabled = false;
    }

    // The slide action could end for various reasons
    // 1. Object collition: if the controller collition detects a wall the slide ends
    // 2. Slide cancel: Allow the player to take control over the controller before
    // the slide ends
    private bool EndSlideByPlayerOrWorld()
    {
        float canSlideCancelZPoint = fixedSlideToPosition.z - 0.26f;
        float forceSlideToEndZPoint = fixedSlideToPosition.z - 0.02001f;
        // Slide cancel, the player can end the slide before it gets slow by jumping
        if (transform.position.z >= canSlideCancelZPoint)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
        }
        // Cancel slide automatically, due to the use of Lerp, there is a moment where the player gets
        // horribly slow when sliding so the slide has to finish before that moment
        if (transform.position.z >= forceSlideToEndZPoint)
        {
            return true;
        }

        return false;
    }
}
