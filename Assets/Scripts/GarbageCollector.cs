using UnityEngine;

/// <summary>
/// Zona de limpieza: destruye los objetos que caen fuera del nivel para que no
/// se queden acumulados en memoria.
///
/// Se pone como un trigger ancho por debajo de todo el escenario.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class GarbageCollector : MonoBehaviour
{
    [Tooltip("Escribe en la Console cada objeto limpiado.")]
    public bool verbose;

    private void Reset()
    {
        // Comodidad al agregar el componente desde el editor.
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // El jugador nunca se destruye: de su muerte se encarga el GameManager.
        if (collision.CompareTag("Player"))
        {
            return;
        }

        if (verbose)
        {
            Debug.Log($"[GarbageCollector] limpiado {collision.name}");
        }

        Destroy(collision.gameObject);
    }
}
