using UnityEngine;
using System.Collections;


public class Health : BaseObject
{

    public int currentHealthPoints = 100;
    public int maxHealth = 100;


    protected override void SecondAwake()
    {
        base.SecondAwake();
        currentHealthPoints = maxHealth;
    }

    public virtual void TakeDamages(int value)
    {
        currentHealthPoints -= value;
        Mathf.Clamp(currentHealthPoints, 0, maxHealth);

        if(currentHealthPoints == 0)
        {
            if(tag == "Player")
            {
                GameOver();
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }


    public void GameOver()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0; //TO DO
    }

}
