using UnityEngine;
using System.Collections;

public class Health : BaseObject
{

    public int healthPoints = 100;


    public void TakeDamages(int value)
    {
        healthPoints -= value;
    }

}
