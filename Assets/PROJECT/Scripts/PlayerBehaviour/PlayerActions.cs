using UnityEngine;
using System.Collections;

public class PlayerActions : BaseObject
{
    private Inventory inventory;


    public float smotherRange = 4;


    protected override void FirstAwake()
    {
        base.FirstAwake();
        inventory = GetComponent<Inventory>();

    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if (Input.GetKeyDown(KeyCode.LeftAlt)) SmotherEnnemy();

        if (Input.GetKeyDown(KeyCode.Space)) ActivateItem();

    }

    void SmotherEnnemy()
    {
        Collider[] ennemysAround = Physics.OverlapSphere(transform.position, smotherRange, 1 << LayerMask.NameToLayer("Ennemy"));
        if (ennemysAround.Length == 0) return;
        float minDist = smotherRange + 1;

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

        Destroy(nearestEnnemy.gameObject);
    }

    void ActivateItem()
    {
        inventory.ActivateSelectedItem();


    }

}
