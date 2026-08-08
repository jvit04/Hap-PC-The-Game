using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Genera objetos por posicion, uno por uno.
///
/// Al arrancar anota donde quedo cada hijo con el tag indicado y lo retira de
/// la escena. Despues, conforme el jugador avanza, va instanciando cada uno
/// cuando se acerca a su posicion.
///
/// Asi el nivel se sigue disenando comodamente arrastrando objetos en el
/// editor, pero en ejecucion se crean dinamicamente y no se ven de lejos.
/// </summary>
public class ZoneSpawner : MonoBehaviour
{
    [Header("Que generar")]
    [Tooltip("Prefab que se instancia. Debe ser el mismo de los hijos.")]
    public GameObject prefab;

    [Tooltip("Solo se toman los hijos que tengan este tag.")]
    public string targetTag = "ItemGood";

    [Header("Activacion por posicion")]
    [Tooltip("Se busca solo por el tag 'Player' si se deja vacio.")]
    public Transform player;

    [Tooltip("A cuantas unidades del objeto aparece. Con mas de 15 nace fuera "
        + "de la pantalla y no se nota; por debajo se ve aparecer.")]
    public float activationDistance = 12f;

    [Tooltip("Escribe en la Console lo que va haciendo. Desactivar al entregar.")]
    public bool verbose;

    private readonly List<Pose> spawnPoses = new List<Pose>();
    private int nextIndex;

    private void Awake()
    {
        CapturePositions();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
            {
                player = found.transform;
            }
        }

        if (player == null)
        {
            Debug.LogWarning($"[{name}] no encontro al jugador, no va a generar nada.");
        }

        if (prefab == null)
        {
            Debug.LogWarning($"[{name}] no tiene prefab asignado.");
        }
    }

    /// <summary>
    /// Anota donde quedo cada hijo en el editor, los ordena de izquierda a
    /// derecha y los retira de la escena.
    /// </summary>
    private void CapturePositions()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (!child.CompareTag(targetTag))
            {
                continue;
            }

            spawnPoses.Add(new Pose(child.position, child.rotation));
            toRemove.Add(child.gameObject);
        }

        // El nivel avanza hacia la derecha, asi que este orden es el orden en
        // que el jugador se los va encontrando.
        spawnPoses.Sort((a, b) => a.position.x.CompareTo(b.position.x));

        foreach (GameObject go in toRemove)
        {
            Destroy(go);
        }

        if (verbose)
        {
            Debug.Log($"[{name}] capturados {spawnPoses.Count} objetos '{targetTag}'");
        }
    }

    private void Update()
    {
        if (player == null || prefab == null || nextIndex >= spawnPoses.Count)
        {
            return;
        }

        // Puede generar varios en el mismo frame si estan muy juntos.
        while (nextIndex < spawnPoses.Count
            && player.position.x >= spawnPoses[nextIndex].position.x - activationDistance)
        {
            SpawnNext();
        }
    }

    private void SpawnNext()
    {
        Pose pose = spawnPoses[nextIndex];
        Instantiate(prefab, pose.position, pose.rotation, transform);
        nextIndex++;

        if (verbose)
        {
            Debug.Log($"[{name}] generado #{nextIndex} en x={pose.position.x:0.0}");
        }
    }

    /// <summary>
    /// Dibuja en el Scene view un circulo por cada objeto pendiente, del
    /// tamano de la distancia a la que va a aparecer.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 1f, 0.35f);

        foreach (Transform child in transform)
        {
            if (child.CompareTag(targetTag))
            {
                Gizmos.DrawWireSphere(child.position, activationDistance);
            }
        }
    }
}
