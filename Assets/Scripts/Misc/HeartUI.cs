using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    private Image image;
    private Vector3 originalScale;
    private Coroutine animCoroutine;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalScale = transform.localScale;
    }

    public void PlayHitAnimation()
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(HitAnim());
    }

    private IEnumerator HitAnim()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float scale = 1f + Mathf.Sin(elapsed * 50f) * 0.1f;
            transform.localScale = originalScale * scale;

            // Мигаем (альфа)
            float alpha = Mathf.PingPong(elapsed * 10f, 1f);
            image.color = new Color(1f, 1f, 1f, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        image.color = Color.white;
    }
}
