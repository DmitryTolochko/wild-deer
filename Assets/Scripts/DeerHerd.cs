using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeerHerd : MonoBehaviour
{
    public List<GameObject> deers = new();
    public GameObject deer;

    void Start()
    {
    }

    void Update()
    {
        if (GameObject.FindWithTag("Dead") != null)
            GameObject.Destroy(GameObject.FindWithTag("Dead"));
    }
    private void OnMouseDown() 
    {
        Instantiate(deer, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
