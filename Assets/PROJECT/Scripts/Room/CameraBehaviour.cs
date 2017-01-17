using UnityEngine;
using System.Collections;

public class CameraBehaviour : BaseObject
{
    public Transform targetRoom;
    public Vector3 cameraOffset;



    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        LookRoom();

    }

    void LookRoom()
    {
        transform.position = targetRoom.position + cameraOffset;
    }

}
