using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketEffectArea : MonoBehaviour
{
    private float effectAreaRadius = 8f;
    

     


    void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, effectAreaRadius);
        if (enemies.Length > 0)
        {
            foreach (var zombie in enemies)
            {
                if (zombie.tag.Equals("enemy"))
                {
                    EnemyNameSpace.EnemyHealth enemyHealth = zombie.GetComponent<EnemyNameSpace.EnemyHealth>();
                  
                    
                    enemyHealth.DieController();
                }
               

            }
        }

        else{
            Destroy(gameObject);
        }

    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, effectAreaRadius);
    }

}
