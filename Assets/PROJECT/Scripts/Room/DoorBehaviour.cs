using UnityEngine;
using System.Collections;

public class DoorBehaviour : RoomObject
{
    public DoorBehaviour TargetDoor;

    public bool locked;

    protected override void SecondAwake()
    {
        base.SecondAwake();

        ToggleLock(true);

        //if (TargetDoor == null) gameObject.SetActive(false);

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

        Camera.main.GetComponent<CameraBehaviour>().targetRoom = TargetDoor.transform.parent;
    }

    void OnDrawGizmos()
    {
        if (!TargetDoor) return;

        Gizmos.DrawLine(transform.position, TargetDoor.transform.position);
    }

}
