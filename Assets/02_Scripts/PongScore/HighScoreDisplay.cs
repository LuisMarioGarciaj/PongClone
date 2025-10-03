using TMPro;
using UnityEngine;

namespace PongScore
{
    public class HighScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private string prefix = "High Score: ";

        private void Update()
        {
            if (highScoreText == null) return;

            int hs = 0;
            if (ScoreManager.Instance != null)
                hs = ScoreManager.Instance.HighScore;
            else
                hs = PlayerPrefs.GetInt("HighScore", 0); // robusto si aún no existe el manager

            highScoreText.text = prefix + hs.ToString();
        }
    }
}
