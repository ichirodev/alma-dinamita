using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class weaponA : MonoBehaviour
{
    // Menu control
    private menu menuEscape;
    // Audio effect of the shot when fired
    [SerializeField] public AudioSource weaponAS;
    [SerializeField] public AudioClip weaponAC;
    // Weapon details
    public short weaponType = 0; // 0: Primary, 1: Secondary, 2: Special
    public string weaponName = "Nerf Pistol";
    // Weapon technical stats
    [SerializeField] int magazineAmmo = 12;
    [SerializeField] int reserveAmmo = 48;
    public int reserveAux = 0;
    private int maxMagazineAmmo = 12;
    private int maxReserveAmmo = 60;
    private float damage = 18f;
    private float range = 100.03f;
    // Status booleans
    [SerializeField] bool isReloading;
    [SerializeField] bool isShooting;
    [SerializeField] bool isWeaponEnabled;
    // Timings
    private float nextShootTime = 0.00000001f;
    private float nextShootDelay = 0.2440f;
    private float finishReloadingTime = 0.0000001f;
    private float reloadDelay = 0.7f;
    // Shooting targets
    public Camera firstPersonCamera;
    public bool debugHit = true;
    [SerializeField] GameObject debugHitGameObject;
    // Weapon recoil
    public class RecoilPattern
    {
        public float[] x;
        public float[] y;
        public int i = 0;
        public int c = 0;

        public RecoilPattern(float[] _x, float[] _y)
        {
            if (_x.Length != _y.Length) 
                throw new Exception("RecoilPattern constructor failed because x and y arrays have a different length");
            

            i = _x.Length;
            x = _x;
            y = _y;
        }

        public float[] GetRecoilAt(int atIndex)
        {
            float[] r = new[] {0.0f, 0.0f};
            try
            {
                r[0] = x[atIndex];
                r[1] = y[atIndex];
            }
            catch (IndexOutOfRangeException indexException)
            {
                r[0] = x[x.Length - 1];
                r[1] = y[y.Length - 1];
                Debug.Log(indexException.Message);
            }
            return r;
        }

        public float[] GetNextRecoil()
        {
            float[] r = new[] {0.0f, 0.0f};
            
            try
            {
                r[0] = x[c];
                r[1] = y[c];
            }
            catch (IndexOutOfRangeException indexException)
            {
                r[0] = 0.0f;
                r[1] = 0.0f;
                Debug.Log(indexException.Message);
            }
            UpdateNextRecoilCounter();
            return r;
        }

        public void UpdateNextRecoilCounter()
        {
            if (c < i-1) 
                c = c + 1;
        }

        public int RestartRecoilCounter()
        {
            c = 0;
            return c;
        }
    }

    private float[] defaultRecoilPatternX = new []
    {
        0.0f, 0.002f, -0.0021f, 0.0024f, -0.008f, 0.0064f, -0.00334f, -0.00453f, 0.0456f, 0.0097f, -0.01577f, 0.00578f, 0.005902f, -0.06111f
    };
    private float[] defaultRecoilPatternY  = new []
    {
        0.0f, 0.01f, 0.02f, 0.025f, 0.029f, 0.03f, 0.06f, 0.0777f, 0.08f, 0.092f, 0.093f, 0.097f, 0.0978f, 0.0789f
    };

    private RecoilPattern weaponRecoil;
    // GUI
    public bool showAmmoOnHUD = true;
    private Text ammoText;
    private void Start()
    {
        weaponRecoil = new RecoilPattern(defaultRecoilPatternX, defaultRecoilPatternY);
        isWeaponEnabled = true;
        isShooting = false;
        isReloading = false;
        firstPersonCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (menuEscape.isOpen) return;
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
            weaponRecoil.RestartRecoilCounter();
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
            weaponRecoil.RestartRecoilCounter();
        }

        if (showAmmoOnHUD)
        {
            ammoText.text = magazineAmmo.ToString() + "/" + reserveAmmo.ToString();
        }
    }
    
    
    private void OnEnable()
    {
        menuEscape = GetComponentInParent<menu>();
        if (showAmmoOnHUD)
        {
            ammoText = GameObject.Find("Ammo").GetComponent<Text>();
            ammoText.enabled = true;
        }
        if (magazineAmmo > maxMagazineAmmo) magazineAmmo = maxMagazineAmmo;
        isWeaponEnabled = true;
        if (weaponAC != null && weaponAS != null)
        {
            weaponAS.clip = weaponAC;
        }
    }

    private void OnDisable()
    {
        isWeaponEnabled = false;
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
        var nextRecoilPosition = weaponRecoil.GetNextRecoil();
        nextBulletRayPosition.z = nextBulletRayPosition.z + nextRecoilPosition[0];
        nextBulletRayPosition.x = nextBulletRayPosition.x + nextRecoilPosition[0];
        nextBulletRayPosition.y = nextBulletRayPosition.y + nextRecoilPosition[1];
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

    public void SetReloadDelay(float t)
    {
        reloadDelay = t;
    }

    public void SetNextShootDelay(float t)
    {
        nextShootDelay = t;
    }

    public void SetDamage(float d)
    {
        damage = d;
    }

    public void SetRange(float r)
    {
        range = r;
    }

    public void SetWeaponAmmo(int curMag, int curRes, int maxMag, int maxRes)
    {
        maxMagazineAmmo = maxMag;
        maxReserveAmmo = maxRes;
        magazineAmmo = curMag;
        reserveAmmo = curRes;
    }

}
