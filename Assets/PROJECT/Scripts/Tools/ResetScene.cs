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

        BaseObject.BeforeChangeScene(hardReset);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
