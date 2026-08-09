using UnityEngine;
using System.Collections; // Este namespace es obligatorio para usar Corrutinas

public class VentanaEmergente : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    public float tiempoDeAviso = 1.0f; // Cuánto tiempo tiene el jugador para huir
    public float tiempoActivo = 2.0f;  // Cuánto tiempo se queda estorbando

    private BoxCollider2D colisionador;
    private SpriteRenderer renderizador;

    void Start()
    {
        // Obtenemos las referencias a los componentes del mismo objeto
        colisionador = GetComponent<BoxCollider2D>();
        renderizador = GetComponent<SpriteRenderer>();

        // Iniciamos la secuencia de tiempo
        StartCoroutine(SecuenciaAparicion());
    }

    IEnumerator SecuenciaAparicion()
    {
        // FASE 1: AVISO (El jugador ve el peligro pero no recibe daño)
        colisionador.enabled = false; // El collider está apagado
        
        // Modificamos el canal Alfa (transparencia) para que se vea fantasmal
        Color colorAviso = renderizador.color;
        colorAviso.a = 0.3f; // 30% de opacidad
        renderizador.color = colorAviso;

        // Le decimos a Unity que pause la ejecución de esta función aquí mismo
        yield return new WaitForSeconds(tiempoDeAviso);

        // FASE 2: ACTIVA (La trampa se cierra)
        colisionador.enabled = true; // El collider se enciende
        
        // Restauramos la transparencia al 100% para que se vea sólida
        colorAviso.a = 1.0f; 
        renderizador.color = colorAviso;

        // Pausamos de nuevo mientras la ventana está activa estorbando
        yield return new WaitForSeconds(tiempoActivo);

        // FASE 3: DESTRUCCIÓN
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Player"))
        {
            Debug.Log("¡El jugador chocó contra un pop-up sólido!");
            // Aquí irá la lógica para restar vida
        }
    }
}