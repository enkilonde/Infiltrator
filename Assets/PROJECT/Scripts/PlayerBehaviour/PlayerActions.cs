﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerActions : BaseObject
{
    private Inventory inventory;
    private Image completionImage;
    private PlayerController controller;
    private RawImage wideMinimap;
    private Camera minimapCam;

    float ActionRange;

    float smotherTime;
    float unlockTime;


    protected override void FirstAwake()
    {
        base.FirstAwake();
        inventory = GetComponent<Inventory>();
        controller = GetComponent<PlayerController>();
    }

    protected override void SecondAwake()
    {
        base.SecondAwake();

        GameObject PersonalCanvas = Instantiate<GameObject>(Resources.Load<GameObject>("PlayerCanvas"));
        PersonalCanvas.GetComponent<FollowTarget>().targetToFollow = transform;
        completionImage = PersonalCanvas.transform.GetChild(0).GetComponent<Image>();


        smotherTime = PlayerProperties.smotherSpeed;
        unlockTime = PlayerProperties.unlockSpeed;
        ActionRange = PlayerProperties.actionRange;

    }

    protected override void OnLoadEnded()
    {
        base.OnLoadEnded();
        wideMinimap = GameObject.Find("FullMiniMap").GetComponent<RawImage>();
        minimapCam = GameObject.Find("Minimap").GetComponentInChildren<Camera>();
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyDown(KeyCode.LeftAlt)) StartCoroutine(SmotherEnnemy(2, KeyCode.LeftAlt));
        if (Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(UnlockDoor(2, KeyCode.LeftControl));

        if (Input.GetKey(KeyCode.Space)) ActivateItem();

        if (Input.GetKeyDown(KeyCode.A)) inventory.ChangeSelectedItem(-1);
        if (Input.GetKeyDown(KeyCode.E)) inventory.ChangeSelectedItem(1);

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyUp(KeyCode.Tab)) WidenMinimap();

    }

    IEnumerator WaitThenCallback(float timeToWait, KeyCode touche, System.Action callback = null)
    {
        while (timeToWait > 0)
        {
            yield return null;
            if (!Input.GetKey(touche)) yield break;
            timeToWait -= Time.deltaTime;
        }

        if(callback != null)
        callback();
    }

    IEnumerator SmotherEnnemy(float timeToWait, KeyCode touche)
    {
        Collider[] ennemysAround = Physics.OverlapSphere(transform.position, ActionRange, 1 << LayerMask.NameToLayer("Ennemy"));
        if (ennemysAround.Length == 0)
        {
            ShowRange(ActionRange);
            yield break;
        }
        float minDist = ActionRange + 1;

        Transform nearestEnnemy = null;

        for (int i = 0; i < ennemysAround.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, ennemysAround[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestEnnemy = ennemysAround[i].transform;
            }
        }

        if (nearestEnnemy.GetComponent<BaseEnemy>().myState == BaseEnemy.State.ALERTED) yield break;
        ChangeEnnemyColor(nearestEnnemy.gameObject, Color.gray);
        nearestEnnemy.GetComponent<BaseEnemy>().Strangled();

        controller.canMove = false;

        float timer = timeToWait;

        while (timer > 0)
        {
            yield return null;
            SetCompletionUI(timer / timeToWait);
            if (!Input.GetKey(touche))
            {
                EndAction();
                ChangeEnnemyColor(nearestEnnemy.gameObject, Color.red);
                nearestEnnemy.GetComponent<BaseEnemy>().FailedStrangle();
                yield break;
            }
            timer -= Time.deltaTime;
        }
        EndAction();
        Destroy(nearestEnnemy.gameObject);
    }

    IEnumerator UnlockDoor(float timeToWait, KeyCode touche)
    {
        Collider[] doorsAround = Physics.OverlapSphere(transform.position, ActionRange, 1 << LayerMask.NameToLayer("Doors"));

        float minDist = ActionRange + 1;

        Transform nearestDoor = null;

        for (int i = 0; i < doorsAround.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, doorsAround[i].transform.position);
            if (dist < minDist && doorsAround[i].GetComponent<DoorBehaviour>().state == DoorBehaviour.doorState.LOCKED)
            {
                minDist = dist;
                nearestDoor = doorsAround[i].transform;
            }
        }

        if(nearestDoor == null)
        {
            ShowRange(ActionRange);
            yield break;
        }

        controller.canMove = false;

        nearestDoor.GetComponent<DoorBehaviour>().SetState(DoorBehaviour.doorState.UNLOCKING);


        float timer = timeToWait;

        while (timer > 0)
        {
            yield return null;
            SetCompletionUI(timer / timeToWait);
            if (!Input.GetKey(touche))
            {
                nearestDoor.GetComponent<DoorBehaviour>().SetState(DoorBehaviour.doorState.LOCKED);
                EndAction();
                yield break;
            }
            timer -= Time.deltaTime;
        }
        EndAction();
        nearestDoor.GetComponent<DoorBehaviour>().SetState(DoorBehaviour.doorState.OPEN);
    }

    void ChangeEnnemyColor(GameObject ennemy, Color color)
    {

        ennemy.transform.GetComponentInChildren<Renderer>().material.color = color;

    }

    void EndAction(bool suceed = true)
    {
        SetCompletionUI(0, false);
        controller.canMove = true;
    }

    void SetCompletionUI(float value, bool activated = true)
    {

        completionImage.fillAmount = value;

        completionImage.enabled = activated;

    }

    void UnlockDoor()
    {
        Collider[] doorsAround = Physics.OverlapSphere(transform.position, ActionRange, 1 << LayerMask.NameToLayer("Ennemy"));
        if (doorsAround.Length == 0) return;
        float minDist = ActionRange + 1;

        Transform nearestDoor = null;

        for (int i = 0; i < doorsAround.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, doorsAround[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestDoor = doorsAround[i].transform;
            }
        }



    }

    void ActivateItem()
    {
        inventory.ActivateSelectedItem();
    }

    void ShowRange(float range)
    {
        ParticleSystem part;
        GameObject partObj = Instantiate(Resources.Load<GameObject>("RangeShow"), transform.position, Quaternion.identity) as GameObject;
        partObj.transform.Rotate(-90, 0, 0);
        part = partObj.GetComponent<ParticleSystem>();
        part.startSpeed = range / part.startLifetime;
        part.Play();
        Destroy(part.gameObject, part.startLifetime);
    }

    void WidenMinimap()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            wideMinimap.enabled = true;
            RenderTexture rTex = new RenderTexture(1920, 1080, 16);
            minimapCam.targetTexture = rTex;
            wideMinimap.texture = rTex;
            minimapCam.rect = new Rect(0, 0, 1, 1);
        }
        else
        {
            wideMinimap.enabled = false;
            minimapCam.targetTexture = null;
            minimapCam.rect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);

        }




    }

}
