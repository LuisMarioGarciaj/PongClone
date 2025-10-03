using UnityEngine;

public class BlinkLine : MonoBehaviour
{
    public SpriteRenderer sr;
    public float speed = 2f;
    public float minAlpha = 0.5f, maxAlpha = 1f;

    void Reset() { sr = GetComponent<SpriteRenderer>(); }

    void Update()
    {
        if (!sr) return;
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; // 0..1
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);
        var c = sr.color; c.a = a; sr.color = c;
    }
}
