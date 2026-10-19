//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Teleporter : MonoBehaviour
//{
//    [SerializeField] private string targetSceneName;
//    [SerializeField] private Vector2 targetPositionInNewScene;
//    [SerializeField] private Animator transitionAnimator;
//    [SerializeField] private float transitionTime = 1f;

//    private bool isTeleporting = false;

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (!isTeleporting && other.CompareTag("Player"))
//        {
//            isTeleporting = true;
//            StartCoroutine(Teleport());
//        }
//    }

//    private System.Collections.IEnumerator Teleport()
//    {
//        if (transitionAnimator != null)
//        {
//            transitionAnimator.SetTrigger("Start");
//        }

//        yield return new WaitForSeconds(transitionTime);
//        //PlayerSpawn.PositionInNextScene = targetPositionInNewScene;
//        SceneManager.LoadScene(targetSceneName);
//    }
//}
