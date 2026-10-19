//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;

//public class TeleportTrigger : MonoBehaviour
//{
//    [SerializeField] private string sceneToLoad;
//    [SerializeField] private float transitionDelay = 1.2f;

//    private Animator animator;
//    private bool isTeleporting = false;

//    private void Awake()
//    {
//        animator = GetComponent<Animator>();

//        if (TeleportManager.Instance != null && TeleportManager.Instance.JustTeleported)
//        {
//            isTeleporting = true;
//            StartCoroutine(ResetTeleportCooldown());
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!isTeleporting && other.CompareTag("Player"))
//        {
//            isTeleporting = true;
//            StartCoroutine(Teleport());
//        }
//    }

//    private IEnumerator Teleport()
//    {
//        Debug.Log("Starting teleport animation...");

//        if (animator != null)
//        {
//            animator.SetTrigger("Teleport");
//        }
//        else
//        {
//            Debug.LogWarning("Animator is missing!");
//        }

//        yield return new WaitForSeconds(transitionDelay);

//        if (TeleportManager.Instance != null)
//        {
//            TeleportManager.Instance.MarkTeleport();
//        }

//        SceneManager.LoadScene(sceneToLoad);
//    }

//    private IEnumerator ResetTeleportCooldown()
//    {
//        yield return new WaitForSeconds(1f);
//        isTeleporting = false;
//    }
//}


using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float transitionDelay = 1.2f;

    private Animator animator;
    private bool isTeleporting = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (TeleportManager.Instance != null && TeleportManager.Instance.JustTeleported)
        {
            isTeleporting = true;
            StartCoroutine(ResetTeleportCooldown());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            isTeleporting = true;
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        Debug.Log("Starting teleport animation...");

        if (animator != null)
        {
            animator.SetTrigger("Teleport");
        }

        yield return new WaitForSeconds(transitionDelay);

        if (TeleportManager.Instance != null)
        {
            TeleportManager.Instance.MarkTeleport();
        }

        // Заменено: SceneManager.LoadScene  FadeToScene
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneFader не найден! Загружаем сцену напрямую.");
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private IEnumerator ResetTeleportCooldown()
    {
        yield return new WaitForSeconds(1f);
        isTeleporting = false;
    }
}
