using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Carga la escena llamada "SampleScene"
    public void LoadSampleScene()
    {
        Debug.Log("...");
        SceneManager.LoadScene("Juego");

    }

    // Cierra el juego
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
