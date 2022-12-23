using UnityEngine;
using UnityEngine.AI;

public class EnemyLvl2 : MonoBehaviour
{
    public int damage = 10;
    public GameObject impactEffect;
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 4;
    public float speedRun = 6;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask whatIsPlayer;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimetoRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    public GameObject Bullet;

    //shot timing
    private float timerShots;
    public float timeBtwShots = 30f;
    public float fireRadius = 25f;
    public float Force = 1000f;

    private void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimetoRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    private void Update()
    {
        EnvironmentView();

        if (!m_IsPatrol)
        {
            Chasing();
        }

        else
        {
            Patroling();
        }
        
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            float distance = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position);
            
            if (distance <= fireRadius && Time.time > timeBtwShots)
            {
                AI_FireBullet();
            }
        }
    }

    void AI_FireBullet()
    {
        RaycastHit hitPlayer;
        Ray playerPos = new Ray(transform.position, transform.forward);

        if(Physics.SphereCast(playerPos, 0.25f, out hitPlayer, fireRadius)) // Check if the 0.25f radius is too big, can make it smaller if Ai shoot through wall
        {
            PlayerHealth target = hitPlayer.transform.GetComponent<PlayerHealth>();

            if (timerShots <= 0 && hitPlayer.transform.tag == "Player")
            {
                GameObject BulletHolder;
                BulletHolder = Instantiate(Bullet, transform.position, transform.rotation) as GameObject; // Use Objectpooling instead, Instantitate is slow

                BulletHolder.transform.Rotate(Vector3.left * 90); // sometimes needed, sometimes not. Depends on how you make the bullet

                Rigidbody Temp_RigidBody;
                Temp_RigidBody = BulletHolder.GetComponent<Rigidbody>();

                Temp_RigidBody.AddForce(transform.forward * Force);

                Destroy(BulletHolder);

                timerShots = timeBtwShots;
                Debug.Log(timerShots);

                target.TakeDamage(damage);
                Instantiate(impactEffect, hitPlayer.point, Quaternion.LookRotation(hitPlayer.normal));

            }

            else
            {
                timerShots -= Time.deltaTime;
            }
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimetoRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }

            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimetoRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }

            else
            {
                Stop();
                m_TimetoRotate -= Time.deltaTime;
            }
        }

        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }

                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }

        }
    }

    private void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimetoRotate = timeToRotate;
            }

            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, whatIsPlayer);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }

                else
                {
                    m_PlayerInRange = false;
                }
            }

            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }

            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}
