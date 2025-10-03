using TMPro;
using UnityEngine;

namespace PongScore
{
    /// Muestra "P1: x   P2: y" en un TMP_Text.
    public class ScoreUIBinder : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private string prefixP1 = "P1: ";
        [SerializeField] private string separator = "   ";
        [SerializeField] private string prefixP2 = "P2: ";

        private void Update()
        {
            if (ScoreManager.Instance == null || scoreText == null) return;
            scoreText.text = $"{prefixP1}{ScoreManager.Instance.P1Score}{separator}{prefixP2}{ScoreManager.Instance.P2Score}";
        }
    }
}
