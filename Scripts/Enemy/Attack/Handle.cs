using UnityEngine;

namespace EnemyNameSpace
{
    public class Handle : MonoBehaviour, IAttack
    {
        private float dam = -20f;
        EnemyAttack enemyattack;
        BoxCollider boxCollider;
        private bool die;

        private void Start()
        {
            
            boxCollider = GetComponent<BoxCollider>();
            enemyattack = GetComponentInParent<EnemyAttack>();
            enemyattack.OnAttackHandle += Attack;
        }


        private void Attack(bool isDie, float damage)
        {
            dam = damage;
            die = isDie;
            boxCollider.enabled = !isDie;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !die)
            {
                float health = enemyattack.PlayerSetDamage(dam);
                die = true;
                boxCollider.enabled = false;
            }

            if(other.CompareTag("Helper") && !die)
                other.GetComponent<HealthControl>().Health(dam);


        }

        public void SetDamage(float damage) => dam = damage;
        private void OnDisable() => enemyattack.OnAttackHandle -= Attack;

    }

}