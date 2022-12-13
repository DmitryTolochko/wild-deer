using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationScript : MonoBehaviour
{
    public GameObject Notification;
    public static bool IsHidden = true;

    private void Update() 
    {
        foreach (var task in GameModel.ActualTasks)
        {
            if (task.DoneCount == task.Count)
            {
                IsHidden = false;
                break;
            }
            else
                IsHidden = true;  
        }

        if (IsHidden)
            Notification.SetActive(false);
        else
            Notification.SetActive(true);
    }


}
