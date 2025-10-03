using TMPro;
using UnityEngine;

namespace PongScore
{
    public class MenuHighScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private string prefix = "High Score: ";

        private void OnEnable()
        {
            int hs = (ScoreManager.Instance != null)
                ? ScoreManager.Instance.HighScore
                : PlayerPrefs.GetInt("HighScore", 0);

            if (highScoreText != null)
                highScoreText.text = prefix + hs.ToString();
        }
    }
}
