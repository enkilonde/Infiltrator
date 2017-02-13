using UnityEngine;
using System.Collections;

public class TestCombineMesh : MonoBehaviour
{

    public int squareNumberOfCubes;
    public GameObject prefab;

    void Start()
    {
        for (int j = 0; j < squareNumberOfCubes; j++)
        {
            for (int i = 0; i < squareNumberOfCubes; i++)
            {
                GameObject truc = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity) as GameObject;
                truc.transform.SetParent(transform);
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            Combine();
        }
    }

    void Combine()
    {
        Mesh newMesh;
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 1;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            //Debug.Log("destroy " + meshFilters[i].gameObject.name, meshFilters[i].gameObject);
            Destroy(meshFilters[i].gameObject);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        newMesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().sharedMesh = newMesh;
        transform.gameObject.SetActive(true);
    }
}

