using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponUpgrade : MonoBehaviour
{
    private GunOffsetSettings gunOffsetSettings;
    private Weapon activeWeapon;

    [HideInInspector]
    public bool doubleShoot = false;

    [HideInInspector]
    public bool isActiveRpg7 = true;


    private void Start()
    {
        gunOffsetSettings = GetComponentInChildren<GunOffsetSettings>();
        activeWeapon = gunOffsetSettings.activeWeapon.GetComponent<Weapon>();
    }


    public void UpdateShootingFrequency(float time,float newValue,float currentValue)
    {
        activeWeapon = gunOffsetSettings.activeWeapon.GetComponent<Weapon>();
        StartCoroutine(UpdateShootingFrequencyTimer(time, newValue, currentValue));
    }

    public void OpenRpg7()
    {
        isActiveRpg7 = true;
        StartCoroutine(UpdateRpg7Timer());
     }
    public void UpdateDoubleShoot(float time,bool newValue,bool currentValue)
    {
        activeWeapon = gunOffsetSettings.activeWeapon.GetComponent<Weapon>();

        StartCoroutine(UpdateDoubleShootTime(time, newValue, currentValue));
    }

    private IEnumerator UpdateShootingFrequencyTimer(float time, float newValue, float currentValue)
    {
        float value = currentValue;
        gunOffsetSettings.activeWeapon.GetComponent<Weapon>().settingsSO.shootingFrequency = newValue;
        WaitForSeconds wait = new WaitForSeconds(time);
        yield return wait;
        gunOffsetSettings.activeWeapon.GetComponent<Weapon>().settingsSO.shootingFrequency = value;

    }
    private IEnumerator UpdateDoubleShootTime(float time, bool newValue, bool currentValue)
    {
        bool value = currentValue;
        doubleShoot = newValue;
        WaitForSeconds wait = new WaitForSeconds(time);
        yield return wait;
        doubleShoot = value;

    }

   private IEnumerator UpdateRpg7Timer()
    {
        gunOffsetSettings.SetWeaponIdlePose(SelectWeapon.Rpg7);
        WaitUntil wait = new WaitUntil(() => isActiveRpg7 == false);
        yield return wait;

        foreach (var w in gunOffsetSettings.Weapons)
        {
            if (w.name.Equals(activeWeapon.name))
            {
                foreach (SelectWeapon s in Enum.GetValues(typeof(SelectWeapon)))
                {
                    if (w.name.Equals(s.ToString()))
                    {
                        gunOffsetSettings.SetWeaponAimPose(s);
                    }
                }
            }

        }

    }
}
