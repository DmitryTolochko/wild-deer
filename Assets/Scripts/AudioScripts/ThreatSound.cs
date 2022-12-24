using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatSound : MonoBehaviour
{
    private AudioSource audioSource;
    private BaseThreat threat;
    private bool IsOn;

    private void Start()
    {
        audioSource = GameObject.Find("FallSoundSystem").GetComponent<AudioSource>();
        threat = GetComponent<BaseThreat>();
    }

    private void Update() 
    {
        if (threat.Status == ThreatStatus.Spawning)
            IsOn = true;
        if (IsOn && threat.DistanceToTarget <= 0.65f && !audioSource.isPlaying)
        {
            audioSource.Play();
            IsOn = false;
        }
    }
}