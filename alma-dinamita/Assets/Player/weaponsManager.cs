using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponsManager : MonoBehaviour
{
    public float timeLeftUntilGameObjectIsEnabled = 0.01f;
    [SerializeField] GameObject primaryWeapon;
    [SerializeField] GameObject secondaryWeapon;
    [SerializeField] GameObject special;
    [SerializeField] float primaryCooldown = 0.4f;
    [SerializeField] float secondaryCooldown = 9f;
    [SerializeField] float specialCooldown = 0.3f;

    public short lastWeapon = 1;
    public short actualWeapon = 1;
    public short auxiliar = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            timeLeftUntilGameObjectIsEnabled = Time.time + primaryCooldown;
            lastWeapon = actualWeapon;
            StartCoroutine(EnableWeapon(1, primaryCooldown));
            actualWeapon = 1;
        }
        if (Input.GetKeyDown("2"))
        {
            timeLeftUntilGameObjectIsEnabled = Time.time + secondaryCooldown;
            lastWeapon = actualWeapon;
            StartCoroutine(EnableWeapon(2, secondaryCooldown));
            actualWeapon = 2;
        }
        if (Input.GetKeyDown("3"))
        {
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

    private IEnumerator EnableWeapon(short weaponNumber, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        switch(weaponNumber)
        {
            case 1:
                {
                    primaryWeapon.SetActive(true);
                    secondaryWeapon.SetActive(false);
                    special.SetActive(false);
                    break;
                }
            case 2:
                {
                    primaryWeapon.SetActive(false);
                    secondaryWeapon.SetActive(true);
                    special.SetActive(false);
                    break;
                }
            case 3:
                {
                    primaryWeapon.SetActive(false);
                    secondaryWeapon.SetActive(false);
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
