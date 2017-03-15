using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealth : Health
{

    BaseEnemy behaviour;

    protected override void SecondAwake()
    {
        base.SecondAwake();
        behaviour = GetComponent<BaseEnemy>();
    }

    public override void TakeDamages(int value)
    {
        base.TakeDamages(value);

        if(behaviour.myState != BaseEnemy.State.ALERTED)
        behaviour.LaunchSearch(behaviour.player.transform.position);

    }


}
