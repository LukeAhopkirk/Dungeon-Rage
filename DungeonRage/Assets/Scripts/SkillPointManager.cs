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

    public int assignedPoints = 20;

    public int statMultiplier = 1;

    public System.Action<float> OnStatChanged;
}

public class SkillPointManager : MonoBehaviour
{
    public StatInfo[] stats;
    public TextMeshProUGUI availableSkillPointsText;
    private int availableSkillPoints;

    public float playerLevel = 0;
    public TextMeshProUGUI playerLevelText;

    public GameObject levelUpPanel;
    private float levelNotifDuration = 2f;
    [SerializeField] private AudioSource levelUpSound;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var stat in stats)
        {
            stat.plusButton.onClick.AddListener(() => AllocatePoint(stat));
            stat.minusButton.onClick.AddListener(() => DeallocatePoint(stat));

            UpdateUI(stat);
        }
        UpdatePlayerLevel();
        UpdateTotalSkillPoints();
        levelUpPanel.SetActive(false);
    }
    public void LevelUp()
    {
        availableSkillPoints += 2;
        playerLevel++;
        UpdateTotalSkillPoints();
        UpdatePlayerLevel();
        levelUpSound.Play();
        StartCoroutine(ShowLevelUpPanel());
    }

    private IEnumerator ShowLevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        yield return new WaitForSeconds(levelNotifDuration);
        levelUpPanel.SetActive(false);
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

            stat.OnStatChanged?.Invoke(stat.assignedPoints * stat.statMultiplier);
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

            stat.OnStatChanged?.Invoke(stat.assignedPoints * stat.statMultiplier);
        }
    }
    void UpdateUI(StatInfo stat)
    {
        stat.assignedPointsText.text = $"{stat.assignedPoints}";
    }

    void UpdatePlayerLevel()
    {
        playerLevelText.text = $" {playerLevel}";
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
