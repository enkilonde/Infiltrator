﻿using UnityEngine;
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

        MinimapGeneration();

        yield return null;

        RoomGeneration();

        yield return null;

        MonsterInstantiate();

        yield return null;

        OnLoadEnded();

    }

    //C'est ici qu'on attribue les références
    protected virtual void FirstAwake() { }


    //on utilisera cette fonction comme on utilise l'Awake normalement.
    protected virtual void SecondAwake() { }

    //on va générer la minimap (faite par Michael)
    protected virtual void MinimapGeneration() { }

    //On va générer les salles (fait par Nelson)
    protected virtual void RoomGeneration() { }

    //On fait pop les ennemis et items dans les salles
    protected virtual void MonsterInstantiate() { }


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
