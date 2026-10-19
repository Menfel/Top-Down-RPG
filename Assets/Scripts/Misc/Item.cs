using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //public int ID;
    //public string Name;

    //public virtual void PickUp()
    //{
    //    Sprite itemIcon = GetComponent<Image>().sprite;
    //    if (ItemPickupController.Instance != null)
    //    {
    //        ItemPickupController.Instance.ShowItemPickup(Name, itemIcon);
    //    }
    //}



    public int ID;
    public string Name;

    public virtual void Use()  // Добавляем virtual
    {
        Debug.Log($"Использован предмет: {Name}");
    }

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>()?.sprite;
        if (ItemPickupController.Instance != null)
        {
            ItemPickupController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}

