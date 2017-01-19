using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenuBehaviour : BaseObject
{

    public bool paused = false;
    public static PauseMenuBehaviour pauseBh;
    Canvas pauseCanvas;


    protected override void FirstAwake()
    {
        base.FirstAwake();

        if (pauseBh == null) pauseBh = this;
        else Destroy(this);

        pauseCanvas = GetComponent<Canvas>();

    }


    protected override void SecondAwake()
    {
        base.SecondAwake();
        TogglePause(true);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause(paused);
        }


    }

    void TogglePause(bool state)
    {
        if (state) // Structure pas du tout opti pour l'instant, mais c'est pour quand on rajoutera les options.
        {
            paused = false;
            pauseCanvas.enabled = false;
        }
        else
        {
            paused = true;
            pauseCanvas.enabled = true;
        }
    }


    public void Resume()
    {
        TogglePause(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    void OnDestroy()
    {
        pauseBh = null;
    }

}
