using UnityEngine;
using System.Collections;
namespace PlayerNameSpace
{


    public class PlayerUpgradeClass
    {
        //Shield properties//
        public bool shieldIsactive;
        public float shieldRemainingTime = 0;


    }

    public class PlayerUpgrade : MonoBehaviour
    {
        [SerializeField] private GameObject shieldParticle;
        private Coroutine coroutine;
        public PlayerUpgradeClass upgradeDataClass;



        private void Start()
        {
            upgradeDataClass = new PlayerUpgradeClass();


        }

        public void UpdateShield(float time)
        {
            if (coroutine != null)
            {
                StopCoroutine("EnableShield");
            }
            coroutine = StartCoroutine(EnableShield(time));

        }





        private IEnumerator EnableShield(float time)
        {
            upgradeDataClass.shieldIsactive = true;
            StartCoroutine(Timer(time));
            shieldParticle.SetActive(true);
            WaitForSeconds wait = new WaitForSeconds(time);
            yield return wait;
            shieldParticle.SetActive(false);



        }


        private IEnumerator Timer(float time)
        {
            float leftTime = time;
            while (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                upgradeDataClass.shieldRemainingTime = leftTime;
                UIManager.Instance.ShildController(true, upgradeDataClass.shieldRemainingTime);

                yield return null;
            }
                        upgradeDataClass.shieldIsactive = false;

            UIManager.Instance.ShildController(false, 0);
        }
    }
}
