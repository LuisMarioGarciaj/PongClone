using UnityEngine;

public class RaquetaDerechaMovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private float movimiento;

    public float limiteSuperior = 4.5f;  // Ajusta según el tamaño de la pantalla y la raqueta
    public float limiteInferior = -4.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimiento = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            movimiento = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movimiento = -1f;
        }
    }

    void FixedUpdate()
    {
        Vector2 nuevaPos = rb.position + Vector2.up * movimiento * velocidad * Time.fixedDeltaTime;

        // Limita la posición para que no se salga de la pantalla (o se atraviese las paredes)
        nuevaPos.y = Mathf.Clamp(nuevaPos.y, limiteInferior, limiteSuperior);

        rb.MovePosition(nuevaPos);
    }
}
