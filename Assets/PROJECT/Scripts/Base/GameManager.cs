using UnityEngine;
using System.Collections;

public class GameManager : BaseObject
{

    private Map mapScript;


    protected override void FirstAwake()
    {
        base.FirstAwake();
        mapScript = FindObjectOfType<Map>();
        if (mapScript == null) Debug.LogError("Missing 'Map' script in scene");



    }

    protected override void SpawnPlayer()
    {
        base.SpawnPlayer();

        GameObject player = Instantiate<GameObject>(Resources.Load<GameObject>("Player"));
        player.transform.position = mapScript.rooms[0].gameobject.transform.position + new Vector3(0, 1.0f, 0);

    }


}
