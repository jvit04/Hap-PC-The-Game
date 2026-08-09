using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Puntaje")]
    public int score;
    public TMP_Text textScore;
    public string scorePrefix = "Puntos: ";

    [Header("Intentos")]
    public TMP_Text textAttempts;
    public string attemptsPrefix = "Intento: ";

    [Header("Pantalla final")]
    [Tooltip("Panel que se muestra al morir o al completar el nivel.")]
    public GameObject panelFinal;
    public TMP_Text textoTitulo;
    [Tooltip("Boton que reinicia el nivel. Solo aparece al perder.")]
    public GameObject botonReintentar;
    [Tooltip("Boton que lleva al nivel 2. Solo aparece al ganar.")]
    public GameObject botonSiguienteNivel;

    public string tituloDerrota = "JUEGO TERMINADO";
    public string tituloVictoria = "NIVEL COMPLETADO";

    [Header("Escenas")]
    public string siguienteEscena = "Level2_Boss";
    public string escenaMenu = "MainMenu";

    [Header("Victoria")]
    [Tooltip("Segundos entre el sonido del portal y la fanfarria de victoria.")]
    public float victoryJingleDelay = 0.7f;
    [Tooltip("Segundos antes de que aparezca el panel de victoria.")]
    public float victoryPanelDelay = 1.6f;

    [Header("Respaldo sin panel")]
    [Tooltip("Solo se usa si no hay panel asignado.")]
    public bool autoRestartOnGameOver = true;
    public float autoRestartDelay = 1.5f;

    // Cuenta cuantas veces se ha intentado pasar el juego. Sobrevive a las
    // recargas de escena y solo vuelve a 1 cuando se cierra el juego.
    private static int attemptNumber = 1;

    private bool runIsOver;

    private void Start()
    {
        if (panelFinal != null)
        {
            panelFinal.SetActive(false);
        }

        UpdateScoreText();
        UpdateAttemptsText();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
            AudioManager.Instance.PlayStartOnce();
        }
    }

    public int AttemptNumber => attemptNumber;

    public void AddScore(int amount = 1)
    {
        if (runIsOver)
        {
            return;
        }

        score += amount;
        UpdateScoreText();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoin();
        }
    }

    /// <summary>
    /// El jugador choco o cayo: se acaba la partida y se muestra Game Over.
    /// </summary>
    public void PlayerDied()
    {
        if (runIsOver)
        {
            return;
        }

        runIsOver = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHit();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayDefeat();
        }

        GameOver();
    }

    /// <summary>
    /// El jugador llego al portal del final del nivel.
    /// </summary>
    public void LevelComplete()
    {
        if (runIsOver)
        {
            return;
        }

        runIsOver = true;
        Debug.Log("NIVEL 1 COMPLETADO - Puntos: " + score);

        CongelarJugador();
        StartCoroutine(SecuenciaDeVictoria());
    }

    private IEnumerator SecuenciaDeVictoria()
    {
        if (AudioManager.Instance != null)
        {
            // Se calla la musica del nivel para que se luzca la victoria.
            AudioManager.Instance.StopMusic();

            // Primero el remolino del portal.
            AudioManager.Instance.PlayPortal();
            yield return new WaitForSeconds(victoryJingleDelay);
            AudioManager.Instance.PlayVictory();
        }

        yield return new WaitForSeconds(
            Mathf.Max(0f, victoryPanelDelay - victoryJingleDelay));

        MostrarPanel(tituloVictoria, reintentar: false, siguiente: true);
    }

    /// <summary>
    /// Se quedo sin vida: muestra la pantalla de derrota.
    /// </summary>
    private void GameOver()
    {
        Debug.Log("GAME OVER - intento numero " + attemptNumber);

        CongelarJugador();

        if (panelFinal != null)
        {
            MostrarPanel(tituloDerrota, reintentar: true, siguiente: false);
        }
        else if (autoRestartOnGameOver)
        {
            // Respaldo por si el panel no esta armado todavia.
            Invoke(nameof(Retry), Mathf.Max(0.1f, autoRestartDelay));
        }
    }

    private void MostrarPanel(string titulo, bool reintentar, bool siguiente)
    {
        if (textoTitulo != null)
        {
            textoTitulo.text = titulo;
        }

        if (botonReintentar != null)
        {
            botonReintentar.SetActive(reintentar);
        }

        if (botonSiguienteNivel != null)
        {
            botonSiguienteNivel.SetActive(siguiente);
        }

        if (panelFinal != null)
        {
            panelFinal.SetActive(true);
        }
    }

    /// <summary>
    /// Quita el control al jugador para que no se siga moviendo bajo el panel.
    /// </summary>
    private void CongelarJugador()
    {
        PlayerController jugador = FindFirstObjectByType<PlayerController>();
        if (jugador != null)
        {
            jugador.enabled = false;

            Rigidbody2D cuerpo = jugador.GetComponent<Rigidbody2D>();
            if (cuerpo != null)
            {
                cuerpo.linearVelocity = Vector2.zero;
            }
        }
    }

    // ---------------------------------------------------------------- botones

    /// <summary>
    /// Boton "Reintentar": empieza un intento nuevo y recarga el nivel.
    /// </summary>
    public void Retry()
    {
        attemptNumber++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Boton "Siguiente nivel": pasa al nivel 2 conservando los intentos.
    /// </summary>
    public void IrAlSiguienteNivel()
    {
        // El nivel 2 trae su propia musica, y este AudioManager sobrevive al
        // cambio de escena: si no lo callamos aqui, sonarian las dos a la vez.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        SceneManager.LoadScene(siguienteEscena);
    }

    /// <summary>
    /// Boton "Menu": vuelve a la pantalla de inicio y deja la partida en cero.
    /// </summary>
    public void VolverAlMenu()
    {
        ResetRun();

        // La musica pudo haberse detenido por una derrota o una victoria, y en
        // el menu no hay nadie que la vuelva a arrancar.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }

        SceneManager.LoadScene(escenaMenu);
    }

    /// <summary>
    /// Boton "Salir". Dentro del editor no hace nada visible.
    /// </summary>
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando la aplicacion");
        Application.Quit();
    }

    /// <summary>
    /// Deja el contador en 1. Lo usa el boton "Jugar" del menu de inicio.
    /// </summary>
    public static void ResetRun()
    {
        attemptNumber = 1;
        AudioManager.ResetStartClip();
    }

    private void UpdateScoreText()
    {
        if (textScore != null)
        {
            textScore.text = scorePrefix + score;
        }
    }

    private void UpdateAttemptsText()
    {
        if (textAttempts != null)
        {
            textAttempts.text = attemptsPrefix + attemptNumber;
        }
    }
}
