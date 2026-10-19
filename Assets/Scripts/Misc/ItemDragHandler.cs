using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        transform.SetParent(canvas.transform);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out pos
        );

        transform.localPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSlot == null) 
        { 
            GameObject dropItem = eventData.pointerEnter;

            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                dropSlot.currentItem.transform.localScale = Vector3.one;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
            transform.localScale = Vector3.one;
        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                DropItem(originalSlot);
            }
            else
            {
                transform.SetParent(originalParent);
            }

            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    //void DropItem(Slot originalSlot)
    //{
    //    originalSlot.currentItem = null;

    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    if (player == null)
    //    {
    //        Debug.LogError("Missing 'Player' tag");
    //        return;
    //    }

    //    Transform playerTransform = player.transform;

    //    Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
    //    Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

    //    Instantiate(gameObject, dropPosition, Quaternion.identity);
    //    Destroy(gameObject);
    //}

    void DropItem(Slot originalSlot)
    {
        originalSlot.currentItem = null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }

        Transform playerTransform = player.transform;

        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        dropItem.GetComponent<BounceEffect>().StartBounce();

        Destroy(gameObject); 
    }

}

