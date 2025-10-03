using UnityEngine;
using UnityEngine.SceneManagement;

namespace PongScore
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [SerializeField] private string highScoreKey = "HighScore";
        [SerializeField] private int puntosParaGanar = 5; // Configura cuántos puntos necesita un jugador para ganar

        public int P1Score { get; private set; }
        public int P2Score { get; private set; }
        public int HighScore { get; private set; }

        // Eventos para notificar cuando hay un ganador
        public System.Action<int> OnGameOver; // Parámetro: 1 para P1, 2 para P2

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }

        public void ResetScores()
        {
            P1Score = 0;
            P2Score = 0;
        }

        public void AddPointP1(int points = 1)
        {
            if (points <= 0) return;
            P1Score += points;
            TryUpdateHighScore();
            CheckForWinner();
        }

        public void AddPointP2(int points = 1)
        {
            if (points <= 0) return;
            P2Score += points;
            TryUpdateHighScore();
            CheckForWinner();
        }

        private void CheckForWinner()
        {
            if (P1Score >= puntosParaGanar)
            {
                // P1 gana
                OnGameOver?.Invoke(1);
                LoadGameOverScene();
            }
            else if (P2Score >= puntosParaGanar)
            {
                // P2 gana
                OnGameOver?.Invoke(2);
                LoadGameOverScene();
            }
        }

        private void LoadGameOverScene()
        {
            // Guardar el récord antes de cambiar de escena
            SaveIfHighScore();

            // Cargar la escena de Game Over
            SceneManager.LoadScene("GameOver");
        }

        /// Guarda el mayor de P1/P2 si supera el récord actual
        private void TryUpdateHighScore()
        {
            int bestThisMatch = Mathf.Max(P1Score, P2Score);
            if (bestThisMatch > HighScore)
            {
                HighScore = bestThisMatch;
                PlayerPrefs.SetInt(highScoreKey, HighScore);
                PlayerPrefs.Save();
            }
        }

        public void SaveIfHighScore()
        {
            TryUpdateHighScore();
        }

        public void ClearHighScore()
        {
            HighScore = 0;
            PlayerPrefs.SetInt(highScoreKey, 0);
            PlayerPrefs.Save();
        }

        public void LoadHighScore()
        {
            HighScore = PlayerPrefs.GetInt(highScoreKey, 0);
        }

        // Método para configurar los puntos necesarios para ganar
        public void SetPuntosParaGanar(int puntos)
        {
            puntosParaGanar = puntos;
        }
    }
}