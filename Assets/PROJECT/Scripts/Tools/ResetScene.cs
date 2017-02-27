using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ResetScene : BaseObject
{

    bool reseted = false;
    float timeReset;

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (Input.GetKeyUp(KeyCode.R))
        {
            ActivateReset();
        }


        if (Input.GetKey(KeyCode.R))
        {
            timeReset += Time.deltaTime;
            if (timeReset >= 5)
            {
                ActivateReset(true);
            }
        }

    }



    public static void ActivateReset(bool hardReset = false)
    {
        BaseObject[] allBO = FindObjectsOfType<BaseObject>();

        for (int i = 0; i < allBO.Length; i++)
        {
            allBO[i].EndLevel(hardReset);
        }
        PlayerProperties.resetplayer = hardReset;
        PlayerProperties.Reset();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
