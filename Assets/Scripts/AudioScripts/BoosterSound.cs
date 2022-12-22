using System.Collections;
using System.Collections.Generic;
using Model.Inventory;
using ServiceInstances;
using UnityEngine;

public class BoosterSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("InventoryUI").GetComponent<AudioSource>();
    }

    public void ChangeSoundAndPlay(bool IsFail = false)
    {
        if (!IsFail)
            audioSource.clip = SoundAssets.GetBoosterSound(GetComponent<BoosterWorld>().Type);
        else
            audioSource.clip = SoundAssets.Instance.FailSound;
        audioSource.Play();
    }
}
