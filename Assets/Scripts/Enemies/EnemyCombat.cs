using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private Animator animator;
    private Transform player;
    private float lastAttackTime;

    private EnemyPathfinding pathfinding;
    private KnockBack knockback;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        pathfinding = GetComponent<EnemyPathfinding>();
        knockback = GetComponent<KnockBack>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        player = PlayerController.Instance.transform;

        if (enemyHealth != null)
        {
            enemyHealth.OnDamaged += PlayTakeHit;
            enemyHealth.OnDeath += PlayDeath;
        }
    }

    private void OnDestroy()
    {
        // Отписка — важно!
        if (enemyHealth != null)
        {
            enemyHealth.OnDamaged -= PlayTakeHit;
            enemyHealth.OnDeath -= PlayDeath;
        }
    }

    private void Update()
    {
        if (player == null || knockback.gettingKnockedBack) return;

        float distance = Vector2.Distance(transform.position, player.position);

        animator.SetBool("isRunning", distance > attackRange);

        if (distance <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            //PlayerController.Instance.TakeDamage(1);
            PlayerHealth playerHealth = PlayerController.Instance.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1, transform);
            }

        }
    }

    private void PlayTakeHit()
    {
        animator.SetTrigger("TakeHit");
    }

    private void PlayDeath()
    {
        animator.SetTrigger("Death");
    }

}
