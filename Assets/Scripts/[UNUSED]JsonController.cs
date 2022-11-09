/*using UnityEngine;
using System.IO;

namespace DefaultNamespace
{
    public static class JsonController
    {
        [ContextMenu("Load")]
        public static GameModel Load()
        {
            return JsonUtility
                .FromJson<GameModel>(File.ReadAllText($"{Application.streamingAssetsPath}/progress.json"));
        }

        [ContextMenu("Save")]
        public static void Save(GameModel game)
        {
            File.WriteAllText($"{Application.streamingAssetsPath}/progress.json", JsonUtility.ToJson(game));
        }
    }
}*/