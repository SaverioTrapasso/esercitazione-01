using UnityEngine;
using Project.Core;
using Project.Gameplay;
using Project.UI;
using UnityEngine.InputSystem;

namespace Project.Editor
{
    /// <summary>
    /// Utility script to set up the scene hierarchy and components from code,
    /// since we don't have access to the Unity Editor GUI.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class SceneSetupUtility : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject duckPrefab;
        public GameObject gunPrefab;

        [ContextMenu("Setup Scene")]
        public void SetupScene()
        {
            // 1. Find or create Hierarchy Folders
            Transform lightFolder = GetOrCreateFolder("## LIGHT");
            Transform staticFolder = GetOrCreateFolder("## STATIC");
            Transform dynamicFolder = GetOrCreateFolder("## DYNAMIC");
            Transform interactiveFolder = GetOrCreateFolder("## INTERACTIVE");
            Transform gameLogicFolder = GetOrCreateFolder("## GAME LOGIC");
            Transform systemFolder = GetOrCreateFolder("## SYSTEM");

            // 2. Setup GameManager
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gm = gmObj.AddComponent<GameManager>();
                gmObj.transform.SetParent(gameLogicFolder);
            }

            // 3. Setup DuckSpawner
            DuckSpawner spawner = FindFirstObjectByType<DuckSpawner>();
            if (spawner == null)
            {
                GameObject spawnerObj = new GameObject("DuckSpawner");
                spawner = spawnerObj.AddComponent<DuckSpawner>();
                spawnerObj.transform.SetParent(gameLogicFolder);
                // Note: duckPrefab and spawnPoints need to be assigned in inspector
            }

            // 4. Setup UIManager
            UIManager ui = FindFirstObjectByType<UIManager>();
            if (ui == null)
            {
                GameObject uiObj = new GameObject("UIManager");
                ui = uiObj.AddComponent<UIManager>();
                uiObj.transform.SetParent(interactiveFolder);
                // Note: UI elements need to be assigned in inspector
            }

            Debug.Log("Scene setup initiated. Please assign serializable references in the Inspector.");
        }

        private Transform GetOrCreateFolder(string name)
        {
            GameObject obj = GameObject.Find(name);
            if (obj == null)
            {
                obj = new GameObject(name);
            }
            return obj.transform;
        }
    }
}
