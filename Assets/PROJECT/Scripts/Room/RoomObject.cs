using UnityEngine;
using System.Collections;

public class RoomObject : BaseObject
{
    [HideInInspector] public RoomBehaviour roomBehaviourScript;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        roomBehaviourScript = GetComponentInParent<RoomBehaviour>();
    }

}
