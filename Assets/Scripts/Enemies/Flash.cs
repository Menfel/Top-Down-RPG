using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private Animator animator;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    public void PlayDamageAnimation()
    {
        //animator.SetTrigger("Damage"); // активируем анимацию Damage
        //StartCoroutine(WaitAndDetectDeath());
        animator.SetTrigger("Damage");
    }

    //private IEnumerator WaitAndDetectDeath()
    //{
    //    yield return new WaitForSeconds(0.2f); 
    //    enemyHealth.DetectDeath(); 
    //}
}
