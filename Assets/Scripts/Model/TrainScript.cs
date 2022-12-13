using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainScript : MonoBehaviour
{
    public static bool IsOn;
    private bool IsWindowActive;
    private string[] lines = File.ReadAllLines("Assets/Resources/TrainText.txt");

    public GameObject ModalWindow;

    private Camera cam;

    private void ShowWindow(string header, string text)
    {
        IsWindowActive = true;
        ModalWindow.SetActive(true);

        var panelTransform = ModalWindow.transform.Find("Panel").transform;
        panelTransform.Find("Header").GetComponent<Text>().text = header;
        panelTransform.Find("Text").GetComponent<Text>().text = text;
        panelTransform
            .Find("Button")
            .GetComponent<Button>()
            .onClick
            .AddListener(HideWindow);
        Time.timeScale = 0;
    }

    public void HideWindow()
    {
        ModalWindow.SetActive(false);
        ModalWindow.transform.Find("Mask").gameObject.SetActive(false);
        Time.timeScale = 1;
        IsWindowActive = false;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        StartCoroutine(StartTrain());
    }

    private IEnumerator StartTrain()
    {
        GameModel.Balance = 0;
        WaterSpawner.IsWaiting = true;
        print("Обучение началось");
        ShowWindow(lines[0].Split("___")[0], lines[0].Split("___")[1]);
        while (IsWindowActive)
            yield return false;

        DeerSpawner.GenerateNew();
        while (GameModel.Deers.Count == 0)
            yield return false;

        var deer = GameModel.Deers.First();
        while (Vector2.Distance(deer.transform.localPosition, deer.GetComponent<Deer>().TargetPos) > 0.55f)
            yield return false;

        var i = 1;
        for (; i < 4; i++)
        {
            ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
            while (IsWindowActive)
                yield return false;
        }

        //Еда

        FoodSpawner.IsWaiting = false;
        while (GameModel.FoodSpawned.Count == 0)
            yield return false;

        FoodSpawner.IsWaiting = true;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(GameModel.FoodSpawned.First(), 
            Resources.Load<Sprite>("Food"));

        //инвентарь

        while (GameModel.FoodSpawned.Count != 0)
            yield return false;

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("InventoryUI").transform.Find("InventoryBackground").gameObject, 
            Resources.Load<Sprite>("InventoryPanel"));

        //голод

        StartCoroutine(GameModel.Deers.First().GetComponent<Deer>().GetBuff(BuffType.Hunger));
        yield return new WaitForSecondsRealtime(3);
        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        while (GameModel.Deers.First().GetComponent<Deer>().BuffType == BuffType.Hunger)
            yield return false;
        
        //задания

        i++;
        NotificationScript.IsHidden = false;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("TasksButton").gameObject, 
            transform.Find("TasksButton").GetComponent<Image>().sprite, true);

        while (SceneManager.sceneCount == 1)
            yield return false;
        
        //открываем окно заданий
        TasksTrainScript.IsOn = true;

        while (SceneManager.sceneCount > 1)
            yield return false;

        i = 11;
        // песец

        ThreatSpawner.CanArouseThreat = true;
        yield return new WaitForSecondsRealtime(5);

        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(GameModel.Threats.First(), 
            Resources.Load<Sprite>("ArcticFox"));

        while (IsWindowActive)
            yield return false;

        // магазин

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("ShopButton").gameObject, 
            transform.Find("ShopButton").GetComponent<Image>().sprite, true);

        while (SceneManager.sceneCount == 1)
            yield return false;

        ShopTrainScript.IsOn = true;

        while (SceneManager.sceneCount > 1)
            yield return false;
        
        // Конец обучения

        i = 16;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);

        while (GameModel.Threats.Count != 0)
            yield return false;

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        while (IsWindowActive)
            yield return false;
        
        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        while (IsWindowActive)
            yield return false;
        
        //

        IsOn = false;
        ThreatSpawner.CanArouseThreat = true;
        FoodSpawner.IsWaiting = false;
        WaterSpawner.IsWaiting = false;
    }

    private void ShowObject(GameObject otherObject, Sprite sprite, bool isUI=false)
    {
        var mask = ModalWindow.transform.Find("Mask");
        mask.gameObject.SetActive(true);
        mask.transform.position = otherObject.transform.position;
        mask.GetComponent<Image>().sprite = sprite;
        mask.GetComponent<RectTransform>().sizeDelta = 
            isUI 
            ? otherObject.GetComponent<RectTransform>().sizeDelta 
            : new Vector2(sprite.rect.size.x * 0.85f, sprite.rect.size.y * 0.8f);

        mask.GetComponent<RectTransform>().localScale = otherObject.transform.localScale;
    }
}