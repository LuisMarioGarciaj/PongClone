using UnityEngine;

public class LateralGoalAwarder : MonoBehaviour
{
    public enum Lado { Izquierda, Derecha }

    [Header("¿Qué pared es ESTA?")]
    [SerializeField] private Lado lado = Lado.Derecha;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("Ball")) return;

        // Verificar si el juego ya terminó
        if (PongScore.ScoreManager.Instance == null) return;

        // Si la bola pega en la pared derecha -> punto para P1 (la bola cruzó la portería de P2)
        if (lado == Lado.Derecha)
        {
            PongScore.ScoreManager.Instance.AddPointP1(1);
        }
        else // pared izquierda -> punto para P2
        {
            PongScore.ScoreManager.Instance.AddPointP2(1);
        }
        // Tu PelotaMovimiento ya resetea al detectar "ParedLateral".
    }
}