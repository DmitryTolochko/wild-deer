using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TasksTrainScript : MonoBehaviour
{
    public GameObject ModalWindow;
    public static bool IsOn;

    private bool IsWindowActive;
    private string[] lines = File.ReadAllLines("Assets/Resources/TrainText.txt");

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

    private void Update() 
    {
        if (IsOn)
        {
            StartCoroutine(Begin());
        }
    }

    public void HideWindow()
    {
        ModalWindow.SetActive(false);
        ModalWindow.transform.Find("Mask").gameObject.SetActive(false);
        Time.timeScale = 1;
        IsWindowActive = false;
    }

    public IEnumerator Begin()
    {
        IsOn = false;
        ModalWindow = transform.Find("ModalWindow").gameObject;
        var i = 8;
        
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);

        while (IsWindowActive)//(GameModel.Balance == 0)
            yield return false;

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);

        while (GameModel.Balance == 0)
            yield return false;

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("Panel").transform.Find("BackButton").gameObject,
            Resources.Load<Sprite>("CloseButton"), 
            true);

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
