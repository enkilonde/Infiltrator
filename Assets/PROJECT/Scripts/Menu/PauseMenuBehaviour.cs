using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBehaviour : BaseObject
{

    public bool paused = false;
    public static PauseMenuBehaviour pauseBh;
    Canvas pauseCanvas;

    //PlayerStats
    Text walkSpeedTxt;
    Text killSpeedtxt;
    Text unlockSpeedTxt;
    Text actionRangeTxt;


    protected override void FirstAwake()
    {
        base.FirstAwake();

        if (pauseBh == null) pauseBh = this;
        else Destroy(this);

        pauseCanvas = GetComponent<Canvas>();
        walkSpeedTxt = pauseCanvas.transform.Find("Stats").Find("WalkSpeed").GetComponent<Text>();
        killSpeedtxt = pauseCanvas.transform.Find("Stats").Find("KillSpeed").GetComponent<Text>();
        unlockSpeedTxt = pauseCanvas.transform.Find("Stats").Find("UnlockSpeed").GetComponent<Text>();
        actionRangeTxt = pauseCanvas.transform.Find("Stats").Find("ActionRange").GetComponent<Text>();

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
            UpdatePlayerStatDisplay();
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

    void UpdatePlayerStatDisplay()
    {
        walkSpeedTxt.text = "Walk speed : " + PlayerProperties.playerWalkSpeed + "m/s";
        killSpeedtxt.text = "Kill speed : " + PlayerProperties.smotherSpeed + "s";
        unlockSpeedTxt.text = "Unlock speed : " + PlayerProperties.unlockSpeed + "s";
        actionRangeTxt.text = "Action Range : " + PlayerProperties.actionRange + "m";

    }

}
