using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Health : BaseObject
{

    public int currentHealthPoints = 100;
    public int maxHealth = 100;
    Renderer rend;
    Color baseColor;


    protected override void SecondAwake()
    {
        base.SecondAwake();
        currentHealthPoints = maxHealth;
        rend = GetComponent<Renderer>();
        if(rend == null)
        {
            rend = GetComponentInChildren<Renderer>();
        }
        baseColor = rend.material.color;
    }

    public virtual void TakeDamages(int value)
    {
        currentHealthPoints -= value;
        Mathf.Clamp(currentHealthPoints, 0, maxHealth);
        StartCoroutine(ChangeColorOnDamages());
        if(currentHealthPoints <= 0)
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

    IEnumerator ChangeColorOnDamages()
    {
        rend.material.color = Color.red;

        yield return null;
        yield return null;
        yield return null;
        yield return null;

        rend.material.color = baseColor;
    }


    public void GameOver()
    {
        //Debug.Log("GAME OVER");

        BaseObject.BeforeChangeScene(true);

        SceneManager.LoadScene(0);
        //Time.timeScale = 0; //TO DO
    }

}
