using UnityEngine;
using System.Collections;

public class DoorBehaviour : BaseObject
{
    public DoorBehaviour TargetDoor;

    protected override void SecondAwake()
    {
        base.SecondAwake();

        if (TargetDoor == null) Destroy(gameObject);

    }



    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag != "Player") return;

        coll.transform.position = TargetDoor.transform.position + TargetDoor.transform.forward * 1.1f + new Vector3(0, 1, 0);

        Camera.main.GetComponent<CameraBehaviour>().targetRoom = TargetDoor.transform.parent;
    }

    void OnDrawGizmosSelected()
    {
        if (!TargetDoor) return;

        Gizmos.DrawLine(transform.position, TargetDoor.transform.position);
    }

}
