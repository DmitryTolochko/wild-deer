using System;
using System.Transactions;
using Assets;
using Model.Inventory;
using ServiceInstances;
using UnityEngine;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    private TrashCanAssets trashCanAssets;
    private Image image;

    private void Start()
    {
        trashCanAssets = TrashCanAssets.Instance;
        image = GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<ProtectiveCapBehavior>(out var cap))
        {
            return;
        }

        image.sprite = trashCanAssets.trashCanHovered;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        image.sprite = trashCanAssets.trashCanIdle;
    }
}