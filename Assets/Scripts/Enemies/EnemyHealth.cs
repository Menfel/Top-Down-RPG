//using UnityEngine;

//public class EnemyHealth : MonoBehaviour
//{
//    [SerializeField] private int startingHealth = 3;
//    [SerializeField] private GameObject deathVFXPrefab;

//    private int currentHealth;
//    private KnockBack knockback;

//    private void Awake()
//    {
//        knockback = GetComponent<KnockBack>();
//    }

//    private void Start()
//    {
//        currentHealth = startingHealth;
//    }

//    public void TakeDamage(int damage)
//    {
//        currentHealth -= damage;
//        Debug.Log(currentHealth);

//        knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);

//        Flash flash = GetComponent<Flash>();
//        if (flash != null)
//        {
//            flash.PlayDamageAnimation();
//        }
//        DetectDeath();

//        //currentHealth -= damage;
//        //Debug.Log(currentHealth);
//        //knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);
//        //DetectDeath();
//    }


//    public void DetectDeath()
//    {
//        if(currentHealth<= 0)
//        {
//            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);  
//            Destroy(gameObject);
//        }
//    }
//}



using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;

    public event Action OnDamaged;
    public event Action OnDeath;

    private int currentHealth;
    private KnockBack knockback;

    private void Awake()
    {
        knockback = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        knockback.GetKnockedBack(PlayerController.Instance.transform, 15f);

        Flash flash = GetComponent<Flash>();
        if (flash != null)
        {
            flash.PlayDamageAnimation();
        }

        OnDamaged?.Invoke(); // вызываем событие урона

        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke(); // вызываем событие смерти

            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
