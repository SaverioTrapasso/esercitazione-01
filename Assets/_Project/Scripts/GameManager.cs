using UnityEngine;
using System;

namespace Project.Core
{
    public enum GameState
    {
        Idle,
        Playing,
        GameOver
    }

    /// <summary>
    /// Manages the game state, timer, and score.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private int gameDuration = 60;

        public event Action OnGameStart;
        public event Action OnGameOver;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnTimerChanged;

        private GameState _currentState = GameState.Idle;
        private int _score;
        private float _timer;

        public bool IsPlaying => _currentState == GameState.Playing;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            ResetGame();
        }

        private void Update()
        {
            if (_currentState == GameState.Playing)
            {
                UpdateTimer();
            }
        }

        public void StartGame()
        {
            if (_currentState == GameState.Playing) return;

            _score = 0;
            _timer = gameDuration;
            _currentState = GameState.Playing;

            OnScoreChanged?.Invoke(_score);
            OnTimerChanged?.Invoke((int)_timer);
            OnGameStart?.Invoke();
        }

        public void AddScore(int points)
        {
            if (!IsPlaying) return;

            _score += points;
            OnScoreChanged?.Invoke(_score);
        }

        private void UpdateTimer()
        {
            _timer -= Time.deltaTime;
            OnTimerChanged?.Invoke(Mathf.CeilToInt(_timer));

            if (_timer <= 0)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            _timer = 0;
            _currentState = GameState.GameOver;
            OnGameOver?.Invoke();
        }

        private void ResetGame()
        {
            _score = 0;
            _timer = gameDuration;
            _currentState = GameState.Idle;
            OnScoreChanged?.Invoke(_score);
            OnTimerChanged?.Invoke((int)_timer);
        }
    }
}
