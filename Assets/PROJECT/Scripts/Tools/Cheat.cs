using UnityEngine;
using System.Collections;

public class Cheat : BaseObject
{

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameObject item = Instantiate<GameObject>(Resources.Load<GameObject>("Items/GenericItem"));
            item.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0, 3);
            item.GetComponent<ItemObject>().item = ItemsUtility.GetRandomEnum<ITEM_LIST>();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            DoorBehaviour[] doors = FindObjectsOfType<DoorBehaviour>();

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].UpdateMinimap(doors[i], doors[i]);
            }
        }


    }


}
