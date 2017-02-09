using UnityEngine;
using System.Collections;
using System;

public enum ITEM_LIST {DebugItem };

public class ItemsUtility
{
    public static Item GetItemFromEnum(ITEM_LIST enumElement)
    {
        return (Item)Activator.CreateInstance(Type.GetType("DebugItem"));
    }


}


[System.Serializable]
public class Item
{
    public string name;
    public Texture2D apparence;
    public bool activeItem;

    public Item() { Initialise(); } // Constructor

    public virtual void Initialise() { } // Fonction appelé quand l'objet est créée.

    public virtual void UseItem() { } // Fonction appelé quand l'objet est utilisé.

    public virtual void PickupItem() // Fonction appelée quand le joueur ramasse l'objet.
    {
        if (!activeItem) UseItem();
    }
}

class DebugItem : Item
{
    
    public override void UseItem()
    {
        base.UseItem();
        Debug.Log("Item used");
    }

    public override void Initialise()
    {
        base.Initialise();
        //Debug.Log("Initialise Item");
        name = "DebugItem";
        activeItem = true;
    }

    public override void PickupItem()
    {
        base.PickupItem();
        Debug.Log("Item Picked up");
    }

}

