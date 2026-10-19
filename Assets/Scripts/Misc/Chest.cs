//using UnityEngine;

//public class Chest : MonoBehaviour, IInteractable
//{
//    public bool IsOpened { get; private set; }

//    public string ChestID { get; private set; }
//    public GameObject itemPrefab;
//    public Sprite openedSprite;

//    void Start()
//    {
//        ChestID ??= GlobalHelper.GenerateUniqueID(gameObject);
//    }

//    public bool CanInteract()
//    {
//        return !IsOpened;
//    }

//    public void Interact()
//    {
//       if(!CanInteract()) return;
//       OpenChest();
//    }

//    private void OpenChest()
//    {
//        SetOpened(true);

//        if (itemPrefab)
//        {
//            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
//            droppedItem.GetComponent<BounceEffect>().StartBounce();
//        }
//    }

//    public void SetOpened(bool opened)
//    {
//        if (IsOpened = opened)
//        {
//            GetComponent<SpriteRenderer>().sprite = openedSprite;
//        }
//    }
//}

using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private string chestID; 
    [SerializeField] private bool isOpened;
    public Sprite openedSprite;
    public GameObject itemPrefab;

    public string ChestID => chestID;         
    public bool IsOpened => isOpened;          

    private void Start()
    {
        if (string.IsNullOrEmpty(chestID))
        {
            chestID = GlobalHelper.GenerateUniqueID(gameObject);
        }
    }

    public bool CanInteract()
    {
        return !isOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    private void OpenChest()
    {
        SetOpened(true);
        SoundEffectManager.Play("Chest");

        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }

        if (openedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

    public void SetOpened(bool value)
    {
        isOpened = value;

        if (value && openedSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}
