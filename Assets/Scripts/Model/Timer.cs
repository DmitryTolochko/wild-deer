using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timerText;
    private Text bestTimerText;

    private Stopwatch stopwatch = new Stopwatch();

    private void Start() 
    {
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        bestTimerText = GameObject.Find("BestTime").GetComponent<Text>();
        var mins = GameModel.bestTime.Minutes;
        var sex = GameModel.bestTime.Seconds;
        bestTimerText.text = $"{(mins < 10 ? "0" : "")}{mins}:{(sex < 10 ? "0" : "")}{sex}";
        stopwatch.Start();
    }

    private void Update() 
    {
        var sex = stopwatch.Elapsed.Seconds;
        var mins = stopwatch.Elapsed.Minutes;
        timerText.text = $"{(mins < 10 ? "0" : "")}{mins}:{(sex < 10 ? "0" : "")}{sex}";
    }
}
