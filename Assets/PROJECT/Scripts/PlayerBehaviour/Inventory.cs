using UnityEngine;
using System.Collections;

public class Inventory : BaseObject
{
    [Range(0, 8)] public int selectedItem;
    public Item[] itemsInInventory = new Item[8];

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
            print("ttt");
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


