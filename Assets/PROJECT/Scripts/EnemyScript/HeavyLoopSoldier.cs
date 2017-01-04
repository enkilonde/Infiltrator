using UnityEngine;
using System.Collections;

public class HeavyLoopSoldier : BaseEnemy
{
    public int zoneX;
    public int zoneY;
    public Vector3[] loopPoints;
    private Vector3 dirDown;
    private Vector3 dirForward;

    private Transform pattern;
    public int nextPoint;
    private Vector3 dir;

    protected override void FirstAwake()
    {
        dirDown = transform.right;
        dirForward = transform.forward;
        pattern = transform.parent;
        Instantiated(State.AWAKE, 100, 100, 60, 3, 1);
        SetLoopPoints();
        Debug.Log("first");
        Debug.Log(myState);
    }



    void SetLoopPoints()
    {
        loopPoints = new Vector3[4];
        loopPoints[0] = pattern.position + 0.5f * pattern.forward + 0.5f * pattern.right;
        loopPoints[1] = loopPoints[0] + (zoneX - 1) * pattern.forward;
        loopPoints[2] = loopPoints[1] + (zoneY - 1) * pattern.right;
        loopPoints[3] = loopPoints[0] + (zoneY - 1) * pattern.right;
    }

    protected override void Move()
    {
        dir = (loopPoints[nextPoint] - transform.position).normalized * speed ;
        Debug.Log(transform.position);
        transform.position = transform.position + dir * Time.deltaTime;
        if(Vector3.Distance(transform.position, loopPoints[nextPoint]) <= 0.1f)
        {
            transform.position = loopPoints[nextPoint];
            nextPoint = ((nextPoint + 1) % loopPoints.Length);
            transform.LookAt(loopPoints[nextPoint]);
        }
        Debug.Log("moving");
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

    }
}
