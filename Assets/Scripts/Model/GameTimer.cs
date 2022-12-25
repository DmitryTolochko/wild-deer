using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private Text timerText;
    private Text bestTimerText;

    public static Stopwatch Stopwatch = new Stopwatch();

    private void Start() 
    {
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        bestTimerText = GameObject.Find("BestTime").GetComponent<Text>();
        Stopwatch.Start();
    }

    private void Update() 
    {
        var sex = Stopwatch.Elapsed.Seconds;
        var mins = Stopwatch.Elapsed.Minutes;
        timerText.text = $"{(mins < 10 ? "0" : "")}{mins}:{(sex < 10 ? "0" : "")}{sex}";

        mins = GameModel.bestTime.Minutes;
        sex = GameModel.bestTime.Seconds;
        bestTimerText.text = $"{(mins < 10 ? "0" : "")}{mins}:{(sex < 10 ? "0" : "")}{sex}";
    }
}
