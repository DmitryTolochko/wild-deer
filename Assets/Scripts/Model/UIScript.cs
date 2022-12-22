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
    public Image Smile;

    private float NegativeStressLevel = 0;
    public Text FemaleCount;
    public Text MaleCount;
    public Text MoneyCountElement;

    private AudioSource audioSource;
    public static bool OnCloseOtherScene;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

        Smile = transform.Find("StressLevel").transform.Find("Smile").GetComponent<Image>();
    }

    private void Update()
    {
        RefreshStressLevel();
        RefreshCounts();
        if (OnCloseOtherScene)
        {
            OnCloseOtherScene = false;
            audioSource.Play();
        }
    }

    private void RefreshStressLevel()
    {
        if (NegativeStressLevel - 0.1f * Time.deltaTime > GameModel.StressLevel)
        {
            NegativeStressLevel -= 0.1f * Time.deltaTime;
            RefreshSmile();
        }
        else if (NegativeStressLevel + 0.1f * Time.deltaTime < GameModel.StressLevel)
        {
            NegativeStressLevel += 0.1f * Time.deltaTime;
            RefreshSmile();
        }


        StressLevelBar.fillAmount = 1 - NegativeStressLevel;

        var g = StressLevelBar.fillAmount * 2;
        var r = g < 1 ? 1 : (1 - StressLevelBar.fillAmount) * 2;
        StressLevelBar.color = new Color(r, g, 0);
    }

    private void RefreshCounts()
    {
        FemaleCount.text = DeerSpawner.FemaleCount.ToString();
        MaleCount.text = DeerSpawner.MaleCount.ToString();
        MoneyCountElement.text = GameModel.Balance.ToString();
    }

    private void RefreshSmile()
    {
        if (StressLevelBar.fillAmount >=0.66f)
            Smile.sprite = Resources.Load<Sprite>("GoodSmile");
        else if (StressLevelBar.fillAmount >= 0.33f)
            Smile.sprite = Resources.Load<Sprite>("MiddleSmile");
        else
            Smile.sprite = Resources.Load<Sprite>("BadSmile");

    }

    public void ChangeSceneToTasks()
    {
        SceneManager.LoadSceneAsync("TasksScene", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }

    public void ChangeSceneToShop()
    {
        SceneManager.LoadSceneAsync("ShopScene", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
}