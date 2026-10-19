//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;

//public class DamagePopup : MonoBehaviour
//{
//    [SerializeField] private int damageAmount = 1;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.gameObject.GetComponent<EnemyAI>())
//        {
//            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
//            enemyHealth.TakeDamage(damageAmount);
//        }
//    }
//}