using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ResetScene : BaseObject
{

    protected override void BaseUpdate()
    {
        base.BaseUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
