
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayerNameSpace
{
    public enum EnemyState
    {
        multiple,
        single,
        none
    }
    public class Shooting : MonoBehaviour
    {

        [HideInInspector]
        public EnemyState enemyState;

        public GunOffsetSettings gunOffsetSettings;
        private WeaponUpgrade weaponUpgrade;
        private ObjectPool objectPool;

        private int bulletIndex = 0;

        private float shootTime = .5f;


        private void Start()
        {
            weaponUpgrade = GetComponent<WeaponUpgrade>();
        }
        public void Shoot(Collider[] targetPoses)
        {
            objectPool = gunOffsetSettings.activeWeapon.GetComponentInChildren<ObjectPool>();
            float shootingFrequency = gunOffsetSettings.activeWeapon.GetComponent<Weapon>().settingsSO.shootingFrequency;

            if (Time.time > shootingFrequency + shootTime)
            {
                shootTime = Time.time;
                if (targetPoses.Length != 0)
                {
                    switch (enemyState)
                    {
                        case EnemyState.multiple:

                            if (weaponUpgrade.doubleShoot)
                                DoubleShoot(targetPoses);
                            else
                                SingleShoot(targetPoses);

                            break;

                        case EnemyState.single:

                            SingleShoot(targetPoses);

                            break;
                    }
                }
            }


        }



        private void SingleShoot(Collider[] targetPoses)
        {
            Vector3 dir;
            GameObject bullet;

         

            if (bulletIndex == objectPool.poolObjects.Count)
            {
                bulletIndex = 0;
            }
            dir = (targetPoses[0].transform.position - objectPool.transform.position + new Vector3(0, 1.5f, 0)).normalized;

            bullet = objectPool.poolObjects[bulletIndex];

            bullet.SetActive(true);

            bullet.transform.SetParent(null);

            bullet.GetComponent<Bullet>().ShootBullet(dir, gunOffsetSettings.activeWeapon.GetComponent<Weapon>().settingsSO.shootingSpeed);

            bulletIndex++;

        }

        private void DoubleShoot(Collider[] targetPoses)
        {
            GameObject bullet;
            Vector3 dir;



            for (int i = 0; i < 2; i++)
            {
                if (bulletIndex == objectPool.poolObjects.Count)
                {
                    bulletIndex = 0;
                }
                dir = (targetPoses[i].transform.position - objectPool.transform.position).normalized;

                bullet = objectPool.poolObjects[bulletIndex];

                bullet.SetActive(true);

                bullet.transform.SetParent(null);
                 bullet.GetComponent<Bullet>().ShootBullet(dir, gunOffsetSettings.activeWeapon.GetComponent<Weapon>().settingsSO.shootingSpeed);
               

                bulletIndex++;
            }
        }

    }


}