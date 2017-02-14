using UnityEngine;
using System.Collections;

public class DoorBehaviour : RoomObject
{
    public DoorBehaviour TargetDoor;
    public int roomIndex;
    public GameObject minimapRoomObject;


    public bool locked;

    public GameObject MinimapContainer;


    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();


        MinimapContainer = GameObject.Find("Minimap");
        roomIndex = transform.parent.parent.GetComponent<RoomBehaviour>().roomClass.roomIndex;
        minimapRoomObject = MinimapContainer.transform.GetChild(roomIndex).gameObject;


        ToggleLock(true);
        minimapRoomObject.SetActive(false);


        if (roomIndex == 0) UpdateMinimap(minimapRoomObject, minimapRoomObject);

    }

    public void ToggleLock(bool state, bool Secondary = false)
    {
        locked = state;

        if(!Secondary && TargetDoor != null) TargetDoor.ToggleLock(state, true);

        if (locked)
        {
            GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }

    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag != "Player") return;
        if (locked) return;
        coll.transform.position = TargetDoor.transform.position + TargetDoor.transform.forward * 1.1f + new Vector3(0, 1, 0);
        coll.GetComponent<PlayerController>().currentRoom = TargetDoor.roomIndex;
        UpdateMinimap(minimapRoomObject, TargetDoor.minimapRoomObject);
        Camera.main.GetComponent<CameraBehaviour>().targetRoom = TargetDoor.transform.parent;
    }

    void UpdateMinimap(GameObject oldRoom, GameObject nextRoom)
    {
        oldRoom.GetComponent<Renderer>().material.color = Color.blue;
        nextRoom.SetActive(true);
        nextRoom.GetComponent<Renderer>().material.color = Color.red;
    }


    void OnDrawGizmos()
    {
        if (!TargetDoor) return;

        Gizmos.DrawLine(transform.position, TargetDoor.transform.position);
    }

}
