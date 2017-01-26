using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class RenderRoom : BaseObject {
    
    //OUAI, c'est moche, mais c'est plus pratique pour l'inspector
    public bool doorTop = true;
    public bool doorBottom = true;
    public bool doorLeft = true;
    public bool doorRight = true;

    //public bool[] doorsBool = new bool[4];
    public int roomLenght = 32;

    public Room room;
    public Material mat;

    protected override void FirstAwake()
    {

        init();

        ApplyRender();

    }

    void init()
    {
        GameObject doorObject = Resources.Load("Door") as GameObject;

        int roomSize = roomLenght/2;
        float pas = ProceduralValues.unitValue/2;
        //pas = 0;
        List<Vector2> roomList = new List<Vector2>();
        if (doorTop)
        {
            roomList.Add(new Vector2(Random.Range(0, roomLenght), 0));
            //GameObject doorUp = Instantiate(doorObject, transform.parent.position + new Vector3(roomSize - pas - roomList.Last().x, 0, roomSize - pas - roomList.Last().y), Quaternion.identity) as GameObject;
            //doorUp.transform.SetParent(transform.parent);
            //doorUp.name = "doorTop";
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorTop");
        }
        if (doorBottom)
        {
            roomList.Add(new Vector2(Random.Range(0, roomLenght), roomLenght-1));
            //GameObject doorUp = Instantiate(doorObject, transform.parent.position + new Vector3(roomSize + pas - roomList.Last().x, 0, roomSize + pas - roomList.Last().y), Quaternion.identity) as GameObject;
            //doorUp.transform.SetParent(transform.parent);
            //doorUp.name = "doorBottom";
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorBottom");
        }
        if (doorLeft)
        {
            roomList.Add(new Vector2(roomLenght-1, Random.Range(0, roomLenght)));
            //GameObject doorUp = Instantiate(doorObject, transform.parent.position + new Vector3(roomSize + pas - roomList.Last().x, 0, roomSize + pas - roomList.Last().y), Quaternion.identity) as GameObject;
            //doorUp.transform.SetParent(transform.parent);
            //doorUp.name = "doorLeft";
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorLeft");
        }
        if (doorRight)
        {
            roomList.Add(new Vector2(0, Random.Range(0, roomLenght)));
            //GameObject doorUp = Instantiate(doorObject, transform.parent.position + new Vector3(roomSize - pas - roomList.Last().x, 0, roomSize - pas - roomList.Last().y), Quaternion.identity) as GameObject;
            //doorUp.transform.SetParent(transform.parent);
            //doorUp.name = "doorRight";
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorRight");
        }

        Vector2[] doors = roomList.ToArray();

        room = new Room(ref doors, RoomType.NORMAL);
    }

    void InstentiateDoor(GameObject doorObject, Vector2 pos, float posAdjust, float roomSize, string name)
    {
        GameObject doorUp = Instantiate(doorObject, transform.parent.position + new Vector3(roomSize + posAdjust - pos.x, ProceduralValues.MeshsHeight, roomSize + posAdjust - pos.y), Quaternion.identity) as GameObject;
        doorUp.transform.SetParent(transform.parent);
        doorUp.name = name;
    }

    void ApplyRender()
    {

        //apply representative matrix of the room to the texture of the ground
        Renderer rend = GetComponent<Renderer>();

        // duplicate the original texture and assign to the material
        Texture2D texture = new Texture2D(ProceduralValues.roomWidth, ProceduralValues.roomHeight, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;

        for (int i = 0; i < room.getListRect().Count; i++)
        {
            for (int x = (int)(room.getListRect()[i].x); x < ((int)room.getListRect()[i].x + (int)room.getListRect()[i].width); x++)
            {
                if (room.getRoomMatrix()[x, (int)room.getListRect()[i].y] != SpriteType.DOOR)
                    room.getRoomMatrix()[x, (int)room.getListRect()[i].y] = SpriteType.NONE;
                if (room.getRoomMatrix()[x, (int)room.getListRect()[i].y + (int)room.getListRect()[i].height - 1] != SpriteType.DOOR)
                    room.getRoomMatrix()[x, (int)room.getListRect()[i].y + (int)room.getListRect()[i].height - 1] = SpriteType.NONE;
            }
            for (int y = (int)(room.getListRect()[i].y); y < ((int)room.getListRect()[i].y + (int)room.getListRect()[i].height); y++)
            {
                if (room.getRoomMatrix()[(int)room.getListRect()[i].x, y] != SpriteType.DOOR)
                    room.getRoomMatrix()[(int)room.getListRect()[i].x, y] = SpriteType.NONE;
                if (room.getRoomMatrix()[(int)room.getListRect()[i].x + (int)room.getListRect()[i].width - 1, y] != SpriteType.DOOR)
                    room.getRoomMatrix()[(int)room.getListRect()[i].x + (int)room.getListRect()[i].width - 1, y] = SpriteType.NONE;
            }
        }


        for (int x = 0; x < ProceduralValues.roomWidth; x++)
        {
            for (int y = 0; y < ProceduralValues.roomHeight; y++)
            {
                switch (room.getRoomMatrix()[x, y])
                {
                    case SpriteType.GROUND:
                        texture.SetPixel(x, y, Color.white);
                        break;
                    case SpriteType.DOOR:
                        texture.SetPixel(x, y, Color.red);
                        break;
                    case SpriteType.ROCK:
                        texture.SetPixel(x, y, Color.blue);
                        break;
                    case SpriteType.NONE:
                        texture.SetPixel(x, y, Color.cyan);
                        break;
                    default:
                        break;
                }
            }
        }



        // actually apply all SetPixels, don't recalculate mip levels
        texture.Apply(false);
        rend.material.mainTexture = texture;
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        
    }
}
