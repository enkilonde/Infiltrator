using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : BaseObject
{


    private Rigidbody playerRigidbody;


    public int currentRoom = 0;

    public bool invisible = false;
    public bool canMove = true;

    public static bool Activated;

    protected override void SecondAwake()
    {
        base.SecondAwake();
        currentRoom = ProceduralValues.partSize - 2;
    }

    void OnEnable()
    {
        if (!Activated)
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            Activated = true;
        }

    }


    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        throw new System.NotImplementedException();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        gameObject.SetActive(true);
        if (playerRigidbody != null) // Si le gameobject éxiste déjà (rappel, le joueur n'ets pas détruit à un changement de scene), on doit lancer les awakes manuellement
        {
            print("Level was loaded");
            BaseObject[] playerBaseobjects = GetComponents<BaseObject>();
            foreach(BaseObject bo in playerBaseobjects)
            {
                bo.StartCoroutine(bo.StartingProcess());
            }
        }

    }

    protected override void FirstAwake()
    {
        base.FirstAwake();

        DontDestroyOnLoad(gameObject);

        PlayerProperties.Reset();

        playerRigidbody = GetComponent<Rigidbody>();
    }



    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        MovePlayer();


    }

    void MovePlayer()
    {

        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxisRaw("Horizontal");
        
        direction.z = Input.GetAxisRaw("Vertical");

        playerRigidbody.velocity = direction.normalized * PlayerProperties.playerWalkSpeed * System.Convert.ToInt32(canMove);

        if(direction.magnitude != 0) transform.LookAt(transform.position + direction.normalized);

    }

    public override void EndLevel(bool hardReset = false)
    {
        base.EndLevel(hardReset);
        if (!hardReset)
        {
            gameObject.SetActive(false);
            loadingEnded = false;
        }
        else
        {
            Destroy(gameObject);
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            Activated = false;
        }

    }


}
