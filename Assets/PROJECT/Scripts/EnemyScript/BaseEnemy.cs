using UnityEngine;
using System.Collections;

public class BaseEnemy : BaseObject {

    public enum State { DEAD, ASLEEP, LOCKED, AWAKE, SEARCHING, ALERTED };
    // DEAD = Devine !
    // ASLEEP = Endormis, se réveille avec un contact/dégât, une alerte (du bruit?)
    // LOCKED = Ne peut pas se déplacer mais peux tourner, utilisé pour les ennemis type caméra
    // AWAKE = Execute son Move()
    // SEARCHING = Lorsque le joueur à été entendu? ou brievement repéré par une caméra
    // ALERTED = Joueur répéré, sa position est connue de tous

    [HideInInspector]
    public State myState;

    [HideInInspector]
    private float maxHealth;
    [HideInInspector]
    private float curHealth;
    [HideInInspector]
    private float viewAngle;
    [HideInInspector]
    private float viewDistance;
    [HideInInspector]
    public float speed;

    //Lors de l'instantiation de l'ennemi on appel cette fonction pour définir ses stats de base
    public virtual void Instantiated(State st, float maxH, float curH, float viewA, float viewD, float spd)
    {
        ChangeState(st);
        maxHealth = maxH;
        curHealth = curH;
    }


    // Lorsque l'ennemi reçoi des dégâts il peut changer d'état
    protected virtual void TakeDamage(float dmg)
    {
        curHealth -= dmg;
        
        if(curHealth <= 0 )
        {
            ChangeState(State.DEAD);
            Death();
        }
        else
        {
            if (myState == State.ASLEEP)
            {
                ChangeState(State.SEARCHING);
            }
        }
    }

    public virtual void ChangeState(State st)
    {
        myState = st;
    }

    protected virtual void Move()
    {
        
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

        if(myState != State.DEAD && myState != State.ASLEEP)
        {
            Move();
        }
	
	}
	
    public void Death()
    {

    }

}
