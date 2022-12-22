using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Deer deer;

    private void Start()
    {
        audioSource = GameObject.Find("InventoryUI").GetComponent<AudioSource>();
        deer = GetComponent<Deer>();
    }

    private void Update() 
    {
        if (!deer.IsSpawned && deer.DistanceToTarget <= 0.65f)
            ChangeSoundAndPlay();
    }

    public void ChangeSoundAndPlay()
    {
        if (!audioSource.clip.Equals(SoundAssets.Instance.FallSound))
        {
            audioSource.clip = SoundAssets.Instance.FallSound;
            audioSource.Play();
        }
    }
}
