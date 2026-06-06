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

        [Header("Input Actions")]
        [SerializeField] private InputActionProperty grabAction;
        [SerializeField] private InputActionProperty triggerAction;

        private void OnEnable()
        {
            grabAction.action.performed += OnGrabPerformed;
            grabAction.action.canceled += OnGrabCanceled;
            triggerAction.action.performed += OnTriggerPerformed;
        }

        private void OnDisable()
        {
            grabAction.action.performed -= OnGrabPerformed;
            grabAction.action.canceled -= OnGrabCanceled;
            triggerAction.action.performed -= OnTriggerPerformed;
        }

        private void OnGrabPerformed(InputAction.CallbackContext context)
        {
            if (gunMesh != null) gunMesh.enabled = true;
        }

        private void OnGrabCanceled(InputAction.CallbackContext context)
        {
            if (gunMesh != null) gunMesh.enabled = false;
        }

        private void OnTriggerPerformed(InputAction.CallbackContext context)
        {
            // Only fire if the gun is "grabbed" (mesh is enabled)
            if (gunMesh != null && gunMesh.enabled)
            {
                Fire();
            }
        }

        private void Fire()
        {
            Ray ray = new Ray(transform.position, transform.forward);
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
            Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
        }
    }
}
