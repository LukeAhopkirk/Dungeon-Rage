using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Image healthBar;
    public Button[] spellButtons;
    public float healthAmount = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(10f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0f, 100f);

        healthBar.fillAmount = healthAmount / 100f;
    }
}
