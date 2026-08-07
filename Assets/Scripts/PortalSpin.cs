using UnityEngine;

/// <summary>
/// Hace girar el portal del final del nivel y le da un latido de escala,
/// para que se lea como un remolino que absorbe al jugador.
/// </summary>
public class PortalSpin : MonoBehaviour
{
    [Header("Giro")]
    [Tooltip("Grados por segundo. Negativo = sentido horario.")]
    public float rotationSpeed = -55f;

    [Header("Latido")]
    [Tooltip("Cuanto crece y encoge, en porcentaje de su tamano.")]
    [Range(0f, 0.4f)] public float pulseAmount = 0.07f;
    public float pulseSpeed = 1.8f;

    [Header("Resplandor (opcional)")]
    public SpriteRenderer glow;
    [Range(0f, 1f)] public float glowMinAlpha = 0.35f;
    [Range(0f, 1f)] public float glowMaxAlpha = 0.9f;

    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Mathf.Sin va de -1 a 1; lo llevamos a 0..1 para el latido.
        float wave = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        transform.localScale = baseScale * (1f + (wave - 0.5f) * 2f * pulseAmount);

        if (glow != null)
        {
            Color color = glow.color;
            color.a = Mathf.Lerp(glowMinAlpha, glowMaxAlpha, wave);
            glow.color = color;
        }
    }
}
