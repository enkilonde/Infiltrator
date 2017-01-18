using UnityEngine;
using System.Collections;

public class Soldier : BaseEnemy
{
    public int patternSelect;
    public int sizeX, sizeY;

    public float viewA, viewD, spd;
    public float atkR;

    protected override void FirstAwake()
    {
        base.FirstAwake();
       // Instantiated(State.AWAKE, 100f, 100f, viewA, viewD, spd, atkR);
        //DefinePattern(1, transform.position, 5, 8, 0);
      //  DefinePattern(patternSelect, transform.position, sizeX, sizeY, 0);
    }
}
