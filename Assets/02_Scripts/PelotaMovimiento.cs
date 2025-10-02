using UnityEngine;

public class PelotaMovimiento : MonoBehaviour
{
    public float velocidad = 5f;  // Velocidad de la pelota
    private Vector2 direccion;

    void Start()
    {
        // La pelota comienza con una dirección aleatoria (en el eje X la pelota puede ir hacia la izquierda o la derecha)
        direccion = new Vector2(Random.Range(0, 2) == 0 ? 1f : -1f, Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        // Mueve la pelota según la dirección
        transform.Translate(direccion * velocidad * Time.deltaTime);

        // Rebote en los límites superior e inferior (suponiendo que los límites son -5 y 5 en Y)
        if (transform.position.y >= 5f || transform.position.y <= -5f)
        {
            direccion.y = -direccion.y;  // Cambiar la dirección en Y (rebote en el eje Y)
        }

        // Si la pelota pasa por los límites izquierdo o derecho de la pantalla, se reinicia
        if (transform.position.x <= -8f)  // Pasa de jugador 1 (izquierda)
        {
            // Aquí puedes añadir una lógica de puntuación si quieres.
            ReiniciarPelota();  // Reinicia la pelota al centro
        }
        else if (transform.position.x >= 8f)  // Pasa de jugador 2 (derecha)
        {
            // Aquí también puedes añadir una lógica de puntuación si quieres.
            ReiniciarPelota();  // Reinicia la pelota al centro
        }
    }

    // Detecta las colisiones con las raquetas
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Rebotar en X al tocar una raqueta
            direccion.x = -direccion.x;
        }
    }

    // Reinicia la pelota a la posición central
    void ReiniciarPelota()
    {
        transform.position = Vector2.zero;  // Coloca la pelota en el centro
        direccion = new Vector2(Random.Range(0, 2) == 0 ? 1f : -1f, Random.Range(-1f, 1f)).normalized;  // Nueva dirección aleatoria
    }
}
