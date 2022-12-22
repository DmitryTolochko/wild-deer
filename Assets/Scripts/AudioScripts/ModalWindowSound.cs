using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindowSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Button button;

    private void Start() 
    {
        audioSource = GameObject.Find("InventoryUI").GetComponent<AudioSource>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        audioSource.clip = SoundAssets.Instance.ButtonAccept;
        audioSource.Play();
    }
}
