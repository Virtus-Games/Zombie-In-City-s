using System.Collections;
using UnityEngine;
using UnityEditor;
using EnemyNameSpace;

namespace PlayerNameSpace
{

    public class FieldOfView : MonoBehaviour
    {

        #region SERIALEZ FILEDS

        [SerializeField] float radius;
        [SerializeField] LayerMask enemy;

        #endregion

        #region  PRIVATE FIELDS
        private bool isDetectedEnemy;

        private Transform gunTransform;

        private Shooting shooting;

        private PlayerAnimationControl animationControl;

        private PlayerController pc;

        private float posZ;

        private Weapon weapon;

        private InputManager IM;

        private bool isEnemyMultiple = false;

        private GunOffsetSettings gunOffsetSettings;



        #endregion

        #region PROPERTIES
        public float Radius { get { return radius; } set { radius = value; } }
        public bool IsDetectedEnemy { get { return isDetectedEnemy; } private set { } }
        public bool IsEnemyCount { get { return isEnemyMultiple; } private set {; } }

        #endregion

        private void Start()
        {
            shooting = GetComponent<Shooting>();
            pc = GetComponent<PlayerController>();
            animationControl = GetComponent<PlayerAnimationControl>();
            gunOffsetSettings = GetComponentInChildren<GunOffsetSettings>();
            weapon = gunOffsetSettings.Weapons[1].GetComponent<Weapon>();
            IM = FindObjectOfType<InputManager>();
            StartCoroutine(CheckViewArea());

        }



        private void RayToEnemy(Collider[] rangeCheck)
        {
            for (int i = 0; i < rangeCheck.Length; i++)
            {
                Debug.DrawLine(weapon.transform.position, rangeCheck[i].transform.position + new Vector3(0, 3f, 0), Color.magenta);
            }
        }

        private IEnumerator CheckViewArea()
        {
            while (true)
            {
                Collider[] rangeCheck = Physics.OverlapSphere(transform.position, radius, enemy);

                if (rangeCheck.Length > 3)
                {
                    isEnemyMultiple = true;
                    AdmonController.Instance.SetRequest();

                }
                else
                {

                    isEnemyMultiple = false;
                    AdmonController.Instance.OpenUpgradeGunBazukaOrGradele(false);
                }


                if (rangeCheck.Length == 1)
                {
                    shooting.enemyState = EnemyState.single;
                    isDetectedEnemy = true;
                    Vector3 targetPos = rangeCheck[0].transform.position;
                    pc.RotatePlayerEnemy(targetPos);

                    shooting.Shoot(rangeCheck);
                }

                else if (rangeCheck.Length > 1)
                {
                    shooting.enemyState = EnemyState.multiple;

                    pc.RotatePlayerEnemy(NearestTarget(rangeCheck));

                    isDetectedEnemy = true;

                    shooting.Shoot(rangeCheck);

                }
                else
                {
                    isDetectedEnemy = false;
                    shooting.enemyState = EnemyState.none;
                }


                float angleBetweenDirAndForward = Vector3.Angle(transform.forward, IM.Direction);

                if (angleBetweenDirAndForward > 100)
                {
                    animationControl.SetAnimSpeed("shootSpeed", -1);
                }
                else
                {
                    animationControl.SetAnimSpeed("shootSpeed", 1);

                }

                RayToEnemy(rangeCheck);

                yield return null;
            }

        }


        private Vector3 NearestTarget(Collider[] rangeCheck)
        {
            Vector3 nearestTarget = Vector3.zero;
            float currentDistance = 0;
            float nextDistance = 0;
            foreach (var item in rangeCheck)
            {
                currentDistance = Vector3.Distance(transform.position, item.transform.position);
                if (currentDistance < nextDistance)
                {
                    nearestTarget = item.transform.position;
                }
                nextDistance = currentDistance;
            }
            return nearestTarget;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (isDetectedEnemy)
            {
                Handles.color = Color.red;
            }
            else
            {
                Handles.color = Color.green;
            }
            Handles.DrawWireDisc(transform.position, Vector3.up, radius);
        }
#endif
    }


}

