using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : BaseObject
{

    private Transform targetRoom;
    private Vector3 cameraOffset = new Vector3(0, 0, -50);
    PlayerController controller;
    Transform minimapContainer;

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        controller = FindObjectOfType<PlayerController>();
        minimapContainer = GameObject.Find("Minimap").transform;
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (controller)
        {
            targetRoom = minimapContainer.GetChild(controller.currentRoom);
        }

        LookRoom();

    }

    void LookRoom()
    {
        if (targetRoom)
            transform.position = targetRoom.position + cameraOffset;
    }

}