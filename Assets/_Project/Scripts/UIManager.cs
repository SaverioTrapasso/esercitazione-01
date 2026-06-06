using UnityEngine;
using TMPro;
using Project.Core;

namespace Project.UI
{
    /// <summary>
    /// Manages the game UI: score, timer, and state transitions.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private GameObject startButton;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += HandleGameStart;
                GameManager.Instance.OnGameOver += HandleGameOver;
                GameManager.Instance.OnScoreChanged += UpdateScore;
                GameManager.Instance.OnTimerChanged += UpdateTimer;
            }

            ShowIdleUI();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= HandleGameStart;
                GameManager.Instance.OnGameOver -= HandleGameOver;
                GameManager.Instance.OnScoreChanged -= UpdateScore;
                GameManager.Instance.OnTimerChanged -= UpdateTimer;
            }
        }

        private void HandleGameStart()
        {
            startButton.SetActive(false);
            timerText.gameObject.SetActive(true);
        }

        private void HandleGameOver()
        {
            startButton.SetActive(true);
            timerText.gameObject.SetActive(false);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void UpdateTimer(int seconds)
        {
            timerText.text = seconds.ToString();
        }

        private void ShowIdleUI()
        {
            startButton.SetActive(true);
            timerText.gameObject.SetActive(false);
            UpdateScore(0);
        }
    }
}
