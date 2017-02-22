using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : BaseObject
{


    private Rigidbody playerRigidbody;

    public float playerSpeed = 5;

    public int currentRoom = 0;

    public bool invisible = false;
    public bool canMove = true;

    protected override void FirstAwake()
    {
        base.FirstAwake();

        PlayerProperties.Reset();

        playerRigidbody = GetComponent<Rigidbody>();

        playerSpeed = PlayerProperties.playerWalkSpeed;
    }



    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        MovePlayer();


    }

    void MovePlayer()
    {

        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");
        
        direction.z = Input.GetAxisRaw("Vertical");

        playerRigidbody.velocity = direction.normalized * playerSpeed * System.Convert.ToInt32(canMove);

        if(direction.magnitude != 0) transform.LookAt(transform.position + direction.normalized);

    }



}
