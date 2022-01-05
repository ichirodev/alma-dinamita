using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponTemplate : MonoBehaviour
{
    // Audio effect of the shot when fired
    [SerializeField] AudioSource actualWeaponAudioSource;
    [SerializeField] AudioClip actualWeaponAudioClip;
    // Weapon details
    public short weaponType = 0; // 0: Primary, 1: Secondary, 2: Special
    public string weaponName = "shit_gun";
    // Weapon technical stats
    public int magazineAmmo = 30;
    public int reserveAmmo = 60;
    public int magazineAux = 0;
    public int reserveAux = 0;
    public int maxMagazineAmmo = 30;
    public int maxReserveAmmo = 60;
    // Status booleans
    public bool isReloading, isShooting, isWeaponEnabled;
    public bool m1 = false;

    public float nextShootTime = 0.00000001f;
    public float nextShootDelay = 3.0f;
    public float finishReloadingTime = 0.0000001f;
    public float reloadDelay = 3.0f;

    private void Start()
    {
        isWeaponEnabled = true;
        isShooting = false;
        isReloading = false;
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
                if (reserveAmmo > maxMagazineAmmo)
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
    }

    private bool WeaponShootIsOnCooldown(float nextShoot)
    {
        if (Time.time < nextShoot)
        {
            Debug.Log("can't shoot lmao");
            return true;
        }
        return false;
    }

    private bool WeaponFire()
    {
        nextShootTime = Time.time + nextShootDelay;
        actualWeaponAudioSource.Play();
        magazineAmmo--;
            
        return true;
    }

}
