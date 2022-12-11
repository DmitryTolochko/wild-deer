using UnityEngine;

namespace Assets
{
    public class TrashCanAssets : MonoBehaviour
    {
        public static TrashCanAssets Instance { get; private set; }
        public Sprite trashCanIdle;
        public Sprite trashCanHovered;

        public Transform trashCanPrefab;

        private void Awake()
        {
            Instance = this;
        }
    }
}