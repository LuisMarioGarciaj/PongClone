using UnityEngine;

public class BallMenuBouncePro : MonoBehaviour
{
    [Header("Playfield (SpriteRenderer que cubre TODO)")]
    public SpriteRenderer playfield;
    [Range(0f, 0.5f)] public float margin = 0.12f;

    [Header("Velocidad (constante)")]
    public float startSpeed = 60f;      // súbelo/bájalo según tu escala
    public bool autoCalibrate = true;   // ajusta velocidad al ancho del campo
    public float secondsToCrossWidth = 1.4f;
    public float maxStep = 3f;          // paso máximo por frame (según escala)

    [Header("Ángulos")]
    [Tooltip("Desvío aleatorio muy leve al rebotar en PARED")]
    [Range(0f, 6f)] public float randomAngleOnWall = 2f;

    [Tooltip("Ángulo mínimo respecto a la horizontal (evita línea casi horizontal)")]
    [Range(0f, 25f)] public float minDegFromHorizontal = 8f;

    [Tooltip("Ángulo máximo respecto a la horizontal (evita quedar casi vertical)")]
    [Range(35f, 85f)] public float maxDegFromHorizontal = 70f;

    [Header("Impact FX (opcionales, puedes dejarlos vacíos)")]
    public SpriteRenderer ballSR;
    public Color hitFlash = new Color(1f, 0.95f, 0.7f, 1f);
    public float flashTime = 0.05f;
    [Range(0f, 0.5f)] public float squashAmount = 0.12f;
    public float squashRecover = 14f;

    [Header("Skin/Escape (anti-pegarse)")]
    public bool useSkinEscape = true;
    public float skinFactor = 0.0012f;
    public float minEscapeMultiplier = 2f;

    // Paletas (solo rebote por la cara interior)
    [System.Serializable]
    public class PaddleDef
    {
        public Transform paddle;                // Transform de la paleta
        public bool isLeft = true;              // true = izquierda, false = derecha
        public SpriteRenderer spriteForSize;    // para altura real
        public float extraYMargin = 0.05f;      // tolerancia vertical
        [Tooltip("Si no asignas spriteForSize, usa este medio alto manual")]
        public float manualHalfHeight = 1f;
        [Tooltip("Usar bounds para el borde interior en vez de 'thickness'")]
        public bool useSpriteBoundsForFace = true;
        [Tooltip("Solo si NO usas bounds: grosor de la paleta")]
        public float thickness = 0.15f;
    }
    public PaddleDef[] paddles;

    // Internos
    private Vector2 dir;
    private float speed;
    private Bounds pbounds;
    private Vector3 defaultScale;
    private Color baseColor;
    private float squashT;

    void Reset() { ballSR = GetComponentInChildren<SpriteRenderer>(); }
    void Awake() { Time.timeScale = 1f; }

    void Start()
    {
        if (!playfield) { enabled = false; return; }
        pbounds = playfield.bounds;

        // Dirección inicial no-perfecta, pero claramente hacia izquierda o derecha
        float ang = Random.Range(-15f, 15f);
        dir = Quaternion.Euler(0, 0, ang) * (Random.value < 0.5f ? Vector2.right : Vector2.left);
        dir = ClampAngle(dir.normalized);

        // Velocidad constante (con auto-calibrado)
        speed = startSpeed;
        if (autoCalibrate)
        {
            float width = Mathf.Max(0.01f, pbounds.size.x - (margin * 2f));
            float target = Mathf.Max(0.25f, secondsToCrossWidth);
            speed = Mathf.Max(startSpeed, width / target);
            maxStep = Mathf.Max(maxStep, width * 0.10f);
        }

        defaultScale = transform.localScale == Vector3.zero ? Vector3.one : transform.localScale;
        if (ballSR) baseColor = ballSR.color;
    }

    void Update()
    {
        float dt = Mathf.Min(Time.deltaTime, 0.033f);
        float remaining = Mathf.Min(speed * dt, maxStep);

        for (int iter = 0; iter < 3 && remaining > 1e-5f; iter++)
        {
            Vector2 pos = transform.position;
            Vector2 next = pos + dir * remaining;

            float minX = pbounds.min.x + margin, maxX = pbounds.max.x - margin;
            float minY = pbounds.min.y + margin, maxY = pbounds.max.y - margin;

            float tHit = 1f;
            Vector2 normal = Vector2.zero;
            bool hitPaddle = false;

            // 1) Paredes del campo
            if (dir.x > 0f && next.x > maxX) { float t = (maxX - pos.x) / (next.x - pos.x); if (t >= 0f && t < tHit) { tHit = t; normal = Vector2.right; } }
            if (dir.x < 0f && next.x < minX) { float t = (minX - pos.x) / (next.x - pos.x); if (t >= 0f && t < tHit) { tHit = t; normal = Vector2.left; } }
            if (dir.y > 0f && next.y > maxY) { float t = (maxY - pos.y) / (next.y - pos.y); if (t >= 0f && t < tHit) { tHit = t; normal = Vector2.down; } }
            if (dir.y < 0f && next.y < minY) { float t = (minY - pos.y) / (next.y - pos.y); if (t >= 0f && t < tHit) { tHit = t; normal = Vector2.up; } }

            // 2) Paletas (cara interior únicamente)
            if (paddles != null)
            {
                for (int i = 0; i < paddles.Length; i++)
                {
                    var p = paddles[i];
                    if (p == null || p.paddle == null) continue;

                    // ¿Dirección hacia la cara interior?
                    if (p.isLeft && dir.x <= 0f) continue;   // izquierda: debe ir +X
                    if (!p.isLeft && dir.x >= 0f) continue;  // derecha: debe ir -X

                    // Borde interior X de la paleta
                    float innerX;
                    if (p.useSpriteBoundsForFace && p.spriteForSize)
                    {
                        var rb = p.spriteForSize.bounds;
                        innerX = p.isLeft ? rb.max.x : rb.min.x; // cara interna hacia el centro
                    }
                    else
                    {
                        innerX = p.paddle.position.x + (p.isLeft ? +p.thickness * 0.5f : -p.thickness * 0.5f);
                    }

                    float denom = (next.x - pos.x);
                    if (Mathf.Abs(denom) < 1e-6f) continue;
                    float t = (innerX - pos.x) / denom;
                    if (t < 0f || t >= tHit) continue; // solo si es el choque más cercano

                    // Y del impacto
                    float yAt = pos.y + (next.y - pos.y) * t;
                    float halfH = p.spriteForSize ? p.spriteForSize.bounds.extents.y : p.manualHalfHeight;
                    float minPy = p.paddle.position.y - halfH - p.extraYMargin;
                    float maxPy = p.paddle.position.y + halfH + p.extraYMargin;
                    if (yAt < minPy || yAt > maxPy) continue;

                    // Es impacto válido en cara interna
                    tHit = t;
                    normal = p.isLeft ? Vector2.right : Vector2.left;
                    hitPaddle = true;
                }
            }

            if (tHit < 1f) // hubo impacto
            {
                Vector2 hitPoint = pos + dir * (remaining * tHit);
                transform.position = hitPoint;

                OnImpact(normal);

                // Reflejo base
                dir = Vector2.Reflect(dir, normal).normalized;

                // Ajuste de ángulo:
                if (hitPaddle)
                {
                    // Calcula un pequeño desvío según punto de impacto relativo (arriba/abajo)
                    PaddleDef p = PickPaddleByNormal(normal);
                    if (p != null)
                    {
                        float halfH = p.spriteForSize ? p.spriteForSize.bounds.extents.y : p.manualHalfHeight;
                        float relY = Mathf.Clamp((hitPoint.y - p.paddle.position.y) / Mathf.Max(0.001f, halfH), -1f, 1f);
                        // 0..±1 → ±desvío, 0 = centro
                        float maxBiasDeg = 12f; // cuánta inclinación máxima por golpear extremos
                        float biasDeg = relY * maxBiasDeg;
                        dir = (Quaternion.Euler(0, 0, biasDeg) * dir).normalized;
                    }
                }
                else
                {
                    // Pared: pequeño random opcional para evitar ciclos idénticos
                    if (randomAngleOnWall > 0f)
                    {
                        float ra = Random.Range(-randomAngleOnWall, randomAngleOnWall);
                        dir = (Quaternion.Euler(0, 0, ra) * dir).normalized;
                    }
                }

                // Clamp de ángulo (evita quedar muy vertical u horizontal)
                dir = ClampAngle(dir);

                // Skin/Escape para no “pegarse”
                if (useSkinEscape)
                {
                    float skin = Mathf.Max(0.001f, (pbounds.size.x + pbounds.size.y) * skinFactor);
                    transform.position = (Vector2)transform.position + normal * skin;
                    float minEscape = skin * minEscapeMultiplier;
                    remaining = Mathf.Max(remaining * (1f - tHit) * 0.95f, minEscape);
                }
                else
                {
                    remaining *= (1f - tHit) * 0.95f;
                }

                // Mantén velocidad constante (no amortiguamos)
                // speed = speed;
            }
            else
            {
                transform.position = next;
                remaining = 0f;
            }
        }

        // Recuperación de squash
        if (squashT > 0f)
        {
            squashT = Mathf.MoveTowards(squashT, 0f, Time.deltaTime * squashRecover);
            ApplySquash(dir, squashT);
        }
    }

    PaddleDef PickPaddleByNormal(Vector2 normal)
    {
        if (paddles == null) return null;
        bool left = (normal == Vector2.right); // si la normal apunta a +X, fue la paleta izquierda
        for (int i = 0; i < paddles.Length; i++)
            if (paddles[i] != null && paddles[i].isLeft == left) return paddles[i];
        return null;
    }

    void OnImpact(Vector2 normal)
    {
        // FX sutiles (opcionales)
        if (ballSR)
        {
            StopAllCoroutines();
            StartCoroutine(Flash());
        }

        squashT = 1f; // squash hacia la normal
        ApplySquash(normal, squashT);
    }

    System.Collections.IEnumerator Flash()
    {
        if (!ballSR) yield break;
        Color orig = ballSR.color;
        ballSR.color = hitFlash;
        yield return new WaitForSeconds(flashTime);
        ballSR.color = orig;
    }

    void ApplySquash(Vector2 n, float t)
    {
        float s = Mathf.SmoothStep(0f, squashAmount, t);
        Vector2 tangent = new Vector2(-n.y, n.x);
        float sx = 1f - s * Mathf.Abs(n.x) + s * Mathf.Abs(tangent.x);
        float sy = 1f - s * Mathf.Abs(n.y) + s * Mathf.Abs(tangent.y);
        var baseScale = (defaultScale == Vector3.zero) ? Vector3.one : defaultScale;
        transform.localScale = new Vector3(baseScale.x * sx, baseScale.y * sy, baseScale.z);
        if (t <= 0.001f) transform.localScale = baseScale;
    }

    Vector2 ClampAngle(Vector2 d)
    {
        // limita ángulo entre [minDegFromHorizontal, maxDegFromHorizontal]
        float signX = Mathf.Sign(d.x == 0 ? 1 : d.x);
        float signY = Mathf.Sign(d.y == 0 ? 1 : d.y);
        float deg = Mathf.Abs(Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg);

        float clamped = Mathf.Clamp(deg, minDegFromHorizontal, maxDegFromHorizontal);
        float finalDeg = clamped * (signY >= 0 ? 1f : -1f);
        Vector2 nd = new Vector2(1f * signX, Mathf.Tan(finalDeg * Mathf.Deg2Rad));
        return nd.normalized;
    }
}
