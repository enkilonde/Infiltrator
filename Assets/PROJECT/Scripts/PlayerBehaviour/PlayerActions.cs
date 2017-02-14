using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerActions : BaseObject
{
    private Inventory inventory;
    private Image completionImage;

    public float ActionRange = 4;


    protected override void FirstAwake()
    {
        base.FirstAwake();
        inventory = GetComponent<Inventory>();
        //completionImage = GameObject.Find("CompletionImage").GetComponent<Image>();
    }

    protected override void SecondAwake()
    {
        base.SecondAwake();

        GameObject PersonalCanvas = Instantiate<GameObject>(Resources.Load<GameObject>("PlayerCanvas"));
        PersonalCanvas.GetComponent<FollowTarget>().targetToFollow = transform;
        completionImage = PersonalCanvas.transform.GetChild(0).GetComponent<Image>();
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyDown(KeyCode.LeftAlt)) StartCoroutine(SmotherEnnemy(2, KeyCode.LeftAlt));
        if (Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(UnlockDoor(2, KeyCode.LeftControl));

        if (Input.GetKeyDown(KeyCode.Space)) ActivateItem();

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
        if (ennemysAround.Length == 0) yield break;
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

        ChangeEnnemyColor(nearestEnnemy.gameObject, Color.gray);
        BaseEnemy.State oldState = nearestEnnemy.GetComponent<BaseEnemy>().myState;
        nearestEnnemy.GetComponent<BaseEnemy>().myState = BaseEnemy.State.LOCKED;

        float timer = timeToWait;

        while (timer > 0)
        {
            yield return null;
            SetCompletionUI(timer / timeToWait);
            if (!Input.GetKey(touche))
            {
                ChangeEnnemyColor(nearestEnnemy.gameObject, Color.white);
                SetCompletionUI(0, false);
                nearestEnnemy.GetComponent<BaseEnemy>().myState = oldState;
                yield break;
            }
            timer -= Time.deltaTime;
        }
        SetCompletionUI(0, false);

        Destroy(nearestEnnemy.gameObject);
    }

    IEnumerator UnlockDoor(float timeToWait, KeyCode touche)
    {
        Collider[] doorsAround = Physics.OverlapSphere(transform.position, ActionRange, 1 << LayerMask.NameToLayer("Doors"));
        print(doorsAround.Length);
        if (doorsAround.Length == 0) yield break;
        float minDist = ActionRange + 1;

        Transform nearesDoor = null;

        for (int i = 0; i < doorsAround.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, doorsAround[i].transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearesDoor = doorsAround[i].transform;
            }
        }

        ChangeEnnemyColor(nearesDoor.gameObject, Color.gray);

        float timer = timeToWait;

        while (timer > 0)
        {
            yield return null;
            SetCompletionUI(timer / timeToWait);
            if (!Input.GetKey(touche))
            {
                ChangeEnnemyColor(nearesDoor.gameObject, Color.white);
                SetCompletionUI(0, false);
                yield break;
            }
            timer -= Time.deltaTime;
        }
        SetCompletionUI(0, false);

        nearesDoor.GetComponent<DoorBehaviour>().ToggleLock(false);
    }

    void ChangeEnnemyColor(GameObject ennemy, Color color)
    {

        ennemy.transform.GetComponentInChildren<Renderer>().material.color = color;

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

}
