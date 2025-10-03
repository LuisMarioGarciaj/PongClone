using UnityEngine;

public class PaddleAutoMove : MonoBehaviour
{
    [Header("Referencias")]
    public SpriteRenderer playfield; // arrastra aquí el borde/blanco que define la cancha

    [Header("Animación")]
    public float verticalMargin = 0.2f; // margen desde los bordes superior/inferior (en unidades de mundo)
    public float speed = 1.5f;          // ciclos por segundo aprox.
    public float phaseOffset = 0f;      // para desfasar paletas (p. ej. 0 y 1.2 rad)

    private float _minY, _maxY;
    private float _centerX;
    private Vector3 _startPos;

    void Awake()
    {
        _startPos = transform.position;
    }

    void Start()
    {
        if (playfield == null)
        {
            Debug.LogError("[PaddleAutoMove] Asigna el SpriteRenderer del playfield");
            enabled = false; return;
        }

        // usa bounds reales del sprite en mundo (independiente de la escala del padre)
        var b = playfield.bounds;
        _minY = b.min.y + verticalMargin;
        _maxY = b.max.y - verticalMargin;

        // conserva X/Z originales y mueve solo Y
        _centerX = transform.position.x;

        // pequeño desfase aleatorio opcional
        phaseOffset += Random.Range(0f, Mathf.PI);
    }

    void Update()
    {
        // t oscila 0..1 con seno
        float t = (Mathf.Sin((Time.time * speed) + phaseOffset) + 1f) * 0.5f;
        float y = Mathf.Lerp(_minY, _maxY, t);
        transform.position = new Vector3(_centerX, y, _startPos.z);
    }
}
