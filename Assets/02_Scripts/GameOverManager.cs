using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textoGanador;
    [SerializeField] private TMP_Text textoPuntajeFinal;
    [SerializeField] private string escenaMenuPrincipal = "MainMenu";
    [SerializeField] private string escenaJuego = "Juego"; // Ajusta al nombre de tu escena de juego

    void Start()
    {
        MostrarResultado();
    }

    void MostrarResultado()
    {
        if (PongScore.ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager no encontrado!");
            return;
        }

        int puntajeP1 = PongScore.ScoreManager.Instance.P1Score;
        int puntajeP2 = PongScore.ScoreManager.Instance.P2Score;

        // Determinar ganador
        if (puntajeP1 > puntajeP2)
        {
            textoGanador.text = "¡JUGADOR 1 GANA!";
        }
        else if (puntajeP2 > puntajeP1)
        {
            textoGanador.text = "¡JUGADOR 2 GANA!";
        }
        else
        {
            textoGanador.text = "¡EMPATE!";
        }

        textoPuntajeFinal.text = $"Puntaje Final: {puntajeP1} - {puntajeP2}";

        // Guardar récord si es necesario
        PongScore.ScoreManager.Instance.SaveIfHighScore();
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(escenaMenuPrincipal);
    }

    public void JugarDeNuevo()
    {
        // Reiniciar puntajes antes de volver a jugar
        if (PongScore.ScoreManager.Instance != null)
        {
            PongScore.ScoreManager.Instance.ResetScores();
        }
        SceneManager.LoadScene(escenaJuego);
    }

    public void SalirDelJuego()
    {
        Application.Quit();

        // Para testing en el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}