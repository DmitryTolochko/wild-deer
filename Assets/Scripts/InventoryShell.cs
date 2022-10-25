using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class InventoryShell : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly List<GameObject> inventoryCells = new();
    public GameObject inventoryCell;
    private readonly Vector3 cellOffset = new(2, 0,0);
    void Start()
    {
        inventoryCells.Add(Instantiate(inventoryCell, new Vector3(-5.43f, -4.373f, 0), Quaternion.identity));
        inventoryCells.Add(Instantiate(inventoryCell, inventoryCells.Last().transform.position + cellOffset, Quaternion.identity));
        inventoryCells.Add(Instantiate(inventoryCell, inventoryCells.Last().transform.position + cellOffset, Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
