using UnityEngine;
using System.Collections;

public class DoorBehaviour : RoomObject
{

    public enum doorState { OPEN, LOCKED, UNLOCKING};


    public DoorBehaviour TargetDoor;
    public int roomIndex;
    public GameObject minimapRoomObject;
    public doorState state = doorState.LOCKED;

    public bool locked;

    public GameObject MinimapContainer;

    Renderer rend;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        rend = GetComponent<Renderer>();
    }

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();


        MinimapContainer = GameObject.Find("Minimap");
        roomIndex = transform.parent.parent.GetComponent<RoomBehaviour>().roomClass.roomIndex;
        minimapRoomObject = MinimapContainer.transform.GetChild(roomIndex).gameObject;


        SetState(doorState.LOCKED);
        minimapRoomObject.SetActive(false);


        if (roomIndex == 0) UpdateMinimap(minimapRoomObject, minimapRoomObject);

    }

    public void SetState(doorState newState)
    {
        switch (newState)
        {
            case doorState.LOCKED:
                ToggleLock(false);
                break;

            case doorState.OPEN:
                ToggleLock(true);
                break;

            case doorState.UNLOCKING:
                rend.material.color = Color.gray;
                break;

            default:
                Debug.Log("WTF man...");
                break;
        }
    }

    public void ToggleLock(bool state, bool Secondary = false)
    {
        locked = state;

        if(!Secondary && TargetDoor != null) TargetDoor.ToggleLock(state, true);

        if (locked)
        {
            rend.material.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            rend.material.color = Color.white;
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
