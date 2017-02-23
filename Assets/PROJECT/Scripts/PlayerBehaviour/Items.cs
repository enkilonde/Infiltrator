using UnityEngine;
using System.Collections;
using System;

public enum ITEM_LIST // Ajoutez le nom d'un item dans cet enum quand vous le créez
{
    DebugItem,

    //Passive Items
    MushroomBoost,

    //Active items
    Dash
}; 






public class ItemsUtility
{
    public static Item GetItemFromEnum(ITEM_LIST enumElement)
    {
        return (Item)Activator.CreateInstance(Type.GetType(enumElement.ToString()));
    }

    public static Item GetRandomItem()
    {
        return (GetItemFromEnum(GetRandomEnum<ITEM_LIST>()));
    }

    public static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }

}


[System.Serializable]
public class Item
{
    public string name; 
    public Sprite sprite;

    //ActiveItem Variables
    public bool activeItem;
    public float reloadTime = 2;
    public float reloadRemaining = 0;
    public bool itemReady = true;


    //Méthodes
    public Item() { Initialise(); } // Constructor

    public virtual void Initialise() { } // Fonction appelé quand l'objet est créée.

    public virtual void UseItem(Transform playertransform) {EventHandler.Instance.StartCoroutine(ReloadItem()); } // Fonction appelé quand l'objet est utilisé.

    public virtual void UseItem() { EventHandler.Instance.StartCoroutine(ReloadItem()); } // Fonction appelé quand l'objet est utilisé.

    public virtual void PickupItem(){} // Fonction appelée quand le joueur ramasse l'objet. 

    IEnumerator ReloadItem()
    {
        reloadRemaining = reloadTime;
        itemReady = false;
        while(reloadRemaining > 0)
        {
            yield return null;
            reloadRemaining -= Time.deltaTime;
        }
        reloadRemaining = 0;
        itemReady = true;
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

#region Passive Items
class MushroomBoost : Item
{
    public override void Initialise()
    {
        base.Initialise();
        name = "Fungal Boost";
        activeItem = false;
    }

    public override void PickupItem()
    {
        base.PickupItem();
        PlayerProperties.playerWalkSpeed += 5;
    }
}

#endregion


#region Active Items

class Dash : Item
{
    public override void Initialise()
    {
        base.Initialise();
        name = "Dash";
        activeItem = true;
    }

    public override void UseItem(Transform PlayerTransform)
    {
        base.UseItem();
        PlayerTransform.GetComponent<Rigidbody>().AddForce(PlayerTransform.forward * 100, ForceMode.VelocityChange);
    }
}


#endregion