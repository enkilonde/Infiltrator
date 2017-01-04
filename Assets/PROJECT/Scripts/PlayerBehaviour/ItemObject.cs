using UnityEngine;
using System.Collections;

public class ItemObject : BaseObject
{
    public ITEM_LIST item;
    public Item actualItem;

    protected override void SecondAwake()
    {
        base.SecondAwake();
        actualItem = ItemsUtility.GetItemFromEnum(item);
    }


    void OnTriggerEnter(Collider coll)
    {

        if(coll.tag == "Player")
        {
            if (coll.GetComponent<Inventory>().PickupItem(actualItem))
            {
                Destroy(gameObject);
            }
        }

    }

}
