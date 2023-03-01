using UnityEngine;
using EnemyNameSpace;
using System;

public class Bomba : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 pos;
    float power;
    float upwards;
    float radius;
    public GameObject Particle;

    public AudioClip bombClip;

    private bool dieController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Die(bool obj)
    {
        dieController = obj;

    }
    EnemyAttack enemyAttack;
    public void SetData(EnemyAttack attack, float power, Vector3 expolotionForce, float upwards, float radius)
    {
        enemyAttack = attack;
        rb = GetComponent<Rigidbody>();
        pos = expolotionForce;
        this.power = power;
        this.upwards = upwards;
        this.radius = radius;
    }




    private void OnCollisionEnter(Collision other)
    {

        GetComponent<AudioSource>().PlayOneShot(bombClip);

        GameObject particule = Instantiate(Particle, transform.position, Quaternion.identity);

        particule.transform.SetParent(transform);

        if (other.gameObject.CompareTag("Player"))
            enemyAttack.PlayerSetDamage(enemyAttack.enemyattack.damage);

        rb.AddExplosionForce(power, pos, upwards, radius, ForceMode.Impulse);
        Destroy(gameObject, 0.6f);
    }


    public void AddForce(Vector3 force)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(force + Vector3.up * 5F, ForceMode.Impulse);
    }



}
