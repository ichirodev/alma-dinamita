using UnityEngine;

public class pickWeapon : MonoBehaviour
{
    [SerializeField] public string typeOf; // Choose what script to use: weaponSA or weaponA
    [SerializeField] public short weaponType; // 1: Primary, 2: Secondary, 3: Special
    [SerializeField] public string weaponName;
    [SerializeField] public short maxMagAmmo;
    [SerializeField] public short currentMagAmmo;
    [SerializeField] public short maxReserveAmmo;
    [SerializeField] public short currentReserveAmmo;
    [SerializeField] public string weaponModel;
    [SerializeField] public float reloadTime;
    [SerializeField] public float range;
    [SerializeField] public float betweenShotsTime;
    [SerializeField] public float damage;
    [SerializeField] public AudioClip clip;
    public bool isAutomatic()
    {
        return typeOf == "weaponA";
    }
}
