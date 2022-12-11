using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Image StressLevelBar;
    public Text FemaleCount;
    public Text MaleCount;
    public Text MoneyCountElement;
    
    private void Start()
    {
        StressLevelBar = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "StressLevelSlider")
            .GetComponent<Image>();

        FemaleCount = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "FemaleCount")
            .GetComponent<Text>();

        MaleCount = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "MaleCount")
            .GetComponent<Text>();
    }

    private void Update()
    {
        RefreshStressLevel();
        RefreshCounts();
    }

    private void RefreshStressLevel()
    {
        if (StressLevelBar.fillAmount - 0.1f * Time.deltaTime > GameModel.StressLevel)
            StressLevelBar.fillAmount -= 0.1f * Time.deltaTime;
        else if (StressLevelBar.fillAmount + 0.1f * Time.deltaTime < GameModel.StressLevel)
            StressLevelBar.fillAmount += 0.1f * Time.deltaTime;

        var r = StressLevelBar.fillAmount * 2;
        var g = r < 1 ? 1 : (1 - StressLevelBar.fillAmount) * 2;
        StressLevelBar.color = new Color(r, g, 0);
        // print($"{r} {g}");
    }

    private void RefreshCounts()
    {
        FemaleCount.text = DeerSpawner.FemaleCount.ToString();
        MaleCount.text = DeerSpawner.MaleCount.ToString();
        MoneyCountElement.text = GameModel.Balance.ToString();
    }

    public void ChangeSceneToTasks()
    {
        SceneManager.LoadScene("TasksScene", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }

    public void ChangeSceneToShop()
    {
        SceneManager.LoadScene("ShopScene", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
}