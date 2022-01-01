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
}
