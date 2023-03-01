using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private ParticleSystem ParticleBullet;
    private Rigidbody rb;
    private GameObject _Player;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
//            rb.useGravity = false;
            
            GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(DestroyBullet(0.5f));

            // Health health = other.gameObject.GetComponent<Health>();
            // float healthValue = health.SetDamage(-damage);
        }
    }

    IEnumerator DestroyBullet(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void SetData(Vector3 to, float speed, float dam)
    {
        //rb = GetComponent<Rigidbody>();
        //rb.AddForceAtPosition(to * speed * Time.deltaTime, transform.position,ForceMode.Impulse);
    }
}