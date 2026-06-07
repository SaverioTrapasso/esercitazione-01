using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Project.Core;

namespace Project.Gameplay
{
    /// <summary>
    /// Handles the gun behavior: raycasting for hits on ducks and UI.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class Gun : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float maxDistance = 50f;
        [SerializeField] private LayerMask hitLayers;
        [SerializeField] private MeshRenderer gunMesh;
        [SerializeField] private Transform muzzleTransform;
        [SerializeField] private OVRInput.Controller controllerType = OVRInput.Controller.RTouch;

        private void Update()
        {
            // Visibility: enable mesh only if controller is connected
            bool isControllerConnected = OVRInput.IsControllerConnected(controllerType);
            if (gunMesh != null && gunMesh.enabled != isControllerConnected)
            {
                gunMesh.enabled = isControllerConnected;
            }

            // Input: check for trigger press
            if (isControllerConnected && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controllerType))
            {
                Fire();
            }
        }

        private void Fire()
        {
            if (GameManager.Instance == null) return;

            Transform spawnPoint = muzzleTransform != null ? muzzleTransform : transform;
            Ray ray = new Ray(spawnPoint.position, spawnPoint.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitLayers))
            {
                // Check if we hit a Duck
                if (hit.collider.CompareTag("Duck"))
                {
                    if (hit.collider.TryGetComponent<DuckBehaviour>(out var duck))
                    {
                        duck.OnHit();
                    }
                }
                // Check if we hit a UI Button (Start Button)
                else if (hit.collider.TryGetComponent<Button>(out var button))
                {
                    button.onClick.Invoke();
                }
            }
        }

        // Draw the ray in the editor for debugging
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Transform spawnPoint = muzzleTransform != null ? muzzleTransform : transform;
            Gizmos.DrawRay(spawnPoint.position, spawnPoint.forward * maxDistance);
        }
    }
}
