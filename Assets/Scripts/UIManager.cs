using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text statsText;
    public GameObject gameOverPanel;
    public Button restartButton;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        UpdateStatsDisplay();
    }

    void UpdateStatsDisplay()
    {
        statsText.text = $"Ã¼·Â: {playerStats.health} | Á¤½Å·Â: {playerStats.sanity}\n" +
                         $"Èû: {playerStats.strength} | ¹ÎÃ¸: {playerStats.agility}\n" +
                         $"Áö·Â: {playerStats.intellect} | ¿î: {playerStats.luck}";
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    void RestartGame()
    {
        gameOverPanel.SetActive(false);
        playerStats.RollStats();
        UpdateStatsDisplay();
        FindObjectOfType<GameManager>().LoadEvent("start");
    }
}
