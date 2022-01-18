using UnityEngine;

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
    private int activeWeapon = 0;
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
                SetWeaponActive(false);
            }
            else if (!isActive && isOpen)
            {
                CloseMenu();
                isOpen = false;
                SetWeaponActive(true);
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

    void SetWeaponActive(bool changeActiveTo)
    {
        if (changeActiveTo)
        {
            switch (activeWeapon)
            {
                case 1:
                {
                    primaryWeaponGO.SetActive(true);
                    break;
                }
                case 2:
                {
                    secondaryWeaponGO.SetActive(true);
                    break;
                }
                case 3:
                {
                    specialWeaponGO.SetActive(true);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        else
        {
            if (primaryWeaponGO.activeSelf)
            {
                activeWeapon = 1;
                primaryWeaponGO.SetActive(false);
            }

            if (secondaryWeaponGO.activeSelf)
            {
                activeWeapon = 2;
                secondaryWeaponGO.SetActive(false);
            }

            if (specialWeaponGO.activeSelf)
            {
                activeWeapon = 3;
                specialWeaponGO.SetActive(false);
            }
        }
    }
}
