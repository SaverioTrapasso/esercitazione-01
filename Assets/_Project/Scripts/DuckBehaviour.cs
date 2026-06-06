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

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        /// <summary>
        /// Called when the duck is hit by the gun's raycast.
        /// </summary>
        public void OnHit()
        {
            GameManager.Instance.AddScore(pointsValue);
            Destroy(gameObject);
        }
    }
}
