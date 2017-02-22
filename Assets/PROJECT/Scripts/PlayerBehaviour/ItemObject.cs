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
        Sprite S = Resources.Load<Sprite>("Items/Sprites/" + actualItem.name);
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Items/Sprites/" + actualItem.name);
        Rect spriteSize = GetComponent<SpriteRenderer>().sprite.rect;

        //transform.localScale = new Vector3(25/spriteSize.width, 25/spriteSize.height, 1);

        transform.Rotate(90, 0, 0);
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
