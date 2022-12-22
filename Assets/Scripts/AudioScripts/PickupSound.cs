using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start() 
    {
        audioSource = GameObject.Find("InventoryUI").GetComponent<AudioSource>();
    }

    private void OnMouseDown() 
    {
        if (TryGetComponent<FoodWorld>(out var food))
            audioSource.clip = SoundAssets.Instance.FoodPickupSound;
        else
            audioSource.clip = SoundAssets.Instance.WaterPickupSound;

        audioSource.Play();
    }
}
