using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PlayerNameSpace
{
    public class Health : MonoBehaviour, IHealth
    {

        private PlayerUpgrade playerUpgrade;
        [Header("Health Foreground Image")]
        public Image healthBarImage;
        public Image HealthWhiteImage;

        //[Tooltip("Player'ı gösteren kamera")]
        private GameObject Camera;

        [Range(0, 1)]
        public float health = 1;
        public GameObject HealthObj;
        public Vector3 offsetUpPlayer;
        public float speed = 10;

        public bool isShowHealth;

        private GameObject Player;
        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            healthBarImage.fillAmount = 1F;

            playerUpgrade = GetComponent<PlayerUpgrade>();

        }

        private void Update()
        {
            if (HealthObj != null && isShowHealth)
                HealthObj.transform.position = Vector3.Lerp(HealthObj.transform.position, Player.transform.position + offsetUpPlayer, Time.deltaTime * speed);
        }

        public void Close(bool val)
        {
            isShowHealth = val;
            HealthObj.SetActive(isShowHealth);
        }

        IEnumerator WhiteImage(float damage)
        {
            yield return new WaitForSeconds(2f);
            HealthWhiteImage.fillAmount -= damage;
        }

        public float SetDamage(float damage)
        {

            if (playerUpgrade.upgradeDataClass.shieldIsactive)
            {
                return healthBarImage.fillAmount;

            }
            // 100f
            health += (damage) / 100f;

            health = Mathf.Max(0, Mathf.Min(1, health));

            healthBarImage.fillAmount = health;

            StartCoroutine(WhiteImage(health));

            if (healthBarImage.fillAmount <= 0)
            {
                gameObject.tag = "Untagged";

                GameManager.Instance.UpdateGameState(GAMESTATE.DEFEAT);


                // TODO: Animator die Trigger
                Destroy(gameObject);
            }

            return healthBarImage.fillAmount;
        }

        public float GetHealth()
        {
            return healthBarImage.fillAmount;
        }
    }
}
