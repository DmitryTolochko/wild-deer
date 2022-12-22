using System.Collections;
using System.Collections.Generic;
using ServiceInstances;
using UnityEngine;

public class SoundAssets : MonoBehaviour
{
    public static SoundAssets Instance { get; private set; }

    public AudioClip FoodSound;
    public AudioClip MedicinesSound;
    public AudioClip FailSound;
    public AudioClip WaterSound;
    public AudioClip ProtectiveCapSound;
    public AudioClip TrapSound;

    public AudioClip WaterPickupSound;
    public AudioClip FoodPickupSound;
    public AudioClip TrashCanSound;
    public AudioClip FallSound;

    public AudioClip ButtonAccept;

    private void Awake()
    {
        Instance = this;
    }

    public static AudioClip GetBoosterSound(BoosterType type)
    {
        return type switch
        {
            BoosterType.Food => Instance.FoodSound,
            BoosterType.PinkTrap => Instance.TrapSound,
            BoosterType.ProtectiveCap => Instance.ProtectiveCapSound,
            BoosterType.Medicines => Instance.MedicinesSound,
            BoosterType.Water => Instance.WaterSound
        };
    }
}
