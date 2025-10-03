using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public RectTransform selectorBall;
    public Button btnStart;
    public Button btnExit;
    public float moveSpeed = 10f;
    public float bobAmplitude = 6f;
    public float bobSpeed = 6f;

    private RectTransform _target;
    private int _index = 0; // 0: Start, 1: Exit
    private Vector2 _baseOffset = new Vector2(-80f, 0f); // pelota a la izquierda

    void Start()
    {
        Select(0);
        btnStart.onClick.AddListener(OnStart);
        btnExit.onClick.AddListener(OnExit);
    }

    void Update()
    {
        // Navegación con flechas o W/S
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) Select(0);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) Select(1);

        // Confirmar
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_index == 0) OnStart(); else OnExit();
        }

        // Mover selector hacia el botón objetivo con “lerp”
        if (_target != null)
        {
            var targetPos = (Vector2)_target.position + _baseOffset;
            // Efecto bob (rebote vertical)
            targetPos.y += Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;

            selectorBall.position = Vector2.Lerp(selectorBall.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }

    void Select(int idx)
    {
        _index = idx;
        _target = (idx == 0) ? btnStart.GetComponent<RectTransform>() : btnExit.GetComponent<RectTransform>();
        // (Opcional) reproducir un bloop retro aquí
    }

    void OnStart()
    {
        // Carga tu escena del juego
        SceneManager.LoadScene("GameScene");
    }

    void OnExit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
