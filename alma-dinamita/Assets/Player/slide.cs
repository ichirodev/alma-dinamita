using UnityEngine;

public class slide : MonoBehaviour
{
    [SerializeField] CharacterController CharControllerComponent;
    [SerializeField] movement MovComponent;
    public Transform slideToPosition;
    public Vector3 fixedSlideToPosition;
    public bool isSliding = false;
    float slideSpeed = 7.880f;

    private void Start()
    {
        CharControllerComponent = GetComponent<CharacterController>();
        MovComponent = GetComponent<movement>();
    }
    private void Update()
    {
        if (fixedSlideToPosition == transform.position)
        {
            isSliding = false;
            CharControllerComponent.enabled = true;
            MovComponent.enabled = true;
            fixedSlideToPosition = new Vector3(0, 0, 0);
        }
        if (CharControllerComponent.isGrounded && MovComponent.sprinting && Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            isSliding = true;
            fixedSlideToPosition = slideToPosition.position;
            CharControllerComponent.enabled = false;
            MovComponent.enabled = false;
        }
        if (isSliding)
        {
            transform.position = Vector3.Lerp(transform.position, fixedSlideToPosition, slideSpeed * Time.deltaTime);
        }
    }

    // The slide action could end for various reasons
    // 1. Object collition: if the controller collition detects a wall the slide ends
    // 2. Slide cancel: Allow the player to take control over the controller before
    // the slide ends
    private bool EndSlide()
    {
        float canSlideCancel = fixedSlideToPosition.z - 0.4f;
        // Slide cancel
        if (transform.position.z >= canSlideCancel)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
        }
        return false;
    }
}
