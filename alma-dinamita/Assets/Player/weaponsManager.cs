using System.Collections;
using UnityEngine;

public class weaponsManager : MonoBehaviour
{
    public float timeLeftUntilGameObjectIsEnabled = 0.01f;
    [SerializeField] GameObject primaryWeapon;
    [SerializeField] GameObject secondaryWeapon;
    [SerializeField] GameObject special;
    float primaryCooldown = 1.4f;
    float secondaryCooldown = 0.7f;
    float specialCooldown = 0.3f;
    public bool changingToPrimary;
    public bool changingToSecondary;
    public bool changingToSpecial;
    public short lastWeapon = 1;
    public short actualWeapon = 1;
    public short auxiliar = 1;

    // Update is called once per frame
    void Update()
    {
        // Changing to a weapon will lock the state to not swap any other weapon
        // until the weapon has been changed
        if (changingToPrimary || changingToSecondary || changingToSpecial)
        {
            // In case the time to enable the gameobject has passed and it has not been enabled
            // enable it manually
            if (timeLeftUntilGameObjectIsEnabled < Time.time - 0.1f)
            {
                Debug.LogError("Gotta fix this dawg");
            }
            return;
        }
        if (Input.GetKeyDown("1"))
        {
            if (actualWeapon == 1)
            {
                return;
            }
            timeLeftUntilGameObjectIsEnabled = Time.time + primaryCooldown;
            lastWeapon = actualWeapon;
            StartCoroutine(EnableWeapon(1, primaryCooldown));
            actualWeapon = 1;
        }
        if (Input.GetKeyDown("2"))
        {
            if (actualWeapon == 2)
            {
                return;
            }
            timeLeftUntilGameObjectIsEnabled = Time.time + secondaryCooldown;
            lastWeapon = actualWeapon;
            StartCoroutine(EnableWeapon(2, secondaryCooldown));
            actualWeapon = 2;
        }
        if (Input.GetKeyDown("3"))
        {
            if (actualWeapon == 3)
            {
                return;
            }
            timeLeftUntilGameObjectIsEnabled = Time.time + specialCooldown;
            lastWeapon = actualWeapon;
            StartCoroutine(EnableWeapon(3, specialCooldown));
            actualWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            auxiliar = actualWeapon;
            actualWeapon = lastWeapon;
            lastWeapon = auxiliar;
            switch(actualWeapon)
            {
                case 1:
                    {
                        timeLeftUntilGameObjectIsEnabled = Time.time + primaryCooldown;
                        StartCoroutine(EnableWeapon(actualWeapon, primaryCooldown));
                        break;
                    }
                case 2:
                    {
                        timeLeftUntilGameObjectIsEnabled = Time.time + secondaryCooldown;
                        StartCoroutine(EnableWeapon(actualWeapon, secondaryCooldown));
                        break;
                    }
                case 3:
                    {
                        timeLeftUntilGameObjectIsEnabled = Time.time + specialCooldown;
                        StartCoroutine(EnableWeapon(actualWeapon, specialCooldown));
                        break;
                    }
                default:
                    {
                        StartCoroutine(EnableWeapon(0, 1f));
                        break;
                    }
            }
        }
    }

    private void Start()
    {
        primaryWeapon.SetActive(true);
        secondaryWeapon.SetActive(false);
        special.SetActive(false);
        actualWeapon = 1;
        lastWeapon = 1;
        timeLeftUntilGameObjectIsEnabled = Time.time + primaryCooldown;
        StartCoroutine(EnableWeapon(1, primaryCooldown));

    }

    private IEnumerator EnableWeapon(short weaponNumber, float cooldown)
    {
        switch(weaponNumber)
        {
            case 1:
                {
                    primaryWeapon.SetActive(false);
                    secondaryWeapon.SetActive(false);
                    special.SetActive(false);
                    changingToPrimary = true;
                    yield return new WaitForSeconds(cooldown);
                    changingToPrimary = false;
                    primaryWeapon.SetActive(true);
                    break;
                }
            case 2:
                {
                    primaryWeapon.SetActive(false);
                    secondaryWeapon.SetActive(false);
                    special.SetActive(false);
                    changingToSecondary = true;
                    yield return new WaitForSeconds(cooldown);
                    changingToSecondary = false;
                    secondaryWeapon.SetActive(true);
                    break;
                }
            case 3:
                {
                    primaryWeapon.SetActive(false);
                    secondaryWeapon.SetActive(false);
                    special.SetActive(false);
                    changingToSpecial = true;
                    yield return new WaitForSeconds(cooldown);
                    changingToSpecial = false;
                    special.SetActive(true);
                    break;
                }
            default:
                { 
                    Debug.Log("Como mierda llegamos aca?");
                    primaryWeapon.SetActive(false);
                    secondaryWeapon.SetActive(false);
                    special.SetActive(false);
                    break;
                }
        }
    }
}
