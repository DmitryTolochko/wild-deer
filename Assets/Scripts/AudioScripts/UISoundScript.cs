using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundScript : MonoBehaviour
{
    private AudioSource audioSource;
    private Button button;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        audioSource.Play();
    }
}
