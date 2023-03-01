
using System;
using UnityEngine;
namespace EnemyNameSpace
{

    [System.Serializable]
    public enum ZombieType
    {
        BASIC,
        MIDDLE,
        STRONG,
        BOSS
    }


    public class EnemyController : MonoBehaviour
    {
        public Zombie zombie;
        public GameObject HealthDamageObj;
        public GameObject HealthBarObj;
        public bool GamePlay = false;
        public float damage = 0;

        public float healthDamage;

        public int deadMoneyCount;

        public AudioSource voice;


        public void voiceController(bool val)
        {
            if (val)
            {
                voice.enabled = true;
                voice.Play();
            }
            else
            {
                voice.Stop();
                voice.enabled = false;
            }
        }


        [ContextMenu("SetDeadCountController")]
        public void SetDeadController()
        {

            UIManager.Instance.SetMoneyCalculate(deadMoneyCount);
            if(zombie.zombieType == ZombieType.BASIC){
                Debug.Log("BASIC");
            }
            DeadCountManager.Instance.DeadController(zombie);
            GetComponent<EnemyAttack>().enabled = false;
            GetComponent<EnemyMove>().enabled = false;
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += UpdateGameState;

        }



        public void VoiceController(bool status)
        {
            voice.enabled = status;
        }

        private void UpdateGameState(GAMESTATE obj) => GamePlay = obj == GAMESTATE.PLAY;

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= UpdateGameState;
        }
    }

}
