using UnityEngine;

public class RaquetaIzquierdaMovimiento : MonoBehaviour
{
    public float velocidad = 5f;  // Velocidad de movimiento de la raqueta

    void Update()
    {
        float movimiento = 0f;

        // Movimiento hacia arriba y abajo con las teclas "W" y "S"
        if (Input.GetKey(KeyCode.W))
        {
            movimiento = 1f;  // Subir
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movimiento = -1f;  // Bajar
        }

        // Mueve la raqueta hacia arriba o abajo
        transform.Translate(0, movimiento * velocidad * Time.deltaTime, 0);
    }
}
