using UnityEngine;
using System.Collections;

public class PelotaMovimiento : MonoBehaviour
{
    public float velocidadInicial = 7f;
    public float incrementoVelocidad = 0.5f;
    public float velocidadMaxima = 17f;

    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private bool esperandoReinicio = false;
    private float velocidadActual;

    [SerializeField]
    AudioSource sound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
        velocidadActual = velocidadInicial;
        LanzarPelota();
    }

    void LanzarPelota()
    {
        float direccionX = Random.Range(0, 2) == 0 ? -1f : 1f;
        float direccionY = Random.Range(-1f, 1f);

        Vector2 direccion = new Vector2(direccionX, direccionY).normalized;
        rb.velocity = direccion * velocidadActual;
    }

    void Update()
    {
        if (!esperandoReinicio)
        {
            // Mantener la velocidad constante pero con el valor actualizado
            rb.velocity = rb.velocity.normalized * velocidadActual;
        }
        else
        {
            rb.velocity = Vector2.zero; // Detener movimiento mientras espera reinicio
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (esperandoReinicio) return; // Ignorar colisiones mientras reinicia

        if (collision.gameObject.CompareTag("Player"))
        {
            sound.Play();

            // Aumentar la velocidad cuando golpea una raqueta
            AumentarVelocidad();

            float nuevaDireccionX = rb.velocity.x > 0 ? -1f : 1f;
            float nuevaDireccionY = Random.Range(-0.5f, 0.5f);
            Vector2 nuevaDireccion = new Vector2(nuevaDireccionX, nuevaDireccionY).normalized;
            rb.velocity = nuevaDireccion * velocidadActual;
        }
        else if (collision.gameObject.CompareTag("Pared"))
        {
            sound.Play();
            float nuevaDireccionY = -rb.velocity.y;
            float nuevaDireccionX = rb.velocity.x + Random.Range(-0.3f, 0.3f);
            Vector2 nuevaDireccion = new Vector2(nuevaDireccionX, nuevaDireccionY).normalized;
            rb.velocity = nuevaDireccion * velocidadActual;
        }
        else if (collision.gameObject.CompareTag("ParedLateral"))
        {
            sound.Play();
            // Reiniciar la velocidad cuando se anota un punto
            velocidadActual = velocidadInicial;
            StartCoroutine(ReiniciarPelota());
        }
    }

    void AumentarVelocidad()
    {
        // Aumentar la velocidad sin superar el máximo
        velocidadActual = Mathf.Min(velocidadActual + incrementoVelocidad, velocidadMaxima);

        // Opcional: mostrar en consola para debugging
        Debug.Log($"Velocidad actual: {velocidadActual}");
    }

    IEnumerator ReiniciarPelota()
    {
        esperandoReinicio = true;
        rb.velocity = Vector2.zero;
        transform.position = posicionInicial;

        yield return new WaitForSeconds(1f);

        LanzarPelota();
        esperandoReinicio = false;
    }
}