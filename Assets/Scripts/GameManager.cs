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

    [Header("Victoria")]
    [Tooltip("Segundos entre el sonido del portal y la fanfarria de victoria.")]
    public float victoryJingleDelay = 0.7f;

    [Header("Game Over")]
    [Tooltip("Reinicio automatico provisional mientras no exista la pantalla de Game Over.")]
    public bool autoRestartOnGameOver = true;
    public float autoRestartDelay = 1.5f;

    // Cuenta cuantas veces se ha intentado pasar el juego. Sobrevive a las
    // recargas de escena y solo vuelve a 1 cuando se cierra el juego.
    private static int attemptNumber = 1;

    private bool runIsOver;

    private void Start()
    {
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

        if (AudioManager.Instance != null)
        {
            // Primero el remolino del portal, y despues la fanfarria.
            AudioManager.Instance.PlayPortal();
            Invoke(nameof(PlayVictoryJingle), victoryJingleDelay);
        }

        // TODO: pantalla de victoria / transicion al nivel 2.
    }

    private void PlayVictoryJingle()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayVictory();
        }
    }

    /// <summary>
    /// Aqui se enganchara la pantalla de Game Over del companero.
    /// Su boton "Reintentar" debe llamar a Retry().
    /// </summary>
    private void GameOver()
    {
        Debug.Log("GAME OVER - intento numero " + attemptNumber);

        // TODO: reemplazar por la pantalla de Game Over.
        if (autoRestartOnGameOver)
        {
            Invoke(nameof(Retry), Mathf.Max(0.1f, autoRestartDelay));
        }
    }

    /// <summary>
    /// Empieza un intento nuevo: sube el contador y recarga el nivel.
    /// </summary>
    public void Retry()
    {
        attemptNumber++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Deja el contador en 1. Lo usara el boton "Jugar" del menu de inicio.
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
