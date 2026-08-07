using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Puntaje")]
    public int score;
    public TMP_Text textScore;
    public string scorePrefix = "PUNTOS ";

    [Header("Intentos")]
    public int maxAttempts = 3;
    public TMP_Text textAttempts;
    public string attemptsPrefix = "INTENTOS ";

    [Header("Muerte")]
    public float delayBeforeRestart = 0.6f;

    // Se mantiene entre recargas de escena para que los intentos no vuelvan a 3.
    private static int attemptsRemaining = -1;
    private static bool runIsOver;

    private bool isRestarting;

    private void Awake()
    {
        if (attemptsRemaining < 0)
        {
            attemptsRemaining = maxAttempts;
        }
    }

    private void Start()
    {
        UpdateScoreText();
        UpdateAttemptsText();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayStartOnce();
        }
    }

    public int AttemptsRemaining => attemptsRemaining;

    public void AddScore(int amount = 1)
    {
        score += amount;
        UpdateScoreText();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoin();
        }
    }

    /// <summary>
    /// El jugador murio: descuenta un intento y reinicia, o termina la partida.
    /// </summary>
    public void LoseAttempt()
    {
        if (isRestarting || runIsOver)
        {
            return;
        }

        attemptsRemaining--;
        UpdateAttemptsText();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayHit();
        }

        if (attemptsRemaining <= 0)
        {
            attemptsRemaining = 0;
            runIsOver = true;
            GameOver();
            return;
        }

        isRestarting = true;
        Invoke(nameof(ReloadScene), Mathf.Max(0f, delayBeforeRestart));
    }

    /// <summary>
    /// Se quedo sin intentos. Aqui se enganchara la pantalla de Game Over.
    /// </summary>
    private void GameOver()
    {
        Debug.Log("GAME OVER - sin intentos");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayDefeat();
        }

        // TODO: mostrar la pantalla de Game Over del companero.
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
            AudioManager.Instance.PlayVictory();
        }

        // TODO: pantalla de victoria / transicion al nivel 2.
    }

    /// <summary>
    /// Vuelve a dejar la partida en cero. Lo usara el boton "Jugar" del menu.
    /// </summary>
    public static void ResetRun()
    {
        attemptsRemaining = -1;
        runIsOver = false;
        AudioManager.ResetStartClip();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            textAttempts.text = attemptsPrefix + attemptsRemaining;
        }
    }
}
