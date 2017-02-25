using UnityEngine;
using System.Collections;

public class Soldier : BaseEnemy
{
    // Valeurs de l'ennemi influant sur les valeurs procédurales
    public State startState = State.AWAKE;
    public float multHealth = 1f;
    public float multAngle = 1f;
    public float multDistance = 1f;
    public float multSpeed = 1f;
    public float multAtkRange = 1f;


    /*protected override void MonsterInstantiate()
    {
        base.MonsterInstantiate();

        Instantiated(startState, ProceduralValues.enemyMaxHealth * multHealth, 
            ProceduralValues.enemyStartHealth * multHealth, 
            ProceduralValues.enemyViewAngle * multAngle, 
            ProceduralValues.enemyViewDistance * multDistance,
            ProceduralValues.enemySpeed * multSpeed, 
            ProceduralValues.enemyAtkRange * multAtkRange);
        //DefinePattern(1, transform.position, 5, 8, 0);
        //DefinePattern(patternSelect, transform.position, sizeX, sizeY, 0);
    }
    */


    public void EnemyActivated( int state)
    {
        Instantiated((State)state, ProceduralValues.enemyMaxHealth * multHealth,
           ProceduralValues.enemyStartHealth * multHealth,
           ProceduralValues.enemyViewAngle * multAngle,
           ProceduralValues.enemyViewDistance * multDistance,
           ProceduralValues.enemySpeed * multSpeed,
           ProceduralValues.enemyAtkRange * multAtkRange);
    }

    public void SetPattern(int pat, int rectX, int rectZ, int sizeX, int sizeY, int prog)
    {
        Vector3 pos = new Vector3(rectX, 0.5f, rectZ);
        DefinePattern(pat, pos, sizeX, sizeY, prog);
    }
}
