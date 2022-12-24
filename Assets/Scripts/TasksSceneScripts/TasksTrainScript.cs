using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TasksTrainScript : MonoBehaviour
{
    public GameObject ModalWindow;
    public static bool IsOn;

    private bool IsWindowActive;
    private string file;
    private List<string> lines;

private void Start()
    {
        lines = new List<string> {"Добро пожаловать!___Вы - сотрудник организации по защите природы и диких животных. Вам предстоит развить и защитить стадо оленей!",
                                        "Знакомство___На северо-западе Восточного Таймыра обнаружен детеныш Rangifer tarandus - Северный Олень.", 
                                        "Знакомство___Ваша задача помочь олененку выжить в суровых условиях и увеличить стадо. ",
                                        "Знакомство___Объект должен быть под постоянным наблюдением, поэтому мы расставили по периметру камеры видеонаблюдения, это облегчит выполнение миссии.",
                                        "Еда___Северные олени в основном питаются ягелем - разновидностью лишайника. Соберите ягель, нажав по нему",
                                        "Инвентарь___Внизу экрана расположен ИНВЕНТАРЬ. В нем хранятся все твои предметы",
                                        "Олень проголодался!___Смотри, олень хочет есть! Перетащи на него собранный тобой ранее ягель, чтобы покормить.",
                                        "Задания___Смотри, появилась красная точка. Давай посмотрим, что там!",
                                        "Задания___Здесь отображаются задачи, выполняя которые ты можешь получить вознаграждение.",
                                        "Задания___Одно из заданий ты уже выполнил, забери свою награду.",
                                        "Задания___Теперь вернись на главный экран",
                                        "Внимание!___Первая встреча с хищником! Нужно защитить олененка! ",
                                        "Внимание!___Зайди в магазин, чтобы купить необходимый предмет, который поможет нам справиться с нависшей угрозой!",
                                        "Магазин___Здесь ты можешь приобрести всё необходимое, главное чтобы хватило денег.",
                                        "Магазин___Сейчас тебе нужен капкан. Купи его!",
                                        "Магазин___Скорее возвращайся к нашему оленю!",
                                        "Капкан___Чтобы применить капкан, переведи его на песца",
                                        "Отлично!___Теперь подведём итог. Выполняй задания чтобы получать деньги.",
                                        "Отлично!___Когда появится какой-то недоброжелатель - скорее покупай нужный инвентарь и применяй его!",
                                        "Отлично!___И не забывай, что олени хотят есть и пить, а ещё могут болеть."};

    }

    private void ShowWindow(string header, string text, bool showButton = true)
    {
        IsWindowActive = true;
        ModalWindow.SetActive(true);

        var panelTransform = ModalWindow.transform.Find("Panel").transform;
        panelTransform.Find("Header").GetComponent<Text>().text = header;
        panelTransform.Find("Text").GetComponent<Text>().text = text;
        if (showButton)
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

    private void HideButton()
    {
        var panelTransform = ModalWindow.transform.Find("Panel").transform;
        panelTransform.Find("Button").gameObject.SetActive(false);
    }

    public IEnumerator Begin()
    {
        IsOn = false;
        transform.Find("Panel").transform.Find("TaskFirst").gameObject.SetActive(false);
        transform.Find("Panel").transform.Find("TaskSecond").gameObject.SetActive(false);
        transform.Find("Panel").transform.Find("TaskThird").gameObject.SetActive(false);
        transform.Find("Panel").transform.Find("BackButton").gameObject.SetActive(false);
        ModalWindow = transform.Find("Panel").transform.Find("ModalWindow").gameObject;
        var i = 8;
        
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);

        while (IsWindowActive)
            yield return false;

        i++;
        transform.Find("Panel").transform.Find("TaskFirst").gameObject.SetActive(true);
        ModalWindow = transform.Find("Panel").transform.Find("ModalWindow2").gameObject;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1], false);
        ShowObject(transform.Find("Panel").transform.Find("TaskFirst").gameObject,
        Resources.Load<Sprite>("CardBackround"),
        true);

        while (GameModel.Balance == 0)
            yield return false;

        HideWindow();
        i++;
        transform.Find("Panel").transform.Find("BackButton").gameObject.SetActive(true);
        ModalWindow = transform.Find("Panel").transform.Find("ModalWindow").gameObject;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("Panel").transform.Find("BackButton").gameObject,
            Resources.Load<Sprite>("CloseButton"), 
            true);
        HideButton();

        transform.Find("Panel").transform.Find("TaskSecond").gameObject.SetActive(true);
        transform.Find("Panel").transform.Find("TaskThird").gameObject.SetActive(true);
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
