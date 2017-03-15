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
    MinimapBehaviour miniMBehaviour;

    public RoomBehaviour ownRoom;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        rend = GetComponent<Renderer>();
    }

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();


        SetState(doorState.LOCKED);

        if (TargetDoor == this) return;

        MinimapContainer = GameObject.Find("Minimap");
        roomIndex = transform.parent.parent.GetComponent<RoomBehaviour>().roomClass.roomIndex;
        minimapRoomObject = MinimapContainer.transform.GetChild(roomIndex).gameObject;
        ownRoom = transform.parent.parent.GetComponent<RoomBehaviour>();
        miniMBehaviour = FindObjectOfType<MinimapBehaviour>();

        minimapRoomObject.SetActive(false);



    }

    protected override void OnLoadEndedLate()
    {
        base.OnLoadEndedLate();
        if (roomIndex == ProceduralValues.partSize - 2)
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
                state = doorState.LOCKED;
                ToggleLock(true);
                break;

            case doorState.OPEN:
                state = doorState.OPEN;
                ToggleLock(false);
                break;

            case doorState.UNLOCKING:
                state = doorState.UNLOCKING;
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
        GetComponent<Collider>().isTrigger = !state;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag != "Player") return;
        if (locked) return;

        if(TargetDoor == this)
        {
            ResetScene.ActivateReset();
            return;
        }

        coll.transform.position = TargetDoor.transform.position + TargetDoor.transform.forward * 1.1f + new Vector3(0, 1, 0);
        coll.GetComponent<PlayerController>().currentRoom = TargetDoor.roomIndex;
        UpdateMinimap(this, TargetDoor);
        Camera.main.GetComponent<CameraBehaviour>().targetRoom = TargetDoor.transform.parent;

        roomBehaviourScript.state = RoomBehaviour.RoomState.EXPLORED;
        TargetDoor.roomBehaviourScript.state = RoomBehaviour.RoomState.CURRENT;
        for (int i = 0; i < TargetDoor.roomBehaviourScript.doors.Length; i++)
        {
            if(TargetDoor.roomBehaviourScript.doors[i].TargetDoor.roomBehaviourScript.state == RoomBehaviour.RoomState.UNKNOWN)
            {
                TargetDoor.roomBehaviourScript.doors[i].TargetDoor.roomBehaviourScript.state = RoomBehaviour.RoomState.KNOWN;
            }
        }

        TargetDoor.roomBehaviourScript.ToggleEnnemysLights(true);
        roomBehaviourScript.ToggleEnnemysLights(false);

    }

    public void UpdateMinimap(DoorBehaviour oldRoom, DoorBehaviour nextRoom)
    {
        if (TargetDoor == this) return;

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
