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
        if (Input.GetKeyUp(KeyCode.R) && reseted)
        {
            reseted = false;
            ActivateReset();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reseted = true;
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

        if (!hardReset)
        {
            ProceduralValues.partSize = 25;
            ProceduralValues.numberOfRoom = 100;
        }
        else
        {
            ProceduralValues.partSize = 4;
            ProceduralValues.numberOfRoom = 4;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
