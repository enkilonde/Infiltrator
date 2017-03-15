using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : BaseObject
{
    [Range(0, 7)] public int selectedItem;
    public List<Item> itemsInInventory = new List<Item>();

    Image displaySelectedItem;
    Image reloadIndicator;

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        //itemsInInventory[0] = new DebugItem();
        displaySelectedItem = GameObject.Find("ItemSelected").GetComponent<Image>(); // The canvas is created in 'PlayerHealth'-> SecondAwake.
        reloadIndicator = GameObject.Find("ReloadIndicator").GetComponent<Image>();
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        UpdateReloadDisplay();
    }

    public void ActivateSelectedItem()
    {
        if (selectedItem >= itemsInInventory.Count) return;
        if (!itemsInInventory[selectedItem].itemReady) return;
        itemsInInventory[selectedItem].UseItem(transform);
    }

    public bool PickupItem(Item item)
    {
        if (!item.activeItem)
        {
            item.PickupItem();
            return true;
        }

        if(itemsInInventory.Count < 8) // inventory is not full
        {
            selectedItem = itemsInInventory.Count;
            itemsInInventory.Add(item);
            item.PickupItem();
        }
        else //inventory is full -> swap current item with new up item
        {
            //TODO : Need to drop current item
            itemsInInventory[selectedItem] = item;
            item.PickupItem();
        }
        UpdateInventoryDisplay();
        return true;
    }

    void UpdateInventoryDisplay()
    {
        displaySelectedItem.sprite = itemsInInventory[selectedItem].sprite;
    }

    void UpdateReloadDisplay()
    {
        if (selectedItem >= itemsInInventory.Count) return;
        if (reloadIndicator == null) return;
        Item currentItem = itemsInInventory[selectedItem];
        reloadIndicator.fillAmount = Mathf.InverseLerp(0, currentItem.reloadTime, currentItem.reloadRemaining);

        if (currentItem.itemReady)
        {
            displaySelectedItem.color = Color.white;
        }
        else
        {
            displaySelectedItem.color = Color.gray;
        }

    }

    public void ChangeSelectedItem(int value, bool relative = true)
    {
        if (itemsInInventory.Count <= 0) return;
        if (relative)
        {
            selectedItem = (int)Mathf.Repeat(selectedItem + value, itemsInInventory.Count);
        }
        else
        {
            if(value < itemsInInventory.Count)
            {
                selectedItem = value;
            }
        }

        UpdateInventoryDisplay();

    }
}


