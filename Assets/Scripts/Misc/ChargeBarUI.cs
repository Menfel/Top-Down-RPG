using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ChargeBarUI : MonoBehaviour
{
    public static ChargeBarUI Instance;

    public Image chargeBarImage;
    public Sprite[] chargeSprites;

    private int maxCharge = 5;
    private int currentCharge;

    private float holdTime = 0f;
    private bool isCharging = false;


    private PlayerControl controls;

    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        controls = new PlayerControl();
        controls.Movement.Charge.started += ctx => StartCharging();
        controls.Movement.Charge.canceled += ctx => StopCharging();

        animator = GetComponent<Animator>();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        currentCharge = maxCharge;
        UpdateChargeBar();
    }

    private void Update()
    {
        if (controls.Movement.Charge.ReadValue<float>() > 0 && currentCharge < maxCharge)
        {
            isCharging = true;
            holdTime += Time.deltaTime;

            if (holdTime >= 3f)
            {
                currentCharge++;
                currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
                UpdateChargeBar();

                Debug.Log("Charge Trigger Sent!");
                PlayerController.Instance?.GetComponent<Animator>()?.SetTrigger("Charge");

                holdTime = 0f; // —брос дл€ следующего восстановлени€
            }
        }
        else
        {
            isCharging = false;
            holdTime = 0f;
        }
    }

    private void StartCharging()
    {
        if (currentCharge < maxCharge)
            isCharging = true;
    }

    private void StopCharging()
    {
        isCharging = false;
        holdTime = 0f;
    }

    public bool UseCharge()
    {
        if (currentCharge > 0)
        {
            currentCharge--;
            UpdateChargeBar();
            return true;
        }
        return false;
    }

    private void UpdateChargeBar()
    {
        int spriteIndex = Mathf.Clamp(maxCharge - currentCharge, 0, chargeSprites.Length - 1);
        chargeBarImage.sprite = chargeSprites[spriteIndex];
    }

    public void AddCharge(int amount)
    {
        currentCharge = Mathf.Min(currentCharge + amount, maxCharge);
        UpdateChargeBar(); 
    }



    public int GetCurrentCharge() => currentCharge;
}