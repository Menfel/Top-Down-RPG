using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 1f;

    private PlayerControl playerControl;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    [SerializeField] private int attackDamage = 4;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject damagePopupPrefab;

    public Transform currentTargetEnemy { get; private set; }
    [SerializeField] private BoxCollider2D attackCollider;
    private ContactFilter2D attackFilter;

    private Coroutine chargeSoundCoroutine;

    private bool isCharging = false;
    private bool isAttacking = false;



    private bool wasMoving = false;

    void Start()
    {
        Transform spawn = GameObject.FindGameObjectWithTag("PlayerSpawn")?.transform;
        if (spawn != null)
        {
            transform.position = spawn.position;

            currentHealth = maxHealth;
        }
    }


    private void Awake()
    {
        Instance = this;
        playerControl = new PlayerControl();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        attackFilter = new ContactFilter2D();
        attackFilter.SetLayerMask(enemyLayer);
        attackFilter.useTriggers = true;

    }

    private void OnEnable()
    {
        playerControl.Enable();
        playerControl.Movement.Attack.performed += OnAttack;
        playerControl.Movement.Charge.started += OnChargeStarted;
        playerControl.Movement.Charge.canceled += OnChargeCanceled;
    }

    private void OnDisable()
    {
        playerControl.Movement.Attack.performed -= OnAttack;
        playerControl.Movement.Charge.started -= OnChargeStarted;
        playerControl.Movement.Charge.canceled -= OnChargeCanceled;
        playerControl.Disable();
    }

   
    private void OnChargeStarted(InputAction.CallbackContext context)
    {
        if (MenuController.Instance != null && MenuController.Instance.IsMenuOpen())
            return;

        bool isWalking = myAnimator.GetBool("isWalking");

        if (isWalking || isAttacking)
        {
            Debug.Log("Нельзя заряжаться во время движения или атаки.");
            return;
        }

        myAnimator.SetBool("IsCharging", true);
        isCharging = true;

        if (chargeSoundCoroutine == null)
        {
            chargeSoundCoroutine = StartCoroutine(PlayChargeSoundLoop());
        }
    }


    private void OnChargeCanceled(InputAction.CallbackContext context)
    {
        myAnimator.SetBool("IsCharging", false);
        isCharging = false;

        if (chargeSoundCoroutine != null)
        {
            StopCoroutine(chargeSoundCoroutine);
            chargeSoundCoroutine = null;
        }
    }

    private IEnumerator PlayChargeSoundLoop()
    {
        while (true)
        {
            SoundEffectManager.Play("Charge");
            yield return new WaitForSeconds(1f); 
        }
    }


    private void OnAttack(InputAction.CallbackContext context)
    {
        if (MenuController.Instance != null && MenuController.Instance.IsMenuOpen())
            return;

        bool isWalking = myAnimator.GetBool("isWalking");

        if (isCharging || isWalking)
        {
            Debug.Log("Нельзя атаковать во время движения или зарядки.");
            return;
        }

        isAttacking = true;
        myAnimator.SetTrigger("Attack");
        SoundEffectManager.Play("AttackWitch");
        PerformAttack();

            StartCoroutine(ResetAttackFlagAfterDelay(0.5f));
    }

    private IEnumerator ResetAttackFlagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }





    private void PerformAttack()
    {
        if (!ChargeBarUI.Instance.UseCharge())
        {
            Debug.Log("Нет заряда для атаки!");
            return;
        }

        Collider2D[] results = new Collider2D[10];
        int hitCount = attackCollider.Overlap(attackFilter, results);

        for (int i = 0; i < hitCount; i++)
        {
            EnemyHealth enemyHealth = results[i].GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                currentTargetEnemy = enemyHealth.transform;

                Flash flash = results[i].GetComponent<Flash>();
                if (flash != null)
                {
                    flash.PlayDamageAnimation();
                }
                break;
            }
        }

    }

    private void Update()
    {
        if (PauseController.IsGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            myAnimator.SetBool("isWalking", false);
            return;

        }
        rb.linearVelocity = movement * moveSpeed;

        PlayerInput();
        HandleAttackInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        
        movement = playerControl.Movement.Move.ReadValue<Vector2>();

        if (movement.x < 0)
        {
            mySpriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            mySpriteRenderer.flipX = false;
        }

        bool isMoving = movement.magnitude > 0.1f;

        // если раньше двигалась, а теперь нет — воспроизвести звук остановки
        if (wasMoving && !isMoving)
        {
            SoundEffectManager.Play("Footstend");
        }

        wasMoving = isMoving;

        myAnimator.SetBool("isWalking", isMoving);
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void HandleAttackInput()
    {
        if (MenuController.Instance != null && MenuController.Instance.IsMenuOpen())
            return;

        if (playerControl.Movement.Attack.triggered)
        {
            myAnimator.SetTrigger("Attack");
        }
    }


    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
        }
    }

    public float GetMovementMagnitude()
    {
        return movement.magnitude;
    }








    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

 

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took damage: " + amount + " | Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
    }

}