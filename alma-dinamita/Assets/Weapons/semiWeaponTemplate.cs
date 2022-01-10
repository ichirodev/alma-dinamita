using UnityEngine;

public class semiWeaponTemplate : MonoBehaviour
{
    // Audio effect of the shot when fired
    [SerializeField] AudioSource weaponAS;
    [SerializeField] AudioClip weaponAC;
    // Weapon details
    public short weaponType = 1; // 0: Primary, 1: Secondary, 2: Special
    public string weaponName = "shit_gun";
    // Weapon technical stats
    [SerializeField] int magazineAmmo = 30;
    [SerializeField] int reserveAmmo = 60;
    public int reserveAux = 0;
    private int maxMagazineAmmo = 30;
    private int maxReserveAmmo = 60;
    // Status booleans
    [SerializeField] bool isReloading;
    [SerializeField] bool isShooting;
    [SerializeField] bool isWeaponEnabled;

    private float nextShootTime = 0.00000001f;
    private float nextShootDelay = 0.10f;
    private float finishReloadingTime = 0.0000001f;
    private float reloadDelay = 2.1f;

    public float damage = 20f;
    public float range = 100.03f;

    public Camera firstPersonCamera;
    public bool debugHit = true;
    [SerializeField] GameObject debugHitGameObject;

    [SerializeField] const float swapCooldown = 0.4f;

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
                return;
            }
            isReloading = false;
        }

        if (!WeaponShootIsOnCooldown(nextShootTime))
        {
            if (Input.GetMouseButtonDown(0))
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
            nextShootTime = Time.time + nextShootDelay;
            weaponAS.Play();
            Shoot();
            magazineAmmo--;
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

    private void OnEnable()
    {
        isWeaponEnabled = true;
        weaponAS.clip = weaponAC;
    }

    private void OnDisable()
    {
        isWeaponEnabled = false;
    }
}
