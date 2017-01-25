using UnityEngine;
using System.Collections;

public class CreateColliders : BaseObject
{

    public bool[,] validpositions;
    public GameObject WallPrefab;
    public Material wallMaterial;

    public int roomLenght = 32;

    protected override void SecondAwake()
    {
        base.SecondAwake();

        SpriteType[,] rawValidPos = transform.parent.GetComponentInChildren<RenderRoom>().room.getRoomMatrix();
        validpositions = new bool[rawValidPos.GetLength(0), rawValidPos.GetLength(1)];
        for (int i = 0; i < rawValidPos.GetLength(0); i++)
        {
            for (int j = 0; j < rawValidPos.GetLength(1); j++)
            {
                validpositions[i, j] = rawValidPos[i, j] != SpriteType.ROCK;
                if(!validpositions[i, j])
                {

                    Vector3 pos = new Vector3(roomLenght/2-i - 0.5f, 0, roomLenght/2-j -0.5f);

                    GameObject wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                    wall.transform.SetParent(transform);
                }
            }
        }
        CombineChildren();



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
            //Debug.Log("destroy " + meshFilters[i].gameObject.name, meshFilters[i].gameObject);
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
