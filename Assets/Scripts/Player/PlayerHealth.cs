using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;

    public Image[] hearts; // В инспекторе добавь сюда 4 Image компонента
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private Animator animator;
    private KnockBack knockback;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int damage, Transform damageSource)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Анимация и отбрасывание
        animator.SetTrigger("Damage");
        if (knockback != null)
            knockback.GetKnockedBack(damageSource, 10f);

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //void UpdateHearts()
    //{
    //    int health = currentHealth;

    //    for (int i = 0; i < hearts.Length; i++)
    //    {
    //        if (health >= 5)
    //        {
    //            hearts[i].sprite = fullHeart;
    //            health -= 5;
    //        }
    //        else if (health >= 3) // половинка если 3-4 HP
    //        {
    //            hearts[i].sprite = halfHeart;
    //            health = 0;
    //        }
    //        else
    //        {
    //            hearts[i].sprite = emptyHeart;
    //        }
    //    }
    //}

    void UpdateHearts()
    {
        int health = currentHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            HeartUI heartAnim = hearts[i].GetComponent<HeartUI>();

            if (health >= 5)
            {
                hearts[i].sprite = fullHeart;
                health -= 5;
            }
            else if (health >= 3)
            {
                hearts[i].sprite = halfHeart;
                health = 0;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (heartAnim != null)
            {
                heartAnim.PlayHitAnimation(); // проигрываем "тряску"
            }
        }
    }

    //void Die()
    //{
    //    animator.SetTrigger("Death");
    //    SoundEffectManager.Play("Death");
    //    Debug.Log("Player died!");
    //}

    private void Die()
    {
        animator.SetTrigger("Death");
        SoundEffectManager.Play("Death");
        Debug.Log("Player died!");
        StartCoroutine(LoadMainMenuAfterDelay(2f));
    }

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainPanel");
    }

    public int GetCurrentHealth() => currentHealth;
}
