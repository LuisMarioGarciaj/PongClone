using UnityEngine;

public class Marcador : MonoBehaviour
{
    private void Start()
    {
        PongScore.ScoreManager.Instance.ResetScores();
    }

    // Llama esto cuando decidas fin de partida (o al volver al menú)
    public void GuardarRecord()
    {
        PongScore.ScoreManager.Instance.SaveIfHighScore();
    }
}
