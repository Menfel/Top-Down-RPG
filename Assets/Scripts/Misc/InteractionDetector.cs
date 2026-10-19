//using UnityEngine;
//using UnityEngine.InputSystem;

//public class InteractionDetector : MonoBehaviour
//{

//    private PlayerControl playerControl;

//    private void Awake()
//    {
//        playerControl = new PlayerControl();
//    }

//    private void OnEnable()
//    {
//        playerControl.Enable();
//        playerControl.Movement.Interact.performed += OnInteract;
//    }

//    private void OnDisable()
//    {
//        playerControl.Movement.Interact.performed -= OnInteract;
//        playerControl.Disable();
//    }




//    private IInteractable interactableInRange = null;
//    public GameObject interactionIcon;

//    void Start()
//    {
//        interactionIcon.SetActive(false);
//    }

//    public void OnInteract (InputAction.CallbackContext context)
//    {
//        if (context.performed) 
//        {
//            interactableInRange?.Interact();
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        //if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
//        //{
//        //    interactableInRange = interactable;
//        //    interactionIcon.SetActive(true);
//        //}

//        if (collision.TryGetComponent(out IInteractable interactable))
//        {
//            if (interactable.CanInteract())
//            {
//                interactableInRange = interactable;
//                interactionIcon.SetActive(true);
//            }
//        }

//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        //if(collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
//        //{
//        //    interactableInRange = null;
//        //    interactionIcon.SetActive(false);
//        //}

//        if (collision.TryGetComponent(out IInteractable _))
//        {
//            interactableInRange = null;
//            interactionIcon.SetActive(false);
//        }
//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange;
    public GameObject interactionIcon;

    private PlayerControl playerControl;

    private void Awake()
    {
        playerControl = new PlayerControl();
    }

    private void OnEnable()
    {
        playerControl.Enable();
        playerControl.Movement.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        playerControl.Movement.Interact.performed -= OnInteract;
        playerControl.Disable();
    }

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            if (interactionIcon != null)
                interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable _))
        {
            interactableInRange = null;
            if (interactionIcon != null)
                interactionIcon.SetActive(false);
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && interactableInRange != null)
        {
            interactableInRange.Interact();
        }
    }
}
