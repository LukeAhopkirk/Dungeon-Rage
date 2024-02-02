using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Image healthBar;
    public Image experienceBar;
    public Image rageBar;
    public Button[] spellButtons;

    public float healthAmount = 100f;
    public float experienceAmount = 0f;
    public float rageAmount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.fillAmount = healthAmount / 100f;
        experienceBar.fillAmount = experienceAmount / 100f;
        rageBar.fillAmount = rageAmount / 100f;

        for (int i = 0; i < spellButtons.Length; i++)
        {
            int index = i;
            spellButtons[i].onClick.AddListener(() => OnSpellButtonClicked(index));
        }
    }

    public void OnSpellButtonClicked(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0:
                // Cast spell 1
                break;
            case 1:
                // Cast spell 2
                break;
            case 2:
                // Cast spell 3
            break;
            default:
                break;
        }
    }
    private void Update()
    {

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

    public void GetExperience(float experience)
    {
        experienceAmount += experience;
        experienceAmount = Mathf.Clamp(experienceAmount, 0f, 100f);

        experienceBar.fillAmount = experienceAmount / 100f;
    }
    public void GetRage(float rage)
    {
        rageAmount += rage;
        rageAmount = Mathf.Clamp(rageAmount, 0f, 100f);

        rageBar.fillAmount = rageAmount / 100f;
    }
}
