using UnityEngine;

public class BotnetAtaques : MonoBehaviour
{
    [Header("Ataque: Celulares")]
    public GameObject prefabCelular;
    public Transform[] puntosDeDisparo; 
    
    [Header("Ataque: Ventanas Emergentes")]
    public GameObject prefabVentana;
    public Transform[] zonasDeAparicionVentanas;

    [Header("Tiempos Globales")]
    public float tiempoEntreAtaques = 2f;
    private float temporizador;
    private int indicePuntoActual = 0;

    void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreAtaques)
        {
            ElegirAtaqueAlAzar();
            temporizador = 0f;
        }
    }

    void ElegirAtaqueAlAzar()
    {
        // Genera un número aleatorio entre 0 y 1 (el 2 es exclusivo en este método con enteros)
        int ataqueElegido = Random.Range(0, 2); 

        if (ataqueElegido == 0)
        {
            DispararCelularAlternado();
        }
        else if (ataqueElegido == 1)
        {
            InvocarVentanasEmergentes();
        }
    }

    void DispararCelularAlternado()
    {
        if (puntosDeDisparo.Length == 0) return;

        Transform puntoTurno = puntosDeDisparo[indicePuntoActual];
        Instantiate(prefabCelular, puntoTurno.position, Quaternion.identity);
        
        indicePuntoActual++;
        if (indicePuntoActual >= puntosDeDisparo.Length) indicePuntoActual = 0;
    }

    void InvocarVentanasEmergentes()
    {
        if (zonasDeAparicionVentanas.Length == 0) return;

        // Elige una zona al azar de tu lista de posiciones
        int zonaAleatoria = Random.Range(0, zonasDeAparicionVentanas.Length);
        Transform posicionElegida = zonasDeAparicionVentanas[zonaAleatoria];

        Instantiate(prefabVentana, posicionElegida.position, Quaternion.identity);
        Debug.Log("¡Botnet invocó un Pop-Up en la zona " + zonaAleatoria + "!");
    }
}