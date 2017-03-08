using UnityEngine;
using System.Collections;

public class Ranged : BaseEnemy
{
    // Valeurs de l'ennemi influant sur les valeurs procédurales
    public State startState = State.AWAKE;
    public float multHealth = 1f;
    public float multAngle = 1f;
    public float multDistance = 1f;
    public float multSpeed = 1f;
    public float multAtkRange = 1.5f;

    public float addedVA = 20f;
    public float addedVD = 1.5f;

    public float attackDelay = 1.0f;
    private float timerAttack = 0f;


    public override void EnemyActivated( int state)
    {
        Instantiated((State)state, ProceduralValues.enemyMaxHealth * multHealth,
           ProceduralValues.enemyStartHealth * multHealth,
           ProceduralValues.enemyViewAngle * multAngle,
           ProceduralValues.enemyViewDistance * multDistance,
           ProceduralValues.enemySpeed * multSpeed,
           ProceduralValues.enemyAtkRange * multAtkRange);

        addVA = addedVA;
        addVD = addedVD;
    }

    public override void SetPattern(int pat, int rectX, int rectZ, int sizeX, int sizeY, int prog)
    {
        Vector3 pos = new Vector3(rectX, 0.5f, rectZ);
        DefinePattern(pat, pos, sizeX, sizeY, prog);
    }

    protected override void Attack()
    {
        timerAttack -= Time.deltaTime;
        if(timerAttack <= 0)
        {
            timerAttack = attackDelay;
            player.GetComponent<PlayerHealth>().TakeDamages(10);
            // Damage the player
        }
    }
}
