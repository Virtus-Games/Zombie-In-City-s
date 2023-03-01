using UnityEngine;
namespace PlayerNameSpace
{

    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Shooting))]
    [RequireComponent(typeof(FieldOfView))]
    [RequireComponent(typeof(PlayerUpgrade))]
    [RequireComponent(typeof(PlayerAnimationControl))]
    public class PlayerController : MonoBehaviour
    {
        //COMPONENT REFERENCES
        private CharacterController cc;

        //FIELD REFERENCES
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;

        //SCRIPT REFERENCES
        private InputManager IM;
        private FieldOfView fov;
        private Health health;
        private PlayerUpgrade playerUpgrade;
        private GunOffsetSettings gunOffsetSettings;
        private WeaponUpgrade weaponUpgrade;

        //PRIVATE FIELDS
        private float speedMag;

        //PUBLIC FILEDS
        public GameObject checkSphere;


        public void OnTransate(bool isStatus)
        {
            Start();
            
            if (isStatus)
            {
                GetComponent<FieldOfView>().enabled = false;
                GetComponent<PlayerAnimationControl>().Idle();
                IM.enabled = false;
                cc.enabled = false;
                fov.enabled = false;
            }
            else
            {
                GetComponent<FieldOfView>().enabled = true;
                IM.enabled = true;
                cc.enabled = true;
                fov.enabled = true;
            }
        }


        #region PROPERTIES

        public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
        public float SpeedMag { get { return speedMag; } private set { } }
        #endregion
        private void Start()
        {
            //health = GetComponent<Health>();
            cc = GetComponent<CharacterController>();
            IM = FindObjectOfType<InputManager>();
            fov = GetComponent<FieldOfView>();
            playerUpgrade = GetComponent<PlayerUpgrade>();
            GetComponent<PlayerAnimationControl>().Idle();
            gunOffsetSettings = FindObjectOfType<GunOffsetSettings>();
            weaponUpgrade = GetComponent<WeaponUpgrade>();
        }

        private void Update()
        {
            if (GameManager.Instance.isPlay)
            {
                cc.Move((IM.Direction * moveSpeed + ApplyGravity()) * Time.deltaTime);

                if (!fov.IsDetectedEnemy)
                    transform.forward = Vector3.Slerp(transform.forward, IM.Direction, rotateSpeed * Time.deltaTime);

                CalculatePlayerSpeed();
            }

        }

        public void Upgrade(UpgradeSO upgrade)
        {
            switch (upgrade.upgradedItem)
            {
                case UpgradeType.Kalkan:

                    playerUpgrade.UpdateShield(10);

                    break;

                case UpgradeType.Hizli_atis:

                    weaponUpgrade.UpdateShootingFrequency(8, .2f, gunOffsetSettings.activeWeapon.GetComponent<Weapon>().settingsSO.shootingFrequency);

                    break;

                case UpgradeType.Explosion:

                    weaponUpgrade.OpenRpg7();

                    break;

                case UpgradeType.İkiliAtis:
                    weaponUpgrade.UpdateDoubleShoot(8, true, weaponUpgrade.doubleShoot);

                    break;
            }
        }

        private void CalculatePlayerSpeed()
        {
            Vector3 speed = cc.velocity;


            speedMag = speed.magnitude;

            bool isMove = speedMag != 0 ? true : false;

            AnimControl(isMove);
        }

        private Vector3 ApplyGravity()
        {

            bool isGround = Physics.CheckSphere(checkSphere.transform.localPosition, 0.4f, ~gameObject.layer);
            if (!isGround)
            {
                Vector3 gravityVector = new Vector3(0, -9f, 0);
                return gravityVector;
            }
            return Vector3.zero;

        }
        private void AnimControl(bool isMove)
        {
            if (isMove && fov.IsDetectedEnemy)
            {
                PlayerAnimationControl.movementState = MovementState.walking_shoot;

            }

            else if (isMove && !fov.IsDetectedEnemy)
            {
                PlayerAnimationControl.movementState = MovementState.walk;
            }

            else if (!isMove && !fov.IsDetectedEnemy)
            {
                PlayerAnimationControl.movementState = MovementState.idle;
            }

            else if (!isMove && fov.IsDetectedEnemy)
            {
                PlayerAnimationControl.movementState = MovementState.shoot;
            }
        }

        public void RotatePlayerEnemy(Vector3 targetPos)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 20).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);


        }

    }





}