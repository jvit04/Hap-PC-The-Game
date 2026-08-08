using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la pantalla de inicio. Se engancha a los botones desde el Inspector.
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("Escena a cargar")]
    [Tooltip("Debe estar agregada en Build Profiles > Scene List.")]
    public string levelSceneName = "Level1_Adventure";

    /// <summary>
    /// Boton "Jugar": deja la partida en cero y arranca el nivel 1.
    /// </summary>
    public void PlayGame()
    {
        // Reinicia el contador de intentos y permite que el sonido de inicio
        // vuelva a sonar, aunque se venga de una partida anterior.
        GameManager.ResetRun();

        if (string.IsNullOrWhiteSpace(levelSceneName))
        {
            Debug.LogError("MenuController: falta escribir el nombre de la escena.");
            return;
        }

        SceneManager.LoadScene(levelSceneName);
    }

    /// <summary>
    /// Boton "Salir". Dentro del editor de Unity no hace nada visible;
    /// solo funciona en el ejecutable.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}
