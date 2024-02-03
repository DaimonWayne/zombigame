using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Types", menuName = "Gun Type")]
public class GunType : ScriptableObject
{

    public enum ShootingMode
    {
        single,
        serial
    }

    public enum Ammunition
    {
        bullet,
        flare
    }

    
    [Header("Weapon Properties")]
    public ShootingMode _mode;
    public Ammunition _ammunition;
    public float _range;
    public float fireRate;
    public float _damage;
    public int _bulletCapacity;
}
