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
        GameObject.Find("LoadingScreenCanvas").GetComponent<Canvas>().enabled = true;



    }

    protected override void SpawnPlayer()
    {
        base.SpawnPlayer();
        GameObject player = null;
        if (PlayerController.Activated)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            player = Instantiate<GameObject>(Resources.Load<GameObject>("Player"));
        }
        
        player.transform.position = mapScript.rooms[ProceduralValues.numberOfRoom - 2].gameobject.transform.position + new Vector3(0, 1.0f, 0);
        Camera.main.GetComponent<CameraBehaviour>().targetRoom = mapScript.rooms[ProceduralValues.numberOfRoom - 2].gameobject.transform;
    }


    protected override void OnLoadEndedLate()
    {
        base.OnLoadEndedLate();
        StartCoroutine(waitForEndLoading());

    }

    IEnumerator waitForEndLoading()
    {
        bool ready = false;
        while(!ready)
        {
            BaseObject[] allBase = FindObjectsOfType<BaseObject>();
            for (int i = 0; i < allBase.Length; i++)
            {
                ready = true;
                if (!allBase[i].loadingEnded)
                {
                    ready = false;
                    break;
                }
            }
            yield return null;
        }

        GameObject.Find("LoadingScreenCanvas").GetComponent<Canvas>().enabled = false;

    }

}
