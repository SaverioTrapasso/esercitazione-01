using UnityEngine;
using Project.Core;
using Project.Gameplay;
using Project.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project.Editor
{
    /// <summary>
    /// Utility script to set up the scene hierarchy and components from code.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class SceneSetupUtility : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject duckPrefab;
        public GameObject gunPrefab;

        [Header("UI References (Optional if setting up manually)")]
        public Canvas canvas;
        public TMP_Text scoreText;
        public TMP_Text timerText;
        public Button startButton;

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
            }
            
            if (duckPrefab != null) {
                // Assign via reflection or serialized field if possible
                var prop = spawner.GetType().GetField("duckPrefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (prop != null) prop.SetValue(spawner, duckPrefab);
            }

            // 4. Setup UIManager
            UIManager ui = FindFirstObjectByType<UIManager>();
            if (ui == null)
            {
                GameObject uiObj = new GameObject("UIManager");
                ui = uiObj.AddComponent<UIManager>();
                uiObj.transform.SetParent(interactiveFolder);
            }

            // 5. Setup Gun on Hands
            SetupGunOnHands(interactiveFolder);

            Debug.Log("Scene setup completed. Please verify references in the Inspector.");
        }

        private void SetupGunOnHands(Transform parent)
        {
            // Find Camera Rig anchors (simple search)
            GameObject leftHand = GameObject.Find("LeftHandAnchor");
            GameObject rightHand = GameObject.Find("RightHandAnchor");

            if (leftHand != null && gunPrefab != null) {
                if (leftHand.GetComponentInChildren<Gun>() == null) {
                    GameObject gunObj = Instantiate(gunPrefab, leftHand.transform);
                    gunObj.name = "Gun_Left";
                    if (gunObj.TryGetComponent<Gun>(out var gun))
                    {
                        var field = typeof(Gun).GetField("controllerType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (field != null) field.SetValue(gun, OVRInput.Controller.LTouch);
                    }
                }
            }

            if (rightHand != null && gunPrefab != null) {
                if (rightHand.GetComponentInChildren<Gun>() == null) {
                    GameObject gunObj = Instantiate(gunPrefab, rightHand.transform);
                    gunObj.name = "Gun_Right";
                    if (gunObj.TryGetComponent<Gun>(out var gun))
                    {
                        var field = typeof(Gun).GetField("controllerType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (field != null) field.SetValue(gun, OVRInput.Controller.RTouch);
                    }
                }
            }
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
