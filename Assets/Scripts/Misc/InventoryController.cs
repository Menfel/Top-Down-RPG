using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefab;

    [System.Obsolete]
    void Start()
    {
        if (inventoryPanel == null)
        {
            inventoryPanel = GameObject.Find("InventoryPanel");
        }

        itemDictionary = Object.FindFirstObjectByType<ItemDictionary>();

        //for (int i = 0; i < slotCount; i++)
        //{
        //    Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();

        //    if (slot == null)
        //    {
        //        Debug.LogError(" Slot component is missing on the slotPrefab!");
        //        continue;
        //    }

        //    if (i < itemPrefab.Length)
        //    {
        //        if (itemPrefab[i] == null)
        //        {
        //            Debug.LogError($"itemPrefab[{i}] is null!");
        //            continue;
        //        }

        //        Debug.Log("Spawning item prefab: " + itemPrefab[i].name);

        //        GameObject item = Instantiate(itemPrefab[i], slot.transform);


        //        Debug.Log($"Item '{item.name}' parent after instantiation: {item.transform.parent.name}");

        //        RectTransform rt = item.GetComponent<RectTransform>();
        //        if (rt != null)
        //        {
        //            rt.anchoredPosition = Vector2.zero;
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"Item '{item.name}' has no RectTransform.");
        //        }

        //        slot.currentItem = item;
        //    }
        //}


        Debug.Log(" Slot count in inventoryPanel: " + inventoryPanel.transform.childCount);
    }

    //public bool AddItem(GameObject itemPrefab)
    //{
    //    foreach (Transform slotTranform in inventoryPanel.transform)
    //    {
    //        Slot slot = slotTranform.GetComponent<Slot>();
    //        if (slot != null && slot.currentItem == null)
    //        {
    //            GameObject newItem = Instantiate(itemPrefab, slotTranform);
    //            newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    //            slot.currentItem = newItem;
    //            return true;
    //        }
    //    }
    //    Debug.Log("Inventory is full!!!");
    //    return false;
    //}


    public bool AddItem(GameObject sceneItemObject)
    {


        Item item = sceneItemObject.GetComponent<Item>();
        if (item == null)
        {
            Debug.LogWarning("Нет компонента Item у поднимаемого объекта.");
            return false;
        }

        GameObject itemPrefab = itemDictionary.GetItemPrefab(item.ID);
        if (itemPrefab == null)
        {
            Debug.LogWarning($"Не найден префаб для ID: {item.ID}");
            return false;
        }

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newItem.transform.localScale = Vector3.one;
                slot.currentItem = newItem;
                return true;
            }
        }

        Debug.Log("Inventory is full!!!");
        return false;
    }



    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTranform in inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTranform.GetSiblingIndex() });
            }
        }
        return invData;
    }

    //public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    //{
    //    if (inventorySaveData == null)
    //    {
    //        Debug.LogError("inventorySaveData is NULL! Возможно, что-то не так с загрузкой сейва.");
    //        return;
    //    }

    //    foreach (Transform child in inventoryPanel.transform)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    for (int i = 0; i < slotCount; i++)
    //    {
    //        Instantiate(slotPrefab, inventoryPanel.transform);
    //    }

    //    //foreach(InventorySaveData data in inventorySaveData)
    //    //{
    //    //    if(data.slotIndex < slotCount)
    //    //    {
    //    //        Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
    //    //        GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
    //    //        if(itemPrefab != null)
    //    //        {
    //    //            GameObject item = Instantiate(itemPrefab, slot.transform);
    //    //            item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    //    //            slot.currentItem = item;
    //    //        }
    //    //    }
    //    //}

    //    foreach (InventorySaveData data in inventorySaveData)
    //    {

    //        if (data == null)
    //        {
    //            Debug.LogWarning("Один из элементов inventorySaveData оказался null!");
    //            continue;
    //        }

    //        if (data.slotIndex < slotCount)
    //        {
    //            Transform slotTransform = inventoryPanel.transform.GetChild(data.slotIndex);
    //            Slot slot = slotTransform.GetComponent<Slot>();
    //            if (slot == null)
    //            {
    //                Debug.LogWarning($"Slot component missing at index {data.slotIndex}");
    //                continue;
    //            }

    //                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
    //            if (itemPrefab == null)
    //            {
    //                Debug.LogWarning($"Item prefab not found for ID: {data.itemID}");
    //                continue;
    //            }

    //            GameObject item = Instantiate(itemPrefab, slot.transform);

    //            RectTransform rt = item.GetComponent<RectTransform>();
    //            if (rt != null)
    //            {
    //                rt.anchoredPosition = Vector2.zero;
    //            }
    //            else
    //            {
    //                Debug.LogWarning($"RectTransform missing on item {item.name}");
    //            }
    //            item.transform.localScale = Vector3.one;

    //            slot.currentItem = item;
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Invalid slot index: {data.slotIndex}");
    //        }
    //    }

    //}




    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("Inventory Panel is null — возможно сцена изменилась и ссылка устарела.");
            return;
        }

        if (inventorySaveData == null)
        {
            Debug.LogError("inventorySaveData is NULL! Возможно, что-то не так с загрузкой сейва.");
            return;
        }

        // Очистка старых слотов
        foreach (Transform child in inventoryPanel.transform)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        // Пересоздание слотов
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data == null)
            {
                Debug.LogWarning("Один из элементов inventorySaveData оказался null!");
                continue;
            }

            if (data.slotIndex < slotCount && data.slotIndex < inventoryPanel.transform.childCount)
            {
                Transform slotTransform = inventoryPanel.transform.GetChild(data.slotIndex);

                if (slotTransform == null)
                {
                    Debug.LogWarning($"Slot Transform is null at index {data.slotIndex}");
                    continue;
                }

                Slot slot = slotTransform.GetComponent<Slot>();
                if (slot == null)
                {
                    Debug.LogWarning($"Slot component missing at index {data.slotIndex}");
                    continue;
                }

                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab == null)
                {
                    Debug.LogWarning($"Item prefab not found for ID: {data.itemID}");
                    continue;
                }

                GameObject item = Instantiate(itemPrefab, slot.transform);

                if (item != null)
                {
                    RectTransform rt = item.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        rt.anchoredPosition = Vector2.zero;
                    }
                    item.transform.localScale = Vector3.one;
                    slot.currentItem = item;
                }
                else
                {
                    Debug.LogWarning($"Item instantiation failed for ID: {data.itemID}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid slot index: {data.slotIndex}");
            }
        }
    }

}
