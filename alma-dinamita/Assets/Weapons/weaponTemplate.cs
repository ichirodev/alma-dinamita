using UnityEngine.UI;
using UnityEngine;

public class weaponTemplate : MonoBehaviour
{
    // Audio effect of the shot when fired
    [SerializeField] AudioSource weaponAS;
    [SerializeField] AudioClip weaponAC;
    // Weapon details
    public short weaponType = 0; // 0: Primary, 1: Secondary, 2: Special
    public string weaponName = "Rifle";
    // Weapon technical stats
    [SerializeField] int magazineAmmo = 45;
    [SerializeField] int reserveAmmo = 135;
    public int reserveAux = 0;
    private int maxMagazineAmmo = 45;
    private int maxReserveAmmo = 135;
    public float damage = 24.3f;
    public float range = 100.03f;
    // Status booleans
    [SerializeField] bool isReloading;
    [SerializeField] bool isShooting;
    [SerializeField] bool isWeaponEnabled;
    // Timings
    private float nextShootTime = 0.00000001f;
    private float nextShootDelay = 0.14f;
    private float finishReloadingTime = 0.0000001f;
    private float reloadDelay = 1.28f;
    // Shooting targets
    public Camera firstPersonCamera;
    public bool debugHit = true;
    [SerializeField] GameObject debugHitGameObject;
    // GUI
    private Text textAmmo;

    private void Start()
    {
        isWeaponEnabled = true;
        isShooting = false;
        isReloading = false;
        firstPersonCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || magazineAmmo == 0)
        {
            if (magazineAmmo < maxMagazineAmmo)
            {
                if (reserveAmmo <= 0)
                {
                    return;
                }
                reserveAmmo += magazineAmmo;
                if (reserveAmmo >= maxMagazineAmmo)
                {
                    magazineAmmo = maxMagazineAmmo;
                    reserveAux = reserveAmmo - magazineAmmo;
                    reserveAmmo = reserveAux;
                    isReloading = true;
                } else if (reserveAmmo < maxMagazineAmmo && reserveAmmo > 0)
                {
                    magazineAmmo = reserveAmmo;
                    reserveAmmo = 0;
                    isReloading = true;
                }
            }
            finishReloadingTime = Time.time + reloadDelay;
        }

        if (isReloading)
        {
            if (finishReloadingTime > Time.time)
            {
                textAmmo.text = "Reloading";
                return;
            }
            isReloading = false;
        }

        if (!WeaponShootIsOnCooldown(nextShootTime))
        {
            if (Input.GetMouseButton(0))
            {
                if (isWeaponEnabled && !isReloading)
                {
                    isShooting = WeaponFire();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }

        textAmmo.text = magazineAmmo.ToString() + "/" + reserveAmmo.ToString();

    }

    private bool WeaponShootIsOnCooldown(float nextShoot)
    {
        if (Time.time < nextShoot)
        {
            return true;
        }
        return false;
    }

    private bool WeaponFire()
    {
        if (magazineAmmo >= 1)
        {
            Shoot();
            weaponAS.Play();
            magazineAmmo--;
            nextShootTime = Time.time + nextShootDelay;
            return true;
        }
        return false;
    }

    private void Shoot()
    {
        Vector3 nextBulletRayPosition = firstPersonCamera.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(firstPersonCamera.transform.position, nextBulletRayPosition, out hit, range))
        {
            if (hit.transform.tag == "OtherPlayer")
            {
                hit.transform.GetComponent<health>().SetDamageOnHealth(damage);
            }
            if (debugHit)
            {
                Instantiate(debugHitGameObject, hit.point, Quaternion.identity);
            }
        }
    }

    private void RefillAmmo()
    {
        reserveAmmo = maxReserveAmmo;
    }

    private void OnEnable()
    {
        if (magazineAmmo > maxMagazineAmmo) magazineAmmo = maxMagazineAmmo;
        textAmmo = GameObject.Find("Ammo").GetComponent<Text>();
        isWeaponEnabled = true;
        weaponAS.clip = weaponAC;
    }

    private void OnDisable()
    {
        isWeaponEnabled = false;
    }
}
