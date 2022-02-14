using UnityEngine;

public class menu : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] public bool isOpen = false;
    [SerializeField] GameObject menuGO;
    movement m;
    mouseLook ml;
    void Start()
    {
        menuGO.SetActive(false);
        m = GetComponent<movement>();
        ml = GetComponent<mouseLook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            if (isActive && !isOpen)
            {
                isOpen = true;
                OpenMenu();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (!isActive && isOpen)
            {
                CloseMenu();
                isOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    void CloseMenu()
    {
        menuGO.SetActive(false);
        m.enabled = true;
        ml.enabled = true;
    }

    void OpenMenu()
    {
        menuGO.SetActive(true);
        m.enabled = false;
        ml.enabled = false;
    }

}
