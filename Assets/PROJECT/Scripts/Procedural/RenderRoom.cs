using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using System.Linq;

public class RenderRoom : BaseObject {
    



    public Room room;
    public Material mat;


    [Header("Test")]
    //OUAI, c'est moche, mais c'est plus pratique pour l'inspector
    public bool doorTop = true;
    public bool doorBottom = true;
    public bool doorLeft = true;
    public bool doorRight = true;

    protected override void FirstAwake()
    {

        //init();

        //ApplyRender(room, gameObject);

    }

    void init()
    {
        GameObject doorObject = Resources.Load("Door") as GameObject;
        int roomLenght = ProceduralValues.roomHeight;
        int roomSize = roomLenght/2;
        float pas = ProceduralValues.unitValue/2;
        //pas = 0;
        List<Vector2> roomList = new List<Vector2>();
        if (doorTop)
        {
            roomList.Add(new Vector2(Random.Range(1, roomLenght-1), 0));
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorTop");
        }
        if (doorBottom)
        {
            roomList.Add(new Vector2(Random.Range(1, roomLenght-1), roomLenght-1));
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorBottom");
        }
        if (doorLeft)
        {
            roomList.Add(new Vector2(roomLenght-1, Random.Range(1, roomLenght-1)));
            InstentiateDoor(doorObject, roomList.Last(), -pas, roomSize, "doorLeft");
        }
        if (doorRight)
        {
            roomList.Add(new Vector2(0, Random.Range(1, roomLenght-1)));
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

    public static void ApplyRender(Room targetRoom, GameObject targetPlane)
    {

        //apply representative matrix of the room to the texture of the ground
        Renderer rend = targetPlane.GetComponent<Renderer>();

        // duplicate the original texture and assign to the material
        Texture2D texture = new Texture2D(ProceduralValues.roomWidth, ProceduralValues.roomHeight, TextureFormat.RGBA32, false, false);
        texture.filterMode = FilterMode.Point;

        for (int i = 0; i < targetRoom.getListRect().Count; i++)
        {
            for (int x = (int)(targetRoom.getListRect()[i].x); x < ((int)targetRoom.getListRect()[i].x + (int)targetRoom.getListRect()[i].width); x++)
            {
                if (targetRoom.getRoomMatrix()[x, (int)targetRoom.getListRect()[i].y] != SpriteType.DOOR)
                    targetRoom.getRoomMatrix()[x, (int)targetRoom.getListRect()[i].y] = SpriteType.NONE;
                if (targetRoom.getRoomMatrix()[x, (int)targetRoom.getListRect()[i].y + (int)targetRoom.getListRect()[i].height - 1] != SpriteType.DOOR)
                    targetRoom.getRoomMatrix()[x, (int)targetRoom.getListRect()[i].y + (int)targetRoom.getListRect()[i].height - 1] = SpriteType.NONE;
            }
            for (int y = (int)(targetRoom.getListRect()[i].y); y < ((int)targetRoom.getListRect()[i].y + (int)targetRoom.getListRect()[i].height); y++)
            {
                if (targetRoom.getRoomMatrix()[(int)targetRoom.getListRect()[i].x, y] != SpriteType.DOOR)
                    targetRoom.getRoomMatrix()[(int)targetRoom.getListRect()[i].x, y] = SpriteType.NONE;
                if (targetRoom.getRoomMatrix()[(int)targetRoom.getListRect()[i].x + (int)targetRoom.getListRect()[i].width - 1, y] != SpriteType.DOOR)
                    targetRoom.getRoomMatrix()[(int)targetRoom.getListRect()[i].x + (int)targetRoom.getListRect()[i].width - 1, y] = SpriteType.NONE;
            }
        }


        for (int x = 0; x < ProceduralValues.roomWidth; x++)
        {
            for (int y = 0; y < ProceduralValues.roomHeight; y++)
            {
                switch (targetRoom.getRoomMatrix()[x, y])
                {
                    case SpriteType.GROUND:
                        texture.SetPixel(x, y, Color.white);
                        break;
                    case SpriteType.DOOR:
                        texture.SetPixel(x, y, Color.black);
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


    public static void CreateRoom(int x, int y, Room room, int roomIndex, Transform parent)
    {
        //GameObject salle = GameObject.CreatePrimitive(PrimitiveType.Plane);


        GameObject salle = Instantiate<GameObject>(Resources.Load<GameObject>("Salle"));
        GameObject plane = salle.GetComponentInChildren<MeshCollider>().gameObject;
        salle.name = "Room " + roomIndex + "(" + room.roomType.ToString() + ")";
        salle.transform.position = new Vector3(x * ProceduralValues.roomWidth, 0, y * ProceduralValues.roomWidth);
        plane.transform.localScale = new Vector3(ProceduralValues.roomWidth/10f, 1, ProceduralValues.roomWidth/10f);
        salle.GetComponent<RoomBehaviour>().roomClass = room;
        room.gameobject = salle;
        room.roomIndex = roomIndex;

        if(room.roomType == RoomType.BOSS)
        {
            salle.transform.Find("ButtonEndLevel").gameObject.SetActive(true);
            salle.transform.Find("ButtonEndLevel").position = salle.transform.position - new Vector3(room.getButton().x, 0, room.getButton().y) + new Vector3(ProceduralValues.roomWidth/2, 0,ProceduralValues.roomHeight/2);
        }

        if (room.roomType == RoomType.TREASURE)
        {
            GameObject item = Instantiate<GameObject>(Resources.Load<GameObject>("Items/GenericItem"));
            item.transform.position = salle.transform.position - new Vector3(room.getButton().x, 0, room.getButton().y) + new Vector3(ProceduralValues.roomWidth / 2, 0.1f, ProceduralValues.roomHeight / 2);
            item.GetComponent<ItemObject>().item = ItemsUtility.GetRandomEnum<ITEM_LIST>();
            item.transform.rotation = Quaternion.Euler(0, 0, 0);
            item.transform.SetParent(salle.transform);
        }

        //ApplyRender(room, plane);

        salle.transform.SetParent(parent);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        
    }
}
