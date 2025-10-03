using UnityEngine;

namespace PongScore
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        [SerializeField] private string highScoreKey = "HighScore";

        public int P1Score { get; private set; }
        public int P2Score { get; private set; }
        public int HighScore { get; private set; }

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
            TryUpdateHighScore(); // <-- importante
        }

        public void AddPointP2(int points = 1)
        {
            if (points <= 0) return;
            P2Score += points;
            TryUpdateHighScore(); // <-- importante
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
            // opcional ahora (ya guardamos on-the-fly), pero lo dejamos por si llamas al final
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
    }
}
