using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("General")]
    public string weaponName = "";

    public int ammoCount;

    public WeaponSlot weaponSlot;
    public enum WeaponSlot {Primary, Secondary }

    [Header("Firing")]
    public float fireRate = 0.5f;
    public float projectileSpeed = 30f;
    //public float recoilForce = 100f;

    [Header("Damage")]
    public float damage = 20f;
    public float explosionRadius = 2f; // For AOE weapons

    [Header("Visuals")]
    public GameObject projectilePrefab;
    public GameObject muzzleFlashPrefab;
    public AudioClip fireSound;

    [Header("Visuals")]
    public string projectilePoolTag;
    public string muzzleFlashPoolTag;
    public string shellPoolTag;

}