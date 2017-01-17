using UnityEngine;
using System.Collections;

public class DoorBehaviour : RoomObject
{
    public DoorBehaviour TargetDoor;

    bool locked;

    protected override void SecondAwake()
    {
        base.SecondAwake();

        ToggleLock(true);

        if (TargetDoor == null) gameObject.SetActive(false);

    }


    public void ToggleLock(bool state, bool Secondary = false)
    {
        locked = state;

        if(!Secondary && TargetDoor != null) TargetDoor.ToggleLock(state, true);

        if (locked)
        {
            GetComponent<Renderer>().material.color = Color.black;
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

        Camera.main.GetComponent<CameraBehaviour>().targetRoom = TargetDoor.transform.parent;
    }

    void OnDrawGizmosSelected()
    {
        if (!TargetDoor) return;

        Gizmos.DrawLine(transform.position, TargetDoor.transform.position);
    }

}
