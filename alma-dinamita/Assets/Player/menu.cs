using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public bool isActive = false;
    private bool quitConfirmation = false;
    [SerializeField] public bool isOpen = false;
    [SerializeField] GameObject menuGO;
    private bool classCalledByButton = false;
    movement m;
    mouseLook ml;
    void Start()
    {
        if (menuGO == null)
        {
            classCalledByButton = true;
            return;
        }
        menuGO.SetActive(false);
        m = GetComponent<movement>();
        ml = GetComponent<mouseLook>();
    }

    void Update()
    {
        if (classCalledByButton) return;
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
        m.escapeMenuIsOpen = false;
        ml.enabled = true;
    }

    void OpenMenu()
    {
        menuGO.SetActive(true);
        m.escapeMenuIsOpen = true;
        ml.enabled = false;
    }

    public void QuitGame()
    {
        if (quitConfirmation)
        {
            Application.Quit();
        }
        quitConfirmation = !quitConfirmation;
        GameObject.Find("QuitGame").GetComponentInChildren<Text>().text = "Quit game, confirm!";
    }

    public void ResumeGame()
    {
        var gameMenu = GameObject.Find("Player").GetComponent<menu>();
        gameMenu.CloseMenu();
        gameMenu.isOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameMenu.isActive = !gameMenu.isActive;
    }

}
