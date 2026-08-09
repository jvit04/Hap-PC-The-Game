using UnityEngine;

public class ProyectilCelular : MonoBehaviour
{
    public float velocidad = 8f;

    void Update()
    {
        // Mueve el celular hacia la izquierda de la pantalla
        transform.Translate(Vector2.left * velocidad * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Player"))
        {
            Debug.Log("¡El jugador recibió un golpe de un celular!");
            // Aquí luego llamaremos a la función de restar vida al jugador
            Destroy(gameObject);
        }
        // Si choca contra una pared invisible detrás del jugador para que no viaje al infinito
        else if (colision.CompareTag("Pared")) 
        {
            Destroy(gameObject);
        }
    }
}