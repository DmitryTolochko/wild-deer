using ServiceInstances;
using UnityEngine;

public class BoostersAssets : MonoBehaviour
{
    public static BoostersAssets Instance { get; private set; }
    public Sprite foodSprite;
    public Sprite pinkTrapSprite;
    public Sprite protectiveCapSprite;

    public Transform boosterPrefab;
    
    private void Awake()
    {
        Instance = this;
    }

    public static Sprite GetSprite(BoosterType type)
    {
        return type switch
        {
            BoosterType.Food => Instance.foodSprite,
            BoosterType.PinkTrap => Instance.pinkTrapSprite,
            BoosterType.ProtectiveCap => Instance.protectiveCapSprite
        };
    }
}