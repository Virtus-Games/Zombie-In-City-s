using UnityEngine;
namespace EnemyNameSpace
{
    public class BasicAttack : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<IHealth>().SetDamage(GetComponentInParent<EnemyController>().healthDamage);
        }
    }
}