using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    
    bool isActive = false;
    bool isOpen = false;
    [SerializeField] GameObject menuGO;
    [SerializeField] GameObject graphGO;
    movement m;
    mouseLook ml;
    [SerializeField] GameObject primaryWeaponGO;
    [SerializeField] GameObject secondaryWeaponGO;
    [SerializeField] GameObject specialWeaponGO;

    void Start()
    {
        menuGO.SetActive(false);
        m = GetComponent<movement>();
        ml = GetComponent<mouseLook>();
        primaryWeaponGO = GameObject.Find("primaryWeapon");
        secondaryWeaponGO = GameObject.Find("secondaryWeapon");
        specialWeaponGO = GameObject.Find("specialWeapon");
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
            }
            else if (!isActive && isOpen)
            {
                CloseMenu();
                isOpen = false;
            }
        }

        
    }
    void CloseMenu()
    {
        menuGO.SetActive(false);
        graphGO.SetActive(true);
        m.enabled = true;
        ml.enabled = true;
    }

    void OpenMenu()
    {
        menuGO.SetActive(true);
        graphGO.SetActive(false);
        m.enabled = false;
        ml.enabled = false;
    }
}
