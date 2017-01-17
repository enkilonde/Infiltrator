using UnityEngine;
using System.Collections;

public class PlayerController : BaseObject
{
    private Rigidbody playerRigidbody;

    public float playerSpeed = 5;

    public Vector2 posInRoom;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        MovePlayer();

        PlayerLookAt();
        

        posInRoom = new Vector2(Mathf.Ceil(transform.position.x) - 16, Mathf.Ceil(transform.position.z) - 16) * -1;

    }

    void MovePlayer()
    {

        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");
        
        direction.z = Input.GetAxisRaw("Vertical");

        playerRigidbody.velocity = direction.normalized * playerSpeed;

        if(direction.magnitude != 0) transform.LookAt(transform.position + direction.normalized);

    }

    void PlayerLookAt()
    {
        //var mousePos2D = Input.mousePosition;
        //mousePos2D.z = Camera.main.transform.position.y;
        //mousePos2D = Camera.main.ScreenToWorldPoint(mousePos2D);
        //transform.LookAt(new Vector3(mousePos2D.x, transform.position.y, mousePos2D.z));



    }

}
