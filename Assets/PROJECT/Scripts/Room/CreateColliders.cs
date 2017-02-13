using UnityEngine;
using System.Collections;

public class CreateColliders : BaseObject
{

    public bool[,] validpositions;
    public GameObject WallPrefab;
    Material wallMaterial;
    RoomBehaviour roomBehaviourScript;

    public DoorBehaviour[] doors;
    Map mapScript;

    int[] roomTargets;

    protected override void FirstAwake()
    {
        base.FirstAwake();
        roomBehaviourScript = GetComponentInParent<RoomBehaviour>();
        mapScript = FindObjectOfType<Map>();
    }

    protected override void CollidersGeneration()
    {
        base.CollidersGeneration();

        CreateWalls();

        CreateDoors();
    }

    protected override void LinkDoors()
    {
        base.LinkDoors();

        //int[] roomTargets = roomBehaviourScript.roomClass.getDoorstargets();

        for (int i = 0; i < doors.Length; i++)
        {

            Transform LD = mapScript.rooms[roomTargets[i]].gameobject.transform.Find("LD");
            DoorBehaviour target = null;
            switch (doors[i].name)
            {
                case "Left":
                    target = LD.Find("Right").GetComponent<DoorBehaviour>();
                    break;

                case "Right":
                    target = LD.Find("Left").GetComponent<DoorBehaviour>();
                    break;

                case "Down":
                    target = LD.Find("Up").GetComponent<DoorBehaviour>();
                    break;

                case "Up":
                    target = LD.Find("Down").GetComponent<DoorBehaviour>();
                    break;

                default:
                    break;
            }//End Switch

            doors[i].TargetDoor = target;

        }

    }


    void CreateWalls()
    {
        SpriteType[,] rawValidPos = transform.parent.GetComponentInParent<RoomBehaviour>().roomClass.getRoomMatrix();
        validpositions = new bool[rawValidPos.GetLength(0), rawValidPos.GetLength(1)];
        for (int i = 0; i < rawValidPos.GetLength(0); i++)
        {
            for (int j = 0; j < rawValidPos.GetLength(1); j++)
            {
                validpositions[i, j] = rawValidPos[i, j] != SpriteType.ROCK;
                if (!validpositions[i, j])
                {

                    Vector3 pos = new Vector3(ProceduralValues.roomWidth / 2 - i - 0.5f, ProceduralValues.MeshsHeight, ProceduralValues.roomHeight / 2 - j - 0.5f);

                    GameObject wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                    wall.transform.SetParent(transform);
                }
            }
        }
        CombineChildren();

    }

    void CreateDoors()
    {
        Vector2[] doorsPos = roomBehaviourScript.roomClass.getVectorDoor();
        roomTargets = roomBehaviourScript.roomClass.getDoorstargets();
        doors = new DoorBehaviour[roomTargets.Length];
        for (int i = 0; i < roomTargets.Length; i++)
        {
            GameObject door = Instantiate<GameObject>(Resources.Load<GameObject>("Door"));
            door.transform.position = new Vector3(ProceduralValues.roomWidth, 0, ProceduralValues.roomWidth) /2 + transform.parent.position - new Vector3(doorsPos[i].x + ProceduralValues.unitValue / 2, 0, doorsPos[i].y + ProceduralValues.unitValue / 2);
            door.transform.SetParent(transform.parent.Find("LD"));
            doors[i] = door.GetComponent<DoorBehaviour>();

            if (doorsPos[i].x == 31)
            {
                door.name = "Left";
                door.transform.LookAt(door.transform.position + Vector3.right);
            }
            else if (doorsPos[i].x == 0)
            {
                door.name = "Right";
                door.transform.LookAt(door.transform.position + Vector3.left);
            }
            else if (doorsPos[i].y == 31)
            {
                door.name = "Down";
                door.transform.LookAt(door.transform.position + Vector3.forward);
            }
            else if (doorsPos[i].y == 0)
            {
                door.name = "Up";
                door.transform.LookAt(door.transform.position + Vector3.back);
            }

        }

    }


    void CombineChildren()
    {
        Mesh newMesh;
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        wallMaterial = WallPrefab.GetComponent<Renderer>().sharedMaterial;
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            Destroy(meshFilters[i].gameObject);
            i++;
        }
        gameObject.AddComponent<MeshRenderer>().material = wallMaterial;
        gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        newMesh = gameObject.GetComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshCollider>().sharedMesh = newMesh;
        transform.gameObject.SetActive(true);
    }

}
