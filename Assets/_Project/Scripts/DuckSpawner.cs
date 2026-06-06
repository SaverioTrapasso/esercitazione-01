using UnityEngine;
using System.Collections.Generic;
using Project.Core;

namespace Project.Gameplay
{
    /// <summary>
    /// Spawns ducks at random points during gameplay.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class DuckSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject duckPrefab;
        [SerializeField] private Transform[] spawnPoints;

        [Header("Settings")]
        [SerializeField] private float spawnInterval = 2f;

        private float _spawnTimer;
        private List<GameObject> _activeDucks = new List<GameObject>();

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver += DestroyAllDucks;
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver -= DestroyAllDucks;
            }
        }

        private void Update()
        {
            if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnInterval)
            {
                SpawnDuck();
                _spawnTimer = 0;
            }

            // Cleanup null references in the list (ducks destroyed by lifetime or shot)
            _activeDucks.RemoveAll(duck => duck == null);
        }

        private void SpawnDuck()
        {
            if (spawnPoints == null || spawnPoints.Length == 0 || duckPrefab == null) return;

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject duck = Instantiate(duckPrefab, spawnPoint.position, spawnPoint.rotation);
            _activeDucks.Add(duck);
        }

        private void DestroyAllDucks()
        {
            foreach (var duck in _activeDucks)
            {
                if (duck != null)
                {
                    Destroy(duck);
                }
            }
            _activeDucks.Clear();
        }
    }
}
