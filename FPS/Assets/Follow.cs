
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour
{
    public NavMeshAgent enemy;

    public Transform player;

    public LayerMask isGround, isPlayer;

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    public float attackTime;
    bool isAttacked;

    //State
    public float viewRange, attackRange;
    public bool playerInViewRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check if player is in view or attack range
        playerInViewRange = Physics.CheckSphere(transform.position, viewRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInViewRange && !playerInAttackRange) Patrol();
        if (playerInViewRange && !playerInAttackRange) FollowPlayer();
        if (playerInViewRange && playerInAttackRange) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            enemy.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.x + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
            walkPointSet = true; 
    }

    private void FollowPlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void Attack()
    {
        enemy.SetDestination(transform.position);

        transform.LookAt(player);

        if (!isAttacked)
        {
            isAttacked = true;
            Invoke(nameof(ResetAttack), attackTime);
        }
    }

    private void ResetAttack()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }

}
    