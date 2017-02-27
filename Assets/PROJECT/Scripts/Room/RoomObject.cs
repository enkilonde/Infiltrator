using UnityEngine;
using System.Collections;

public class RoomObject : BaseObject
{
     public RoomBehaviour roomBehaviourScript;


    protected override void SecondAwake()
    {
        base.SecondAwake();
        DoorBehaviour t = this as DoorBehaviour;
        if (t != null) roomBehaviourScript = transform.parent.parent.GetComponent<RoomBehaviour>();

        ButtonUnlockDoors t2 = this as ButtonUnlockDoors;
        if (t2 != null) roomBehaviourScript = transform.parent.parent.GetComponent<RoomBehaviour>();

        RoomBehaviour t3 = this as RoomBehaviour;
        if (t3 != null) roomBehaviourScript = t3;
    }
}
