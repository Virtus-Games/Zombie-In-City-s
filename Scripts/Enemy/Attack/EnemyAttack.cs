using System;
using UnityEngine;

namespace EnemyNameSpace
{
    [System.Serializable]
    public struct Enemyattack
    {
        public GameObject BulletPrefab;
        public Transform Point;
        [Range(0, 40)]
        public float damage;
        public float timeDistance;
        public float forceSpeed;
        public float timeUpdater;
        public float power;
        public float timeBomb;
        public float healthForPlayer;

    }
    interface IAttack
    {
        void SetDamage(float damage);
    }

    public class EnemyAttack : MonoBehaviour
    {

        #region Value Settings
        private GameObject _Player;
        private EnemyMove _EnemyMove;
        private EnemyAnimatorController _EnemyAnimator;
        private EnemyController _EnemyController;
        public Action<bool, float> OnAttackHandle;
        public Enemyattack enemyattack;
        private bool isPlayerHave = true;

        #endregion
        void Start()
        {
            _EnemyMove = GetComponent<EnemyMove>();
            _EnemyController = GetComponent<EnemyController>();
            _EnemyAnimator = GetComponent<EnemyAnimatorController>();
        }
        public void Attack()
        {
            // && _EnemyController.GamePlay
            if (_Player != null && isPlayerHave)
                EnemyAttackStart();
        }
        private void EnemyAttackStart()
        {
            if (enemyattack.timeDistance < Time.time)
            {
                _EnemyAnimator.isAttack = true;

                GetPlayer();

                switch (_EnemyController.zombie.zombieType)
                {
                    case ZombieType.BASIC:
                        // OnTriggerEnter  Basic Script                        
                        break;
                    case ZombieType.MIDDLE:
                        _EnemyAnimator.Bomb();
                        break;
                    case ZombieType.STRONG:
                        Particle();
                        break;
                    case ZombieType.BOSS:
                        _EnemyAnimator.Handle();
                        break;
                }

                _EnemyAnimator.isAttack = false;
                enemyattack.timeDistance = enemyattack.timeUpdater + Time.time;
            }
            else
                _EnemyAnimator.Idle();
        }

        #region  Attack Controller
        public void HandleEvent()
        {
            OnAttackHandle?.Invoke(false, enemyattack.damage);

        }

        public void Shoot()
        {
            GameObject obj = Instantiate(enemyattack.BulletPrefab, enemyattack.Point.position, Quaternion.identity);
            obj.transform.SetParent(null);
            Vector3 direction = _Player.gameObject.transform.position - obj.transform.position;
            direction.Normalize();
            obj.GetComponent<EnemyBullet>().SetData(obj.transform.forward, enemyattack.forceSpeed, enemyattack.damage);
        }

        /*
        public GameObject InstantAndDirection()
        {
            GameObject obj = Instantiate(enemyattack.BulletPrefab, enemyattack.Point.position, Quaternion.identity);
            Vector3 direction = _Player.gameObject.transform.position - obj.transform.position;
            direction.Normalize();
            return obj;
        }
        */

        public void Particle()
        {
           
                enemyattack.BulletPrefab.gameObject.SetActive(true);
                enemyattack.BulletPrefab.GetComponent<ParticulGirly>().GivePlayer(GetComponent<EnemyHealth>(), enemyattack.damage);
        }


        public void BombaEvent()
        {
            GameObject obj = Instantiate(enemyattack.BulletPrefab, enemyattack.Point.position, Quaternion.identity);
            Vector3 direction = _Player.gameObject.transform.position - obj.transform.position;
            direction.Normalize();
            obj.GetComponent<Bomba>().SetData(this, enemyattack.power, transform.position, enemyattack.forceSpeed, 5f);
            obj.GetComponent<Bomba>().AddForce(direction * enemyattack.forceSpeed);
        }

        #endregion

        #region  Player Controller
        public Vector3 PlayerForwardController(GameObject obj)
        {
            Vector3 direction = _Player.transform.position - obj.transform.position;
            direction.Normalize();
            return direction;
        }


        float PlayerHealth;
        public float PlayerSetDamage(float damage)
        {

            GetPlayer();

            if (_Player != null)
            {
                PlayerHealth = _Player.GetComponent<IHealth>().SetDamage(-damage);
                if (PlayerHealth == 0)
                {
                    GameManager.Instance.UpdatePlayerStatus(false);
                    GivePlayer(null);
                    return 0f;
                }
                else
                    return PlayerHealth;
            }
            else
                return 0f;

        }


        public void GivePlayer(GameObject player)
        {
            _Player = player;
        }

        public GameObject GetPlayer()
        {

            return _Player;
        }

        #endregion

        #region  Uptating State
        private void PlayerHaveInGame(bool arg1)
        {

            isPlayerHave = arg1;
        }

        private void OnEnable()
        {
            GameManager.OnPlayerHaveInGame += PlayerHaveInGame;
        }

        private void OnDisable()
        {
            GameManager.OnPlayerHaveInGame -= PlayerHaveInGame;
        }


        #endregion

    }
}

