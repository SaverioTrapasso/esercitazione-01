using UnityEngine;
using TMPro;
using Project.Core;

namespace Project.UI
{
    /// <summary>
    /// Manages the game UI: score, timer, and state transitions based on game state.
    /// Assisted by: Junie (JetBrains)
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject headerScorePanel;
        [SerializeField] private GameObject contentScorePanel;
        [SerializeField] private GameObject headerTimerPanel;
        [SerializeField] private GameObject contentTimerPanel;
        [SerializeField] private GameObject startButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;

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
            SetPlayingUI();
        }

        private void HandleGameOver()
        {
            SetGameOverUI();
        }

        private void UpdateScore(int score)
        {
            if (scoreText != null)
                scoreText.text = score.ToString();
        }

        private void UpdateTimer(int seconds)
        {
            if (timerText != null)
                timerText.text = seconds.ToString();
        }

        private void ShowIdleUI()
        {
            headerScorePanel.SetActive(true);
            contentScorePanel.SetActive(true);
            headerTimerPanel.SetActive(false);
            contentTimerPanel.SetActive(false);
            startButton.SetActive(true);
            
            UpdateScore(0);
        }

        private void SetPlayingUI()
        {
            headerScorePanel.SetActive(true);
            contentScorePanel.SetActive(true);
            headerTimerPanel.SetActive(true);
            contentTimerPanel.SetActive(true);
            startButton.SetActive(false);
        }

        private void SetGameOverUI()
        {
            headerScorePanel.SetActive(true);
            contentScorePanel.SetActive(true);
            headerTimerPanel.SetActive(false);
            contentTimerPanel.SetActive(false);
            startButton.SetActive(true);
        }
    }
}
