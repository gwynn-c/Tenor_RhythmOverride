using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Weapon Information")]
    public string _name;
    public string _description;
    public Sprite _icon;

    [Header("Weapon References")]
    public GameObject bulletPrefab;
    public GameObject muzzleVFXPrefab;
    
    [Header("Weapon Stats")]
    public float shootForce;
    public float upwardForce;
    public float timeBetweenShots;
    public float spread;
    public float timebetweenShooting;
    public int bulletsPerShot;
}