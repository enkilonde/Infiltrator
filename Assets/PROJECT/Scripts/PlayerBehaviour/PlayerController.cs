using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : BaseObject
{
    private Rigidbody playerRigidbody;

    public float playerSpeed = 5;

    float roomSize = 32;

    public Vector2 posInRoom;

    public bool[,] validpositions;

    public bool useGenerationToBlock = true;

    public int currentRoom = 0;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        if(useGenerationToBlock)
        {
            SpriteType[,] rawValidPos = FindObjectOfType<RenderRoom>().room.getRoomMatrix();
            validpositions = new bool[rawValidPos.GetLength(0), rawValidPos.GetLength(1)];
            for (int i = 0; i < rawValidPos.GetLength(0); i++)
            {
                for (int j = 0; j < rawValidPos.GetLength(1); j++)
                {
                    validpositions[i, j] = rawValidPos[i, j] != SpriteType.ROCK;
                    //Debug.Log("Case (" + i + ", " + j + ") : " + rawValidPos[i, j] + "    Case (" + i + ", " + j + ") : " + validpositions[i, j]);
                }
            }
        }



    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (useGenerationToBlock) MovePlayer2();
        else MovePlayer();

        posInRoom = WorldToRoomLocal(transform.position);  

    }

    void MovePlayer()
    {

        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");
        
        direction.z = Input.GetAxisRaw("Vertical");

        playerRigidbody.velocity = direction.normalized * playerSpeed;

        if(direction.magnitude != 0) transform.LookAt(transform.position + direction.normalized);

    }

    void MovePlayer2()
    {
        print("ttt");

        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");

        direction.z = Input.GetAxisRaw("Vertical");


        Vector3 displacement = direction.normalized * playerSpeed * Time.deltaTime;


        //Vector3 nextPosition = transform.position + displacement;
        //Vector3 nextPositionWithCapsule = nextPosition + displacement.normalized * (GetComponent<CapsuleCollider>().radius);
        //Vector2 nextTile = WorldToRoomLocal(nextPositionWithCapsule);

        if(CanMoveHere(displacement))
        transform.position += displacement;

        //Debug.DrawLine(transform.position + new Vector3(0, 5, 0), nextPositionWithCapsule + new Vector3(0, 5, 0), Color.red);

        if (direction.magnitude != 0) transform.LookAt(transform.position + direction.normalized);

    }




    Vector2 WorldToRoomLocal(Vector3 pos)
    {
        return new Vector2(Mathf.Ceil(pos.x) - roomSize/2, Mathf.Ceil(pos.z) - roomSize/2) * -1;
    }

    bool CanMoveHere(Vector3 displacement)
    {
        if (!useGenerationToBlock) return true;
        float capsuleRadius = GetComponent<CapsuleCollider>().radius;

        Vector3 nextPosition = transform.position + displacement;

        Vector3 nextPosCapsule = nextPosition + displacement.normalized * capsuleRadius;
        Vector3 nextPosCapsuleX = transform.position + new Vector3(displacement.x, 0, 0).normalized * (0 + capsuleRadius) + new Vector3(displacement.x, 0, 0);
        Vector3 nextPosCapsuleY = transform.position + new Vector3(0, 0, displacement.z).normalized * (0 + capsuleRadius) + new Vector3(0, 0, displacement.z);
        Vector2 nextileXY = WorldToRoomLocal(nextPosCapsule);
        Vector2 nextileX = WorldToRoomLocal(nextPosCapsuleX);
        Vector2 nextileY = WorldToRoomLocal(nextPosCapsuleY);

        Debug.DrawLine(transform.position + new Vector3(0, 5, 0), nextPosCapsule + new Vector3(0, 5, 0), Color.red);
        Debug.DrawLine(transform.position + new Vector3(0, 5, 0), nextPosCapsuleX + new Vector3(0, 5, 0), Color.red);
        Debug.DrawLine(transform.position + new Vector3(0, 5, 0), nextPosCapsuleY + new Vector3(0, 5, 0), Color.red);

        if (nextileXY.x > roomSize || nextileXY.x < 0 || nextileXY.y > roomSize || nextileXY.y < 0 || nextileX.x > roomSize || nextileX.x < 0 || nextileX.y > roomSize || nextileX.y < 0 || nextileY.x > roomSize || nextileY.x < 0 || nextileY.y > roomSize || nextileY.y < 0) return false;

        return (validpositions[(int)nextileXY.x, (int)nextileXY.y] && validpositions[(int)nextileX.x, (int)nextileX.y] && validpositions[(int)nextileY.x, (int)nextileY.y]);
    }

}
