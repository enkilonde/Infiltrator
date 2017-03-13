using UnityEngine;
using System.Collections;

public class Cheat : BaseObject
{

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyDown(KeyCode.F1)) // Spawn random item
        {
            GameObject item = Instantiate<GameObject>(Resources.Load<GameObject>("Items/GenericItem"));
            item.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0, 3);
            item.GetComponent<ItemObject>().item = ItemsUtility.GetRandomEnum<ITEM_LIST>();
        }

        if (Input.GetKeyDown(KeyCode.F2)) // reveal map
        {
            DoorBehaviour[] doors = FindObjectsOfType<DoorBehaviour>();

            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].UpdateMinimap(doors[i], doors[i]);
            }
        }


        if (Input.GetKeyDown(KeyCode.F3)) // kill all ennemis
        {
            GameObject currentRoom = GameObject.Find("All Rooms").transform.GetChild(FindObjectOfType<PlayerController>().currentRoom).gameObject;
            foreach(Transform monster in currentRoom.transform.Find("Ennemis"))
            {
                Destroy(monster.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.F4)) // open doors
        {
            GameObject currentRoom = GameObject.Find("All Rooms").transform.GetChild(FindObjectOfType<PlayerController>().currentRoom).gameObject;
            DoorBehaviour[] doors = currentRoom.transform.Find("LD").GetComponentsInChildren<DoorBehaviour>();
            foreach(DoorBehaviour door in doors)
            {
                door.ToggleLock(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {

        }

        if (Input.GetKeyDown(KeyCode.F6))
        {

        }

    }


}
