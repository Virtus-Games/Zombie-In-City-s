using PlayerNameSpace;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor;

public enum SelectWeapon
{
    Rpg7,
    Pistol,
    AK_47,
    P90,
    Rifle

}
public class GunOffsetSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject activeWeapon;
  
    public SelectWeapon selectWeapon;

    [SerializeField] List<Rig> rigs;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<Rig> poses;

   

    public List<GameObject> Weapons
    {
        get { return weapons; }
    }
     

    public void SetWeaponIdlePose(SelectWeapon selectWeapon)
    {
        string weaponName = selectWeapon.ToString();

        foreach (var w in weapons)
        {
            if (w.name.Equals(weaponName))
            {
                w.gameObject.SetActive(true);
                SetWeaponRigLayer(weaponName);
                SetWeaponIdlePose(weaponName);
                activeWeapon = w.gameObject;
            }
            else
            {
                w.gameObject.SetActive(false);
            }
        }
    }
    public void SetWeaponAimPose(SelectWeapon selectWeapon)
    {
        string weaponName = selectWeapon.ToString();

        foreach (var w in weapons)
        {
            if (w.name.Equals(weaponName))
            {
                w.gameObject.SetActive(true);
                SetWeaponRigLayer(weaponName);
                SetWeaponAimingPose(weaponName);
                activeWeapon = w.gameObject;

            }
            else
            {
                w.gameObject.SetActive(false);
            }
        }

    }

    private void SetWeaponRigLayer(string rigLayer)
    {
        foreach (var r in rigs)
        {
            if (r.name.Equals(rigLayer))
            {
                r.weight = 1;
            }
            else
            {
                r.weight = 0;
            }
        }
    }

    private void SetWeaponIdlePose(string weaponName)
    {
        string poseName = weaponName + "Idle";
        foreach (var w in poses)
        {
            if (w.name.Equals(poseName))
            {
                w.weight = 1;
            }
            else
            {
                w.weight = 0;
            }
        }
    }
    private void SetWeaponAimingPose(string weaponName)
    {
        string poseName = weaponName + "Aiming";
        foreach (var w in poses)
        {
            if (w.name.Equals(poseName))
            {
                w.weight = 1;
            }
            else
            {
                w.weight = 0;
            }
        }
    }

    

}


#if UNITY_EDITOR
[CustomEditor(typeof(GunOffsetSettings))]
public class GunOffsetSettingsEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GunOffsetSettings script = (GunOffsetSettings)target;
        if (GUILayout.Button("Set Weapon Idle Pose")) {
            script.SetWeaponAimPose(script.selectWeapon);
        }
        
    }
}

#endif