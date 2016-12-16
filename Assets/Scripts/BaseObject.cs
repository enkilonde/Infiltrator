using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour
{

    bool loadingEnded = false;

    void Awake()
    {
        StartCoroutine(StartingProcess());
    }
    

    IEnumerator StartingProcess()
    {

        FirstAwake();

        yield return null;

        SecondAwake();

        yield return null;

        OnLoadEnded();

    }

    //C'est ici qu'on attribue les références
    protected virtual void FirstAwake()
    {

    }

    //on utilisera cette fonction comme on utilise l'Awake normalement.
    protected virtual void SecondAwake()
    {

    }

    
    protected virtual void OnLoadEnded()
    {
        loadingEnded = true;
    }


    void Update()
    {
        if (loadingEnded) BaseUpdate();
    }

    protected virtual void BaseUpdate()
    {

    }

}
