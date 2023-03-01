
using UnityEngine;

namespace EnemyNameSpace
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        //private const string isBlendWalkingAndIdleTag = "isWalking";
        private const string isWalkingTag = "isWalking";
        private const string isDieTag = "isDie";
        private const string isHandleTag = "isHandle";
        private const string isShootTag = "isShoot";
        private const string isIdleTag = "isIdle";
        private const string isBombTag = "isBomb";

        public bool isAttack = false;
        private Vector3 velocity;
        private Vector3 localVelocity;
        private float localAnimatedZSpeed;
        private Animator _Animator;


        private void Start()
        {
            _Animator = GetComponent<Animator>();
        }
        public void Shoot()
        {
            _Animator.SetTrigger(isShootTag);
        }

        public void Move()
        {
            _Animator.SetBool(isIdleTag, false);
            _Animator.SetBool(isWalkingTag, true);
        }


        public void Idle()
        {

            _Animator.SetBool(isWalkingTag, false);
            _Animator.SetBool(isIdleTag, true);

        }


        private void CloseWalkAndRun()
        {
            _Animator.SetBool(isWalkingTag, false);
            _Animator.SetBool(isIdleTag, false);
        }


        public void Roaring()
        {
            _Animator.SetTrigger("roaring");
        }
        public void ResetRoaring()
        {
            _Animator.ResetTrigger("roaring");
        }


        public void Handle()
        {
            CloseWalkAndRun();

            if (isAttack)
                _Animator.SetTrigger(isHandleTag);
            else
                Idle();
        }

        public void Bomb()
        {
            CloseWalkAndRun();

            if (isAttack)
                _Animator.SetTrigger(isBombTag);
            else
            {
                Idle();
                ResetHandle(isBombTag);
            }
        }


        public void ResetHandle(string name) => _Animator.ResetTrigger(name);


        // public void UpdateAnimator()
        // {
        //     velocity = GetComponent<NavMeshAgent>().velocity;
        //     localVelocity = transform.InverseTransformDirection(velocity);
        //     localAnimatedZSpeed = Mathf.Clamp(localVelocity.z, 0, 1);
        //     _Animator.SetFloat(isBlendWalkingAndIdleTag, localAnimatedZSpeed, 0.1f, Time.deltaTime);
        // }

        public void Die() => _Animator.SetTrigger(isDieTag);

    
    }

}