using UnityEngine;

public class BallLoop : MonoBehaviour
{
    public SpriteRenderer playfield;  // borde/cancha
    public Vector2 unitsPerSecond = new Vector2(6f, 4f);
    public float margin = 0.15f;      // que no “pegue” justo al borde

    private Vector2 _vel;
    private Bounds _b;

    void Start()
    {
        if (playfield == null)
        {
            Debug.LogError("[BallLoop] Asigna el SpriteRenderer del playfield");
            enabled = false; return;
        }
        _b = playfield.bounds;
        _vel = unitsPerSecond; // en unidades de mundo/segundo
    }

    void Update()
    {
        Vector2 pos = transform.position;
        pos += _vel * Time.deltaTime;

        float minX = _b.min.x + margin, maxX = _b.max.x - margin;
        float minY = _b.min.y + margin, maxY = _b.max.y - margin;

        if (pos.y > maxY) { pos.y = maxY; _vel.y *= -1f; }
        if (pos.y < minY) { pos.y = minY; _vel.y *= -1f; }
        if (pos.x > maxX) { pos.x = maxX; _vel.x *= -1f; }
        if (pos.x < minX) { pos.x = minX; _vel.x *= -1f; }

        transform.position = pos;
    }
}
