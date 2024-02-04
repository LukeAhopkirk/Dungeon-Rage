using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class StatInfo
{
    public string statName;
    public TextMeshProUGUI assignedPointsText;
    public Button plusButton;
    public Button minusButton;

    public int assignedPoints = 0;

    public int statMultiplier = 1;

    public System.Action<float> OnStatChanged;
}

public class SkillPointManager : MonoBehaviour
{
    public StatInfo[] stats;
    public TextMeshProUGUI availableSkillPointsText;
    private int availableSkillPoints = 10;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var stat in stats)
        {
            stat.plusButton.onClick.AddListener(() => AllocatePoint(stat));
            stat.minusButton.onClick.AddListener(() => DeallocatePoint(stat));

            UpdateUI(stat);
        }

        UpdateTotalSkillPoints();
    }

    void AllocatePoint(StatInfo stat)
    {
        if (availableSkillPoints > 0)
        {
            stat.assignedPoints++;
            availableSkillPoints--;
            stat.statMultiplier++;

            UpdateUI(stat);
            UpdateTotalSkillPoints();

            stat.OnStatChanged?.Invoke(stat.assignedPoints * stat.statMultiplier *0.1f);
        }
    }
    void DeallocatePoint(StatInfo stat)
    {
        if (stat.assignedPoints > 0)
        {
            stat.assignedPoints--;
            availableSkillPoints++;
            stat.statMultiplier--;

            UpdateUI(stat);
            UpdateTotalSkillPoints();

            stat.OnStatChanged?.Invoke(stat.assignedPoints * stat.statMultiplier * 0.1f);
        }
    }
    void UpdateUI(StatInfo stat)
    {
        stat.assignedPointsText.text = $"{stat.assignedPoints}";
    }

    void UpdateTotalSkillPoints()
    {
        availableSkillPointsText.text = $"{availableSkillPoints}";
    }

    public int GetStatValue(string statName)
    {
        foreach (var stat in stats)
        {
            if (stat.statName == statName)
            {
                return stat.assignedPoints * stat.statMultiplier;
            }
        }

        return 0;
    }
}
