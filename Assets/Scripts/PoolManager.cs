using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PoolObjectType
{
    Deer, 
    Food, 
    InventoryCell
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject container;
    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}
public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    List<PoolInfo> listOfPool;

    private Vector3 defaultPos = new Vector3(-100, -100, -100);

    void Start()
    {
        for (int i = 0; i < listOfPool.Count; i++)
            FillPool(listOfPool[i]);
    }

    void FillPool(PoolInfo info )
    {
        for (int i = 0; i < info.amount; i++)
        {
            GameObject obInstance = null;
            obInstance = Instantiate(info.prefab, info.container.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            info.pool.Add(obInstance);
        } 
    }

    public GameObject GetPoolObject(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if (pool.Count > 0)
        {
            obInstance = pool[pool.Count -1];
            pool.Remove(obInstance);
        }
        else
            obInstance = Instantiate(selected.prefab, selected.container.transform);

        return obInstance;
    }

    public void CoolObject(GameObject ob, PoolObjectType type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if (!pool.Contains(ob))
            pool.Add(ob);
    }

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            if (type == listOfPool[i].type)
                return listOfPool[i];
        }

        return null;
    }
}
