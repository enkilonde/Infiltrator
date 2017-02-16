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

    public RoomBehaviour ownRoom;

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
        ownRoom = transform.parent.parent.GetComponent<RoomBehaviour>();

        SetState(doorState.LOCKED);
        minimapRoomObject.SetActive(false);




    }

    protected override void OnLoadEndedLate()
    {
        base.OnLoadEndedLate();
        if (roomIndex == 0)
        {
            minimapRoomObject.SetActive(true);
            UpdateMinimap(this, this);
        }
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
        UpdateMinimap(this, TargetDoor);
        Camera.main.GetComponent<CameraBehaviour>().targetRoom = TargetDoor.transform.parent;
    }

    void UpdateMinimap(DoorBehaviour oldRoom, DoorBehaviour nextRoom)
    {

        oldRoom.minimapRoomObject.GetComponent<Renderer>().material.color = Color.blue;
        //nextRoom.SetActive(true);
        nextRoom.minimapRoomObject.GetComponent<Renderer>().material.color = Color.red;
        for (int i = 0; i < nextRoom.ownRoom.doors.Length; i++)
        {
            if(nextRoom.ownRoom.doors[i].TargetDoor != oldRoom && !nextRoom.ownRoom.doors[i].TargetDoor.minimapRoomObject.activeSelf)
            {
                nextRoom.ownRoom.doors[i].TargetDoor.minimapRoomObject.SetActive(true);
                nextRoom.ownRoom.doors[i].TargetDoor.minimapRoomObject.GetComponent<Renderer>().material.color = Color.gray;
            }
        }
    }


    void OnDrawGizmos()
    {
        if (!TargetDoor) return;

        Gizmos.DrawLine(transform.position, TargetDoor.transform.position);
    }

}
