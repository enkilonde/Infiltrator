using UnityEngine;
using System.Collections;

public class ButtonUnlockDoors : RoomObject
{


    void OnTriggerEnter(Collider coll)
    {
        roomBehaviourScript.UnlockDoors();
    }

}
