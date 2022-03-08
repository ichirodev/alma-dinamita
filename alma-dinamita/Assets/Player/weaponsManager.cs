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
    // Change to: Weapon Object
    private Object changeToThis;
    public Camera changeToCamera;
    public bool debugHit = true;
    // Escape menu
    [SerializeField] public menu escMenu;
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
            if (escMenu.isOpen) return;
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
            if (escMenu.isOpen) return;
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
            if (escMenu.isOpen) return;
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
            if (escMenu.isOpen) return;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (escMenu.isOpen) return;
            ChangeToNewWeaponScript();
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

    private void ChangeToNewWeaponScript()
    {
        Vector3 rayHitPosition = changeToCamera.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(changeToCamera.transform.position, rayHitPosition, out hit, 25))
        {
            if (hit.transform.CompareTag("CanBePicked"))
            {
                
                pickWeapon tempPickWeapon = hit.transform.GetComponent<pickWeapon>();
                int pickedWeaponType = tempPickWeapon.weaponType;
                switch (pickedWeaponType)
                {
                    case 1:
                    {
                        Debug.Log("Change primary to" + hit.transform.name);
                        if (hit.transform.GetComponent<pickWeapon>().isAutomatic())
                        {
                            // Add the required component to the GameObject
                            primaryWeapon.AddComponent<weaponA>();
                            // Pass the picked weapon stats to the in-hand weapon
                            var tempWA = primaryWeapon.GetComponent<weaponA>();
                            tempWA.weaponAC = tempPickWeapon.clip;
                            tempWA.weaponAS =
                                GameObject.Find("ShootingAudioSource").GetComponent<AudioSource>();
                            tempWA.enabled = false;
                            tempWA.firstPersonCamera =
                                GameObject.Find("WeaponCamera").GetComponent<Camera>();
                            tempWA.SetDamage(tempPickWeapon.damage);
                            tempWA.SetRange(tempPickWeapon.range);
                            tempWA.SetReloadDelay(tempPickWeapon.reloadTime);
                            tempWA.SetWeaponAmmo(tempPickWeapon.currentMagAmmo,
                                tempPickWeapon.currentReserveAmmo,
                                tempPickWeapon.maxMagAmmo,
                                tempPickWeapon.maxReserveAmmo);
                            tempWA.SetNextShootDelay(tempPickWeapon.betweenShotsTime);
                            tempWA.weaponName = tempPickWeapon.weaponName;
                            tempWA.weaponType = tempPickWeapon.weaponType;
                            tempWA.debugHit = false;
                            
                            // Change to primary weapon and activate its script
                            timeLeftUntilGameObjectIsEnabled = Time.time + primaryCooldown;
                            lastWeapon = actualWeapon;
                            StartCoroutine(EnableWeapon(1, primaryCooldown));
                            actualWeapon = 1;
                            tempWA.enabled = true;
                        }
                        else
                        {
                            // Add the required component to the GameObject
                            primaryWeapon.AddComponent<weaponSA>();
                            // Pass the picked weapon stats to the in-hand weapon
                            var tempWSA = primaryWeapon.GetComponent<weaponSA>();
                            tempWSA.weaponAC = tempPickWeapon.clip;
                            tempWSA.weaponAS =
                                GameObject.Find("ShootingAudioSource").GetComponent<AudioSource>();
                            tempWSA.enabled = false;
                            tempWSA.firstPersonCamera =
                                GameObject.Find("WeaponCamera").GetComponent<Camera>();
                            tempWSA.SetDamage(tempPickWeapon.damage);
                            tempWSA.SetRange(tempPickWeapon.range);
                            tempWSA.SetReloadDelay(tempPickWeapon.reloadTime);
                            tempWSA.SetWeaponAmmo(tempPickWeapon.currentMagAmmo,
                                tempPickWeapon.currentReserveAmmo,
                                tempPickWeapon.maxMagAmmo,
                                tempPickWeapon.maxReserveAmmo);
                            tempWSA.SetNextShootDelay(tempPickWeapon.betweenShotsTime);
                            tempWSA.weaponName = tempPickWeapon.weaponName;
                            tempWSA.weaponType = tempPickWeapon.weaponType;
                            tempWSA.debugHit = false;
                            
                            // Change to primary weapon and activate its script
                            timeLeftUntilGameObjectIsEnabled = Time.time + primaryCooldown;
                            lastWeapon = actualWeapon;
                            StartCoroutine(EnableWeapon(1, primaryCooldown));
                            actualWeapon = 1;
                            tempWSA.enabled = true;
                            
                        }
                        Destroy(hit.transform.gameObject);
                        break;
                    }
                    case 2:
                    {
                        Debug.Log("Change secondary to " + hit.transform.name);
                        if (hit.transform.GetComponent<pickWeapon>().isAutomatic())
                        {
                            // Add the required component to the GameObject
                            secondaryWeapon.AddComponent<weaponA>();
                            // Pass the picked weapon stats to the in-hand weapon
                            var tempWA = secondaryWeapon.GetComponent<weaponA>();
                            tempWA.weaponAC = tempPickWeapon.clip;
                            tempWA.weaponAS =
                                GameObject.Find("ShootingAudioSource").GetComponent<AudioSource>();
                            tempWA.enabled = false;
                            tempWA.firstPersonCamera =
                                GameObject.Find("WeaponCamera").GetComponent<Camera>();
                            tempWA.SetDamage(tempPickWeapon.damage);
                            tempWA.SetRange(tempPickWeapon.range);
                            tempWA.SetReloadDelay(tempPickWeapon.reloadTime);
                            tempWA.SetWeaponAmmo(tempPickWeapon.currentMagAmmo,
                                tempPickWeapon.currentReserveAmmo,
                                tempPickWeapon.maxMagAmmo,
                                tempPickWeapon.maxReserveAmmo);
                            tempWA.SetNextShootDelay(tempPickWeapon.betweenShotsTime);
                            tempWA.weaponName = tempPickWeapon.weaponName;
                            tempWA.weaponType = tempPickWeapon.weaponType;
                            tempWA.debugHit = false;
                            
                            // Change to primary weapon and activate its script
                            timeLeftUntilGameObjectIsEnabled = Time.time + secondaryCooldown;
                            lastWeapon = actualWeapon;
                            StartCoroutine(EnableWeapon(2, secondaryCooldown));
                            actualWeapon = 2;
                            tempWA.enabled = true;
                        }
                        else
                        {
                            // Add the required component to the GameObject
                            secondaryWeapon.AddComponent<weaponSA>();
                            // Pass the picked weapon stats to the in-hand weapon
                            var tempWSA = secondaryWeapon.GetComponent<weaponSA>();
                            tempWSA.weaponAC = tempPickWeapon.clip;
                            tempWSA.weaponAS =
                                GameObject.Find("ShootingAudioSource").GetComponent<AudioSource>();
                            tempWSA.enabled = false;
                            tempWSA.firstPersonCamera =
                                GameObject.Find("WeaponCamera").GetComponent<Camera>();
                            tempWSA.SetDamage(tempPickWeapon.damage);
                            tempWSA.SetRange(tempPickWeapon.range);
                            tempWSA.SetReloadDelay(tempPickWeapon.reloadTime);
                            tempWSA.SetWeaponAmmo(tempPickWeapon.currentMagAmmo,
                                tempPickWeapon.currentReserveAmmo,
                                tempPickWeapon.maxMagAmmo,
                                tempPickWeapon.maxReserveAmmo);
                            tempWSA.SetNextShootDelay(tempPickWeapon.betweenShotsTime);
                            tempWSA.weaponName = tempPickWeapon.weaponName;
                            tempWSA.weaponType = tempPickWeapon.weaponType;
                            tempWSA.debugHit = false;
                            
                            // Change to primary weapon and activate its script
                            timeLeftUntilGameObjectIsEnabled = Time.time + secondaryCooldown;
                            lastWeapon = actualWeapon;
                            StartCoroutine(EnableWeapon(2, secondaryCooldown));
                            actualWeapon = 2;
                            tempWSA.enabled = true;
                        }
                        Destroy(hit.transform.gameObject);
                        break;
                    }
                    case 3:
                    {
                        Debug.Log("Change special to " + hit.transform.name);
                        Destroy(hit.transform.gameObject);
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                //hit.transform.GetComponent<pickWeapon>().GetWeaponComponent();
            }
        }
    }
}
