using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : Health
{

    RawImage healthBar;

    protected override void SecondAwake()
    {
        base.SecondAwake();
        GameObject PersonalStaticCanvas = Instantiate<GameObject>(Resources.Load<GameObject>("PlayerStaticCanvas"));
        healthBar = PersonalStaticCanvas.transform.Find("HealthbarFrontground").GetComponent<RawImage>();
    }


    public override void TakeDamages(int value)
    {
        base.TakeDamages(value);

        healthBar.rectTransform.sizeDelta = new Vector2(currentHealthPoints, healthBar.rectTransform.sizeDelta.y);
    }

}
