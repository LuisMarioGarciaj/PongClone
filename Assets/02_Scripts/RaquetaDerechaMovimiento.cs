using UnityEngine;

public class RaquetaDerechaMovimiento : MonoBehaviour
{
    public float velocidad = 5f;  // Velocidad de movimiento de la raqueta

    void Update()
    {
        float movimiento = 0f;

        // Movimiento hacia arriba y abajo con las teclas de flecha "Arriba" y "Abajo"
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movimiento = 1f;  // Subir
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movimiento = -1f;  // Bajar
        }

        // Mueve la raqueta hacia arriba o abajo
        transform.Translate(0, movimiento * velocidad * Time.deltaTime, 0);
    }
}
