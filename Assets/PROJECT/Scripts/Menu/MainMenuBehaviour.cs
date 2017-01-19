using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{


    public void PlayScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


}
