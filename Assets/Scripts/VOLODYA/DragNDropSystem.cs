using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace VOLODYA
{
    public class DragNDropSystem : MonoBehaviour
    {
        private Vector3 mousePositionOffset;

        private Vector3 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
        private void OnMouseDown()
        {
            var mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        }

        private void OnMouseDrag()
        {
            transform.position = GetMouseWorldPosition() + mousePositionOffset;
        }
    }
}