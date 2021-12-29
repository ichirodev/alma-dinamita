using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class xTemplate : MonoBehaviour
{
    public bool weaponIsEnabled = true;
    public float timeAtUpdate = 0.00001f;
    public string weaponType = "primary";
    public string weaponName = "template";
    public bool isAutomatic = true;

    public short magAmmo = 30;
    public short maxMagAmmo = 30;
    public short reserveAmmo = 90;

    public float fireCooldown = 0.100f;
    public float nextBulletFired = 0.0000001f;

    public float reloadTime = 1.3f;
    public float lastReload = 0.0000001f;
    public bool isReloading = false;

    [SerializeField] Text ammoUI;
    private string ammo = "?";
    private async void Reload()
    {
        isReloading = true;
        await Task.Run(() => Thread.Sleep(4000));
        reserveAmmo = (short)(reserveAmmo + magAmmo);
        magAmmo = 0;
        if (reserveAmmo >= maxMagAmmo)
        {
            magAmmo = maxMagAmmo;
            reserveAmmo = (short)(reserveAmmo - maxMagAmmo);
        }
        else if (reserveAmmo <= maxMagAmmo && reserveAmmo > 0)
        {
            magAmmo = reserveAmmo;
            reserveAmmo = 0;
        }
        else
        {
            Debug.Log("No ammo");
        }
        isReloading = false;
    }

    void Fire()
    {
        magAmmo--;
    }

    private void Start()
    {
        weaponIsEnabled = true;
    }
    void Update()
    {
        timeAtUpdate = Time.time;
        ammo = magAmmo.ToString() + "/" + reserveAmmo.ToString();
        ammoUI.text = ammo;
        if (!weaponIsEnabled) return;

        if (!isReloading)
        {
            if (isAutomatic && Input.GetMouseButton(0))
            {
                if (magAmmo > 0)
                {
                    if (timeAtUpdate > nextBulletFired)
                    {
                        Fire();
                        nextBulletFired = nextBulletFired + fireCooldown;
                    }
                }
                else if (magAmmo == 0)
                {
                    if (lastReload < timeAtUpdate && timeAtUpdate > nextBulletFired)
                    {
                        lastReload = Time.time + reloadTime;
                        Reload();
                    }
                }
            } else if (!isAutomatic && Input.GetMouseButtonDown(0))
            {
                if (magAmmo > 0)
                {
                    if (timeAtUpdate > nextBulletFired)
                    {
                        Fire();
                        nextBulletFired = nextBulletFired + fireCooldown;
                    }
                }
                else if (magAmmo == 0)
                {
                    if (lastReload < timeAtUpdate && timeAtUpdate > nextBulletFired)
                    {
                        lastReload = Time.time + reloadTime;
                        Reload();
                    }
                }
            }
        } else if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            if (lastReload < timeAtUpdate && timeAtUpdate > nextBulletFired)
            {
                lastReload = Time.time + reloadTime;
                Reload();
            }
        }
    }

    private void OnEnable()
    {
        weaponIsEnabled = true;
        nextBulletFired = Time.time;
    }

    private void OnDisable()
    {
        weaponIsEnabled = false;
    }
}
