using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : BaseObject
{

    private Transform targetRoom;
    private Vector3 cameraOffset = new Vector3(0, 0, -50);
    PlayerController controller;
    Transform minimapContainer;
    Camera cam;

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        controller = FindObjectOfType<PlayerController>();
        minimapContainer = GameObject.Find("Minimap").transform;
        cam = GetComponent<Camera>();
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (controller)
        {
            targetRoom = minimapContainer.GetChild(controller.currentRoom);
        }

        LookRoom();


        if (Input.GetKey(KeyCode.Tab))
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + Input.mouseScrollDelta.y * Time.deltaTime * 100, 1f, 100);
            cameraOffset += new Vector3(Input.GetAxis("MoveMapH"), Input.GetAxis("MoveMapV"), 0);
            if (Input.GetKeyUp(KeyCode.Keypad5)) cameraOffset = new Vector3(0, 0, cameraOffset.z);
        }

    }

    void LookRoom()
    {
        if (targetRoom)
            transform.position = targetRoom.position + cameraOffset;
    }

}