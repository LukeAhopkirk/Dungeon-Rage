using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatInfo
{
    public Text availablePointsText;
    public Text assignedPointsText;
    public Button plusButton;
    public Button minusButton;

    public int availablePoints = 0;
    public int assignedPoints = 0;

    public int statMultiplier = 1;
}

public class SkillPointManager : MonoBehaviour
{
    public StatInfo[] stats;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var stat in stats)
        {
            stat.plusButton.onClick.AddListener(() => AllocatePoint(stat));
            stat.minusButton.onClick.AddListener(() => DeallocatePoint(stat));
            stat.statMultiplier++;

            UpdateUI(stat);
        }
    }

    void DeallocatePoint(StatInfo stat)
    {
        if (stat.assignedPoints > 0)
        {
            stat.assignedPoints--;
            stat.availablePoints++;
            stat.statMultiplier--;

            UpdateUI(stat);
        }
    }
    void UpdateUI(StatInfo stat)
    {
        stat.availablePointsText.text = stat.availablePoints.ToString();
        stat.assignedPointsText.text = stat.assignedPoints.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
