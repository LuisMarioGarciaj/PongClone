using UnityEngine;
using System.Collections;

public class PelotaMovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private bool esperandoReinicio = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
        LanzarPelota();
    }

    void LanzarPelota()
    {
        float direccionX = Random.Range(0, 2) == 0 ? -1f : 1f;
        float direccionY = Random.Range(-1f, 1f);

        Vector2 direccion = new Vector2(direccionX, direccionY).normalized;
        rb.velocity = direccion * velocidad;
    }

    void Update()
    {
        if (!esperandoReinicio)
        {
            rb.velocity = rb.velocity.normalized * velocidad;
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
            float nuevaDireccionX = rb.velocity.x > 0 ? -1f : 1f;
            float nuevaDireccionY = Random.Range(-0.5f, 0.5f);
            Vector2 nuevaDireccion = new Vector2(nuevaDireccionX, nuevaDireccionY).normalized;
            rb.velocity = nuevaDireccion * velocidad;
        }
        else if (collision.gameObject.CompareTag("Pared"))
        {
            float nuevaDireccionY = -rb.velocity.y;
            float nuevaDireccionX = rb.velocity.x + Random.Range(-0.3f, 0.3f);
            Vector2 nuevaDireccion = new Vector2(nuevaDireccionX, nuevaDireccionY).normalized;
            rb.velocity = nuevaDireccion * velocidad;
        }
        else if (collision.gameObject.CompareTag("ParedLateral"))
        {
            StartCoroutine(ReiniciarPelota());
        }
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
