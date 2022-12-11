using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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
        StartCoroutine(FisrtTwoWindows());
    }

    public void StartTrain()
    {
        FisrtTwoWindows();
    }

    private IEnumerator FisrtTwoWindows()
    {
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
        var mask = ModalWindow.transform.Find("Mask");
        mask.gameObject.SetActive(true);
        mask.GetComponent<Image>().sprite = Resources.Load<Sprite>("Food");
        mask.transform.position = GameModel.FoodSpawned.First().transform.position;
        mask.transform.localScale = GameModel.FoodSpawned.First().transform.localScale;
        //ShowObject(GameModel.FoodSpawned.First());

        //инвентарь

        while (GameModel.FoodSpawned.Count != 0)
            yield return false;

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("InventoryUI").transform.Find("InventoryBackground").gameObject);
    }

    private void ShowObject(GameObject otherObject)
    {
        var mask = ModalWindow.transform.Find("Mask");
        mask.gameObject.SetActive(true);
        mask.transform.position = otherObject.transform.position;
        var sprite = Resources.Load<Sprite>("InventoryPanel");
        mask.GetComponent<Image>().sprite = sprite;
        mask.GetComponent<RectTransform>().sizeDelta = sprite.rect.size * otherObject.transform.localScale;
    }
}