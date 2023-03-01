using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyNameSpace;

public class Bullet : MonoBehaviour
{
    //PRIVATE FIELDS
    private Rigidbody rb;
    private ObjectPool objectPool;
    public AudioClip clip;
    private Vector3 explosionPos;
    private WeaponUpgrade weaponUpgrade;

    public ParticleSystem explosionEffect;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        objectPool = FindObjectOfType<ObjectPool>();
        weaponUpgrade = FindObjectOfType<WeaponUpgrade>();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            transform.SetParent(objectPool.transform);
            transform.localPosition = Vector3.zero;
            explosionPos = other.gameObject.transform.position;
            gameObject.SetActive(false);
           other.gameObject.GetComponent<EnemyHealth>().SetDamage(20);

            if (gameObject.tag.Equals("Rocket"))
            {
                
                GameObject explosionPoint = new GameObject("ExplosionPoint");
                explosionPoint.transform.SetParent(null);
                explosionPoint.transform.position = explosionPos;
                 explosionPoint.AddComponent<RocketEffectArea>();

                ParticleSystem s = Instantiate(explosionEffect);
                s.transform.position = explosionPos;
                s.Play();              
                
            }

        }

        if(other.gameObject.tag.Equals("money")){
               if(other.transform.tag.Equals("money")){
                GameObject money = other.gameObject;
                money.GetComponent<Money>().Played();
            }
        }

    }


    private void BulletRepair()
    {
        transform.SetParent(objectPool.transform);
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
    public void ShootBullet(Vector3 dir, float force)
    {
        weaponUpgrade.isActiveRpg7 = false;
        Invoke("BulletRepair", 3);
        transform.rotation = Quaternion.Euler(90, 180, 0);
        UIManager.Instance.SoundController(clip);
        rb.velocity = dir * force;
    }

    

    
     
}
