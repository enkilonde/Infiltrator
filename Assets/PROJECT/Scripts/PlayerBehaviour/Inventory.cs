using UnityEngine;
using System.Collections;

public class Inventory : BaseObject
{
    [Range(0, 7)] public int selectedItem;
    public Item[] itemsInInventory;

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();

        itemsInInventory = new Item[8];
        //itemsInInventory[0] = new DebugItem();

    }

    public void ActivateSelectedItem()
    {
        itemsInInventory[selectedItem].UseItem();
    }


    public bool PickupItem(Item item)
    {
        if (!item.activeItem)
        {
            item.PickupItem();
            return true;
        }


        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if(itemsInInventory[i] == null)
            {
                itemsInInventory[i] = item;
                item.PickupItem();
                return true;
            }

        }

        return false; ;
    }

}


