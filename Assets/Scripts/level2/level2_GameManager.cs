using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class Level2_GameManager : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject player;
    public BotnetHealth botnetBoss;
    public BotnetAtaques ataquesDelJefe;

    [Header("Sistema de Puntaje")]
    public int score = 0;

    [Header("Audios del Nivel")]
    public AudioClip startSound;
    public AudioClip coinSound;
    public AudioClip victorySound;
    private AudioSource myAudioSource;
    public AudioSource reproductorMusica;

    [Header("UI - Interfaz")]
    public GameObject panelFinal;
    public TextMeshProUGUI textoPuntaje;
    public TextMeshProUGUI textoMonedas;
    public TextMeshProUGUI textoTituloPanel;
    [Header("Botones Dinámicos")]
    public GameObject botonReiniciar;
    public GameObject botonNivel1;
    private int monedasRecogidas = 0;
    void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (botnetBoss != null) botnetBoss.OnBossDeath += HandleVictory; 
    }

    void OnDisable()
    {
        if (botnetBoss != null) botnetBoss.OnBossDeath -= HandleVictory;
    }

    void Start()
    {
        if (ataquesDelJefe != null) ataquesDelJefe.enabled = false;

        // Reproduce el sonido de inicio de nivel
        if (myAudioSource != null && startSound != null)
        {
            myAudioSource.PlayOneShot(startSound);
        }

        OnIntroFinished(); 
        ActualizarTextos();
    }

    public void OnIntroFinished()
    {
        InitializeBattle();
    }

    private void InitializeBattle()
    {
        if (ataquesDelJefe != null) ataquesDelJefe.enabled = true;
    }

public void AddScore()
    {
        score += 10;
        ActualizarTextos();
    }


    public void RecogerMoneda()
    {
        monedasRecogidas++;
        score += 500; 
        
        if (myAudioSource != null && coinSound != null)
        {
            myAudioSource.PlayOneShot(coinSound);
        }
        
        ActualizarTextos();
    }

    // Función que refresca la pantalla
    private void ActualizarTextos()
    {
        if (textoPuntaje != null) 
            textoPuntaje.text = "Puntaje: " + score;
            
        if (textoMonedas != null) 
            textoMonedas.text = "Monedas: " + monedasRecogidas + " / 7";
    }

    private void HandleVictory()
    {
        Debug.Log("GameManager: ¡Jefe derrotado! Iniciando secuencia de victoria...");
        StartCoroutine(VictorySequence());
    }

    private IEnumerator VictorySequence()
    {
        if (reproductorMusica != null)
        {
            reproductorMusica.Stop();
        }

        if (player != null)
        {
            PlayerControllerLevel2 playerScript = player.GetComponent<PlayerControllerLevel2>();
            if (playerScript != null)
            {
                playerScript.esInvulnerable = true; 
                playerScript.enabled = false; // Congelamos los controles
            }

            // Frenamos su cuerpo físico en seco
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            
            Animator playerAnim = player.GetComponent<Animator>();
            if (playerAnim != null) playerAnim.SetBool("isJumping", false);
        }

        // 2. ESPERAR (Pausa dramática para escuchar la muerte de Botnet)
        yield return new WaitForSeconds(2f);

        // 3. REPRODUCIR VICTORIA
        if (myAudioSource != null && victorySound != null)
        {
            myAudioSource.PlayOneShot(victorySound);
        }

        // 4. MOSTRAR PANTALLA FINAL (Preparando el terreno)
        yield return new WaitForSeconds(1f); // Pequeña pausa extra antes de mostrar el menú
        MostrarPantallaFinal();
    }

// Se llama cuando ganas
    private void MostrarPantallaFinal()
    {
        if (textoTituloPanel != null) textoTituloPanel.text = "Nivel Completado!";
        
        // Apagamos el botón de reiniciar y encendemos el de ir al Nivel 1
        if (botonReiniciar != null) botonReiniciar.SetActive(false);
        if (botonNivel1 != null) botonNivel1.SetActive(true);
        
        if (panelFinal != null) panelFinal.SetActive(true);
    }

    // Se llama cuando MUERES
    public void MostrarGameOver()
    {
        if (textoTituloPanel != null) textoTituloPanel.text = "Juego Terminado!";
        
        // Apagamos el botón del Nivel 1 y encendemos el de reiniciar
        if (botonNivel1 != null) botonNivel1.SetActive(false);
        if (botonReiniciar != null) botonReiniciar.SetActive(true);
        
        if (panelFinal != null) panelFinal.SetActive(true);
    }

    // Para el botón de reiniciar la pelea
    public void ReintentarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SalirDelJuego()
    {
        Debug.Log("Cerrando la aplicación...");
        
        // Esta línea cierra el juego compilado (.exe)
        Application.Quit(); 
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Esta función la usará el botón para regresar al Nivel 1 
    public void VolverAlNivel1()
    {
        SceneManager.LoadScene("Nivel1"); 
    }
}