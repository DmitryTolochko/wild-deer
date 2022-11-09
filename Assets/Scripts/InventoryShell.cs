using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ServiceInstances;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class InventoryShell : MonoBehaviour
{
    public GameObject inventoryCell;
    private readonly List<GameObject> inventoryCells = new();
    private readonly Vector3 cellOffset = new(2, 0, 0);

    void Start()
    {
        var test = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "InventoryBackground")
            ?.GetComponent<Collider2D>();
        
        inventoryCells.Add(Instantiate(inventoryCell, new Vector3(-5.43f, -4.373f, 0), Quaternion.identity));
        inventoryCells.Add(Instantiate(inventoryCell, inventoryCells.Last().transform.position + cellOffset,
            Quaternion.identity));
        inventoryCells.Add(Instantiate(inventoryCell, inventoryCells.Last().transform.position + cellOffset,
            Quaternion.identity));
    }

    /*public void PlaceBooster(BoosterType type)
    {
        var booster = PoolManager.Instance.GetPoolObject(PoolObjectType.Booster);
        booster.transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(-3f, 3f));
        booster.gameObject.SetActive(true);
        var boosterInstance = booster.GetComponent<Booster>();
        boosterInstance.type = type;
        // boosterPos = count == 0 ? shell.position : last.position 
    }*/
}