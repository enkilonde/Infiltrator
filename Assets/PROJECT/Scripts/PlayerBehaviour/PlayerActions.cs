using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerActions : BaseObject
{
    private Inventory inventory;
    private Image completionImage;
    private PlayerController controller;

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

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyDown(KeyCode.LeftAlt)) StartCoroutine(SmotherEnnemy(2, KeyCode.LeftAlt));
        if (Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(UnlockDoor(2, KeyCode.LeftControl));

        if (Input.GetKeyDown(KeyCode.Space)) ActivateItem();

        if (Input.GetKeyDown(KeyCode.A)) inventory.ChangeSelectedItem(-1);
        if (Input.GetKeyDown(KeyCode.E)) inventory.ChangeSelectedItem(1);

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

        if (doorsAround.Length == 0)
        {
            ShowRange(ActionRange);
            yield break;
        }
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

}
