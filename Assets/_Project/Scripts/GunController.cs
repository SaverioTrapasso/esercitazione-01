using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Project.Core;

namespace Project.Gameplay
{
    /// <summary>
    /// Handles both gun instances (right and left hand) using pure OVRInput.
    /// Grab (PrimaryHandTrigger) shows the gun mesh and tracks the hand anchor.
    /// Index trigger (while grabbing) fires a raycast from the muzzle.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class GunController : MonoBehaviour
    {
        [Header("Hand Anchors")]
        [SerializeField] private Transform rightHandAnchor;
        [SerializeField] private Transform leftHandAnchor;

        [Header("Gun Meshes")]
        [SerializeField] private MeshRenderer gunMeshRight;
        [SerializeField] private MeshRenderer gunMeshLeft;

        [Header("Muzzles")]
        [SerializeField] private Transform muzzleRight;
        [SerializeField] private Transform muzzleLeft;

        [Header("Settings")]
        [SerializeField] private float maxDistance = 50f;

        [Header("VFX")]
        // Visual effect instantiated at the raycast impact point (Duck or any surface).
        [SerializeField] private VisualEffect impactVFXPrefab;

        private void Update()
        {
            // Handle each hand independently using pure OVRInput.
            HandleHand(OVRInput.Controller.RTouch, rightHandAnchor, gunMeshRight, muzzleRight);
            HandleHand(OVRInput.Controller.LTouch, leftHandAnchor, gunMeshLeft, muzzleLeft);
        }

        /// <summary>
        /// Manages visibility, tracking and firing for a single hand.
        /// </summary>
        private void HandleHand(OVRInput.Controller controller, Transform anchor, MeshRenderer gunMesh, Transform muzzle)
        {
            // Grab held down -> gun visible and tracking the hand anchor.
            bool isGrabbing = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controller);

            if (gunMesh != null)
            {
                if (gunMesh.enabled != isGrabbing)
                {
                    gunMesh.enabled = isGrabbing;
                }

                // Keep the gun aligned with the hand anchor while grabbing.
                // Move the Offset parent (not the mesh itself) so the mesh's
                // local rotation/position offset configured in the Inspector is preserved.
                //if (isGrabbing && anchor != null && gunMesh.transform.parent != null)
                //{
                //    gunMesh.transform.parent.position = anchor.position;
                //    gunMesh.transform.parent.rotation = anchor.rotation;
                //}
            }

            // Fire only while grabbing and on index trigger down.
            if (isGrabbing && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                Fire(muzzle);
            }
        }

        /// <summary>
        /// Performs an instant raycast forward from the given muzzle.
        /// </summary>
        private void Fire(Transform muzzle)
        {
            if (muzzle == null) return;

            Ray ray = new Ray(muzzle.position, muzzle.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                // Hit a Duck -> add score.
                if (hit.collider.CompareTag("Duck"))
                {
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddScore(10);
                    }
                }
                // Hit a UI Button -> invoke its click.
                else if (hit.collider.TryGetComponent<Button>(out var button))
                {
                    button.onClick.Invoke();
                }

                // Spawn the impact VFX on any hit (Duck or any other surface)
                // so the player always gets visual feedback.
                SpawnImpactVFX(hit.point, hit.normal);
            }
        }

        /// <summary>
        /// Instantiates the impact VFX oriented along the surface normal
        /// and destroys it shortly after the burst.
        /// </summary>
        private void SpawnImpactVFX(Vector3 point, Vector3 normal)
        {
            if (impactVFXPrefab != null)
            {
                var vfx = Instantiate(impactVFXPrefab, point, Quaternion.LookRotation(normal));
                Destroy(vfx.gameObject, 1f);
            }
        }
    }
}
