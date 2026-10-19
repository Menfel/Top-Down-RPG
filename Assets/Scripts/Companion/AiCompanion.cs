using UnityEngine;

public class AiCompanion : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private float offsetDistance = 1f;
    [SerializeField] private int attackDamage = 2;
    private Animator myAnimator;

    private Transform currentTarget;
    private float attackDistance = 1f;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private bool playingFootgoes = false;
    public float footgoesSpeed = 0.5f;

    [Header("Footstep Settings")]
    public float footgoesInterval = 0.4f; 
    private float nextFootstepTime = 0f;
    private bool isMoving = false;



    void Start()
    {
        Transform spawn = GameObject.FindGameObjectWithTag("CompanionSpawn")?.transform;
        if (spawn != null)
        {
            transform.position = spawn.position;
        }
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void DealDamageToEnemy()
    {
        if (currentTarget == null) return;

        EnemyHealth enemyHealth = currentTarget.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
            Debug.Log("MeowKnight нанес урон: " + currentTarget.name);
        }
    }

    private void Update()
    {
        if (PlayerController.Instance.currentTargetEnemy != null)
        {
            currentTarget = PlayerController.Instance.currentTargetEnemy;

            float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

            if (distanceToTarget > attackDistance)
            {
                Vector2 direction = (currentTarget.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, followSpeed * Time.deltaTime);

                if (direction.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);

                myAnimator.SetFloat("moveX", 1f); 
                myAnimator.SetFloat("moveY", 0f);

                isMoving = true;
            }
            else
            {
                isMoving = false;
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    TriggerRandomAttack();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            FollowPlayer();
        }
        HandleFootsteps();
    }

    private void FollowPlayer()
    {
        float playerMovement = PlayerController.Instance.GetMovementMagnitude();

        if (playerMovement > 0)
        {
            Vector2 direction = (player.position    - transform.position).normalized;
            Vector2 targetPosition = (Vector2)player.position - direction * offsetDistance;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);

            if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        myAnimator.SetFloat("moveX", playerMovement);
        myAnimator.SetFloat("moveY", playerMovement);
    }

    private void TriggerRandomAttack()
    {
        int randomAttack = Random.Range(1, 4); 
        isAttacking = true;
        myAnimator.SetTrigger($"Attack_{randomAttack}");
        Invoke(nameof(ResetAttackState), 0.5f); 
    }

    private void ResetAttackState()
    {
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }


    private void HandleFootsteps()
    {
        if (isMoving && Time.time >= nextFootstepTime)
        {
            SoundEffectManager.Play("Footgoes", true); 
            nextFootstepTime = Time.time + footgoesInterval;
        }
    }

}
