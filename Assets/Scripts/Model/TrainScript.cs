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
        ModalWindow.transform.Find("Panel").transform.Find("Header").GetComponent<Text>().text = header;
        ModalWindow.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = text;
        ModalWindow.transform.Find("Panel").transform.Find("Button").GetComponent<Button>().onClick.AddListener(HideWindow);
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

        ModalWindow.transform.Find("Mask").gameObject.SetActive(true);
        ModalWindow.transform.Find("Mask").GetComponent<Image>().sprite = Resources.Load<Sprite>("Food");
        ModalWindow.transform.Find("Mask").transform.position = GameModel.FoodSpawned.First().transform.position;
        ModalWindow.transform.Find("Mask").transform.localScale = GameModel.FoodSpawned.First().transform.localScale;
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
        ModalWindow.transform.Find("Mask").gameObject.SetActive(true);
        ModalWindow.transform.Find("Mask").transform.position = otherObject.transform.position;
        ModalWindow.transform.Find("Mask").transform.localScale = otherObject.transform.localScale;
        try
        {
            ModalWindow.transform.Find("Mask").GetComponent<Image>().sprite = otherObject.GetComponent<Image>().sprite;
        }
        catch (NullReferenceException)
        {
            ModalWindow.transform.Find("Mask").GetComponent<Image>().sprite = otherObject.GetComponent<SpriteRenderer>().sprite;
        }
    }
}
