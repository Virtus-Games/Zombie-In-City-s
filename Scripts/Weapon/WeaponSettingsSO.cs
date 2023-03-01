using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weapon")]
public class WeaponSettingsSO : ScriptableObject
{
    public float shootingSpeed;
    public float shootingFrequency;
    public GameObject bulletPrefab;

}
