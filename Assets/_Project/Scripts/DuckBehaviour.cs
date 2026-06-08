using UnityEngine;
using Project.Core;

namespace Project.Gameplay
{
    /// <summary>
    /// Handles duck behavior: movement, lifetime, and hit detection.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class DuckBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 2f;
        [SerializeField] private Vector3 direction = Vector3.right;
        [SerializeField] private float lifetime = 8f;
        [SerializeField] private int pointsValue = 10;

        [Header("Wing Animation")]
        // SkinnedMeshRenderer of the duck mesh that exposes the wings blendshape.
        [SerializeField] private SkinnedMeshRenderer duckMesh;
        // Oscillation speed of the wings, tweakable from the Inspector.
        [SerializeField] private float wingSpeed = 2f;
        // Blendshape index for the wings (configurable, exact index is unknown).
        [SerializeField] private int wingBlendShapeIndex = 0;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            // Move first so a misconfigured blendshape can never block the duck movement.
            transform.Translate(direction * speed * Time.deltaTime);
            AnimateWings();
        }

        /// <summary>
        /// Animates the wings blendshape in a continuous 0 -> 100 -> 0 loop using a sine wave.
        /// </summary>
        private void AnimateWings()
        {
            if (duckMesh == null || duckMesh.sharedMesh == null) return;

            // Guard against an invalid index so a wrong Inspector value can't throw every frame.
            if (wingBlendShapeIndex < 0 || wingBlendShapeIndex >= duckMesh.sharedMesh.blendShapeCount) return;

            // Map sin (-1..1) to the Unity blendshape range (0..100).
            float weight = (Mathf.Sin(Time.time * wingSpeed) + 1f) * 0.5f * 100f;
            duckMesh.SetBlendShapeWeight(wingBlendShapeIndex, weight);
        }

        /// <summary>
        /// Called when the duck is hit by the gun's raycast.
        /// Adds score and destroys the duck instantly.
        /// </summary>
        public void OnHit()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(pointsValue);
            }

            Destroy(gameObject);
        }
    }
}
