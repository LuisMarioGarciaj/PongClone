using UnityEngine;
using UnityEngine.UI;

public class ScanlineFlicker : MonoBehaviour
{
    public Image scanlineImg;
    public float speed = 3f;
    public float minAlpha = 0.05f, maxAlpha = 0.15f;

    void Update()
    {
        if (!scanlineImg) return;
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        var c = scanlineImg.color; c.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        scanlineImg.color = c;
    }
}
