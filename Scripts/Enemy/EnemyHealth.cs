using UnityEngine;
using UnityEngine.UI;

namespace EnemyNameSpace
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        private const string PlayerTag = "Player";
        EnemyController enemyController;
        EnemyAttack enemyAttack;
        public GameObject HealthBar;
        public bool isHaveHealtnBar = false;
        public Image healthBarImage;
        public float speed = 20;
        public Vector3 directionUp;

        private void Start()
        {
            enemyAttack = GetComponent<EnemyAttack>();
            enemyController = GetComponent<EnemyController>();
            damageCalculate = enemyController.zombie.health;
            if (isHaveHealtnBar)
                HealthBar.SetActive(true);
        }



        private void Update()
        {
            if (isHaveHealtnBar)
                HealthBar.transform.position = Vector3.Lerp(HealthBar.transform.position, transform.position + directionUp, Time.deltaTime * speed);
        }

        float damageCalculate;

        public float SetDamage(float damage)
        {

            float randomHealth = Random.Range(enemyController.healthDamage - 10, enemyController.healthDamage + 10);

            randomHealth = Mathf.Max(10, randomHealth);

            damageCalculate += (-randomHealth) / 100;

            damageCalculate = Mathf.Max(0, Mathf.Min(1, damageCalculate));

            if (enemyAttack.GetPlayer() == null)
                enemyAttack.GivePlayer(GameObject.FindWithTag(PlayerTag));

            HealthBarNeed(randomHealth);

            if (damageCalculate <= 0)
                DieController();

            return damageCalculate;
        }

        private void HealthBarNeed(float val)
        {
            HealthBarSet(val);
            GivePlayerHeathBar();
        }

        private void GivePlayerHeathBar()
        {
            if (GameManager.Instance.GetHealthPlayerBar() && damageCalculate == 0)
            {

                GameObject posion = Instantiate(enemyController.HealthBarObj, transform.position, Quaternion.identity);
                posion.GetComponent<HealthBar>().SetData(enemyAttack.enemyattack.healthForPlayer, 10f);

                posion.GetComponent<Rigidbody>().AddForce(
                            enemyAttack.PlayerForwardController(transform.gameObject)
                                * (enemyAttack.enemyattack.forceSpeed / 2),
                                ForceMode.Impulse
                    );
            }
        }

        private void HealthBarSet(float damageValue)
        {
            if (!isHaveHealtnBar)
            {
                GameObject obj = Instantiate(enemyController.HealthDamageObj, transform.position, Quaternion.identity);
                obj.GetComponent<DamagePopup>().Setup(Mathf.RoundToInt(damageValue));
            }
            else if (isHaveHealtnBar)
                healthBarImage.fillAmount = damageCalculate;
        }

        public float GetHealth() => damageCalculate;

        public void DieController()
        {
            if (enemyController.zombie.zombieType == ZombieType.STRONG)
                enemyAttack.enemyattack.BulletPrefab.SetActive(false);

            if (isHaveHealtnBar)
                HealthBar.SetActive(false);

            enemyAttack.GetComponent<EnemyAnimatorController>().Die();

            GetComponent<EnemyController>().SetDeadController();

            GetComponent<EnemyMove>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            gameObject.tag = "Untagged";
        }
    }



}
public interface IHealth
{
    float SetDamage(float damage);
    float GetHealth();

}