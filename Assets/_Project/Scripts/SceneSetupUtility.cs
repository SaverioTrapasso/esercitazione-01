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
        public GameObject headerScorePanel;
        public GameObject contentScorePanel;
        public GameObject headerTimerPanel;
        public GameObject contentTimerPanel;
        public GameObject startButtonObj;
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
                SetPrivateField(spawner, "duckPrefab", duckPrefab);
            }

            // 4. Setup UIManager
            UIManager ui = FindFirstObjectByType<UIManager>();
            if (ui == null)
            {
                GameObject uiObj = new GameObject("UIManager");
                ui = uiObj.AddComponent<UIManager>();
                uiObj.transform.SetParent(interactiveFolder);
            }

            // Assign UI references if available
            if (ui != null)
            {
                if (headerScorePanel != null) SetPrivateField(ui, "headerScorePanel", headerScorePanel);
                if (contentScorePanel != null) SetPrivateField(ui, "contentScorePanel", contentScorePanel);
                if (headerTimerPanel != null) SetPrivateField(ui, "headerTimerPanel", headerTimerPanel);
                if (contentTimerPanel != null) SetPrivateField(ui, "contentTimerPanel", contentTimerPanel);
                if (startButtonObj != null) SetPrivateField(ui, "startButton", startButtonObj);
                if (scoreText != null) SetPrivateField(ui, "scoreText", scoreText);
                if (timerText != null) SetPrivateField(ui, "timerText", timerText);
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
                        SetPrivateField(gun, "controllerType", OVRInput.Controller.LTouch);
                    }
                }
            }

            if (rightHand != null && gunPrefab != null) {
                if (rightHand.GetComponentInChildren<Gun>() == null) {
                    GameObject gunObj = Instantiate(gunPrefab, rightHand.transform);
                    gunObj.name = "Gun_Right";
                    if (gunObj.TryGetComponent<Gun>(out var gun))
                    {
                        SetPrivateField(gun, "controllerType", OVRInput.Controller.RTouch);
                    }
                }
            }
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null) field.SetValue(obj, value);
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
