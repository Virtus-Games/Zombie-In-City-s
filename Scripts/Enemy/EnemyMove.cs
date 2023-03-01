
using UnityEngine;
using UnityEngine.AI;


namespace EnemyNameSpace
{

    [RequireComponent(typeof(NavMeshAgent))]

    public class EnemyMove : MonoBehaviour
    {
        public float remainingDistanceRange;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float radius;
        [SerializeField] private float tolerens;
        [SerializeField] private Waypoint waypoint;
        [SerializeField] private float waypointSpeed;

        [HideInInspector]
        public NavMeshAgent Agent;
        private EnemyAnimatorController _EnemyAnimator;
        private EnemyController enemyControlleE;
        private EnemyAttack _EnemyAttack;
        private GameObject _Player;
        private int currentWaypointIndex;
        private bool _distance;
        private float _moveSpeed;

        private void Start()
        {
            _moveSpeed = moveSpeed;
            enemyControlleE = GetComponent<EnemyController>();
            _EnemyAttack = GetComponent<EnemyAttack>();
            _EnemyAnimator = GetComponent<EnemyAnimatorController>();
            Agent = GetComponent<NavMeshAgent>();
            Agent.speed = moveSpeed;
        }


        private void Update()
        {
            SearchPlayer();
            MovePlayer();
        }



        private void SearchPlayer()
        {
            if (_Player == null)
            {
                Collider[] colradius = Physics.OverlapSphere(transform.position, radius);

                foreach (var item in colradius)
                {
                    if (item.CompareTag("Player"))
                    {
                        _Player = item.gameObject;
                        Agent.isStopped = false;
                        _EnemyAttack.GivePlayer(_Player);
                        _EnemyAnimator.Move();
                        SetPosition(_Player.transform);
                        break;
                    }
                    else if (waypoint != null && _Player == null)
                    {
                        moveSpeed = waypointSpeed;

                        if (AtWaypoint(currentWaypointIndex))
                            CycleWaypoints();

                        Rotate(NextRotation, rotationSpeed);
                        _EnemyAnimator.Move();
                        Vector3 nextPosition = GetWaypoint(currentWaypointIndex);
                        Agent.SetDestination(nextPosition);
                    }
                }
            }
        }

        public bool AtWaypoint(int currentIndex)
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetWaypoint(currentIndex));
            return distanceToWaypoint <= tolerens;
        }

        private Vector3 GetWaypoint(int point) => waypoint.GetPosition(point).position;

        private Transform NextRotation => waypoint.GetPosition(currentWaypointIndex);

        private void CycleWaypoints()
        {
            currentWaypointIndex = waypoint.GetNextIndex(currentWaypointIndex);

        }

        private void MovePlayer()
        {

            if (_Player != null)
            {
                moveSpeed = _moveSpeed;

                _distance = Vector3.Distance(_Player.transform.position, transform.position) < remainingDistanceRange ? true : false;

                bool isRadiusExit = Vector3.Distance(_Player.transform.position, transform.position) > radius ? true : false;

                if (_Player != null)
                {
                    if (_distance)
                    {
                        Agent.isStopped = true;
                        Rotate(_Player.transform, rotationSpeed);
                        _EnemyAnimator.Idle();
                        _EnemyAttack.Attack();
                    }

                    if (!_distance)
                    {
                        Agent.isStopped = false;
                        _EnemyAnimator.Move();
                        SetPosition(_Player.transform);
                    }

                    if (isRadiusExit)
                    {
                        _EnemyAnimator.Idle();
                        _Player = null;
                        _EnemyAttack.GivePlayer(null);
                        SetPosition(null);
                    }
                }
            }
        }
        public void SetPosition(Transform pos)
        {
            if (pos != null)
                Agent.SetDestination(pos.position);
            else
            {
                Agent.isStopped = true;
                Agent.SetDestination(transform.position);
            }
        }

        public void Rotate(Transform pos, float rotateSpeed)
        {

            if (pos != null)
            {
                Vector3 direction = pos.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;

                if (Quaternion.Angle(transform.rotation, lookRotation) > 0.1f)
                    transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
        }

        private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, radius);
    }

}