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
    private string file; // = Application.streamingAssetsPath + "/TrainText.txt";
    private List<string> lines;

    public GameObject ModalWindow;
    public GameObject Debug;

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

    // private void Start()
    // {
    //     lines = new List<string> {"Добро пожаловать!___Вы - сотрудник организации по защите природы и диких животных. Вам предстоит развить и защитить стадо оленей!",
    //                                     "Знакомство___На северо-западе Восточного Таймыра обнаружен детеныш Rangifer tarandus - Северный Олень.", 
    //                                     "Знакомство___Ваша задача помочь олененку выжить в суровых условиях и увеличить стадо. ",
    //                                     "Знакомство___Объект должен быть под постоянным наблюдением, поэтому мы расставили по периметру камеры видеонаблюдения, это облегчит выполнение миссии.",
    //                                     "Еда___Северные олени в основном питаются ягелем - разновидностью лишайника. Соберите ягель, нажав по нему",
    //                                     "Инвентарь___Внизу экрана расположен ИНВЕНТАРЬ. В нем хранятся все твои предметы",
    //                                     "Олень проголодался!___Смотри, олень хочет есть! Перетащи на него собранный тобой ранее ягель, чтобы покормить.",
    //                                     "Задания___Смотри, появилась красная точка. Давай посмотрим, что там! Нажми на кнопку в левом нижнем углу",
    //                                     "Задания___Это список Заданий. Здесь отображаются задачи, выполняя которые, ты можешь получить вознаграждение, которое тебе точно пригодится позже.",
    //                                     "Задания___Смотри, одно из заданий ты уже выполнил, забери свою награду.",
    //                                     "Задания___Теперь вернись на главный экран",
    //                                     "Внимание!___Первая встреча с хищником! Нужно защитить олененка! ",
    //                                     "Внимание!___Зайди в магазин, чтобы купить необходимый предмет, который поможет нам справиться с нависшей угрозой!",
    //                                     "Магазин___Здесь ты можешь приобрести всё необходимое, главное чтобы хватило денег.",
    //                                     "Магазин___Сейчас тебе нужен капкан. Купи его!",
    //                                     "Магазин___Скорее возвращайся к нашему оленю!",
    //                                     "Капкан___Чтобы применить капкан, переведи его на песца",
    //                                     "Отлично!___Теперь подведём итог. Выполняй задания чтобы получать деньги.",
    //                                     "Отлично!___Когда появится какой-то недоброжелатель - скорее покупай нужный инвентарь и применяй его!",
    //                                     "Отлично!___И не забывай, что олени хотят есть и пить, а ещё могут болеть."};
    //
    //     ModalWindow = transform.Find("ModalWindow").gameObject;
    //     StartCoroutine(StartTrain());
    // }

    private IEnumerator StartTrain()
    {
        GameModel.Balance = 0;
        ThreatSpawner.CanArouseThreat = false;
        FoodSpawner.IsWaiting = true;
        WaterSpawner.IsWaiting = true;
        print("Обучение началось");
        ShowWindow(lines[0].Split("___")[0], lines[0].Split("___")[1]);
        Debug.GetComponent<Text>().text = "Обучение началось";
        while (IsWindowActive)
            yield return new WaitForSeconds(0);
        Debug.GetComponent<Text>().text = "--";
        DeerSpawner.GenerateNew();
        while (GameModel.Deers.Count == 0)
            yield return new WaitForSeconds(0);

        var deer = GameModel.Deers.First();
        while (Vector2.Distance(deer.transform.localPosition, deer.GetComponent<Deer>().TargetPos) > 0.55f)
            yield return new WaitForSeconds(0);

        var i = 1;
        for (; i < 4; i++)
        {
            ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
            while (IsWindowActive)
                yield return new WaitForSeconds(0);
        }

        //Еда

        FoodSpawner.IsWaiting = false;
        while (GameModel.FoodSpawned.Count == 0)
            yield return new WaitForSeconds(0);

        FoodSpawner.IsWaiting = true;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(GameModel.FoodSpawned.First(),
            Resources.Load<Sprite>("Food"));

        //инвентарь

        while (GameModel.FoodSpawned.Count != 0)
            yield return new WaitForSeconds(0);

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
            yield return new WaitForSeconds(0);

        //задания

        i++;
        NotificationScript.IsHidden = false;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("TasksButton").gameObject,
            transform.Find("TasksButton").GetComponent<Image>().sprite, true);

        while (SceneManager.sceneCount == 1)
            yield return new WaitForSeconds(0);

        //открываем окно заданий
        TasksTrainScript.IsOn = true;

        while (SceneManager.sceneCount > 1)
            yield return new WaitForSeconds(0);

        i = 11;
        // песец

        ThreatSpawner.CanArouseThreat = true;
        yield return new WaitForSecondsRealtime(5);

        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(GameModel.CurrentThreat,
            Resources.Load<Sprite>("ArcticFox"));

        while (IsWindowActive)
            yield return new WaitForSeconds(0);

        // магазин

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        ShowObject(transform.Find("ShopButton").gameObject,
            transform.Find("ShopButton").GetComponent<Image>().sprite, true);

        while (SceneManager.sceneCount == 1)
            yield return new WaitForSeconds(0);

        ShopTrainScript.IsOn = true;

        while (SceneManager.sceneCount > 1)
            yield return new WaitForSeconds(0);

        // Конец обучения

        i = 16;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);

        while (GameModel.CurrentThreat != null)
            yield return new WaitForSeconds(0);

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        while (IsWindowActive)
            yield return new WaitForSeconds(0);

        i++;
        ShowWindow(lines[i].Split("___")[0], lines[i].Split("___")[1]);
        while (IsWindowActive)
            yield return new WaitForSeconds(0);

        //

        IsOn = false;
        ThreatSpawner.CanArouseThreat = true;
        FoodSpawner.IsWaiting = false;
        WaterSpawner.IsWaiting = false;
    }

    private void ShowObject(GameObject otherObject, Sprite sprite, bool isUI = false)
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