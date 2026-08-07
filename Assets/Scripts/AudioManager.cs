using UnityEngine;

/// <summary>
/// Reproduce la musica de fondo y los efectos de sonido del juego.
/// Sobrevive a los reinicios de escena para que la musica no se corte.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Musica de fondo")]
    public AudioClip musicClip;
    [Range(0f, 1f)] public float musicVolume = 0.35f;

    [Header("Efectos")]
    public AudioClip coinClip;
    public AudioClip hitClip;
    public AudioClip startClip;
    public AudioClip portalClip;
    public AudioClip victoryClip;
    public AudioClip defeatClip;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    [Header("Disparo")]
    public AudioClip shootClip;
    [Tooltip("Volumen aparte: al disparar en automatico los tiros se solapan.")]
    [Range(0f, 1f)] public float shootVolume = 0.35f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private static bool startClipAlreadyPlayed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        PlayMusic();
    }

    /// <summary>
    /// Arranca (o reanuda) la musica de fondo. Se llama al empezar cada intento,
    /// porque la derrota la detiene y este objeto sobrevive a la recarga de escena.
    /// </summary>
    public void PlayMusic()
    {
        if (musicClip == null || musicSource == null || musicSource.isPlaying)
        {
            return;
        }

        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayCoin() => PlaySfx(coinClip);

    public void PlayHit() => PlaySfx(hitClip);

    public void PlayPortal() => PlaySfx(portalClip);

    public void PlayShoot()
    {
        if (shootClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(shootClip, shootVolume);
        }
    }

    public void PlayVictory() => PlaySfx(victoryClip);

    public void PlayDefeat() => PlaySfx(defeatClip);

    /// <summary>
    /// Suena una sola vez al arrancar la partida, no en cada reintento.
    /// </summary>
    public void PlayStartOnce()
    {
        if (startClipAlreadyPlayed)
        {
            return;
        }

        startClipAlreadyPlayed = true;
        PlaySfx(startClip);
    }

    /// <summary>
    /// Baja la musica, por ejemplo durante la pantalla de derrota.
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    /// <summary>
    /// Permite que el sonido de inicio vuelva a sonar en una partida nueva.
    /// </summary>
    public static void ResetStartClip()
    {
        startClipAlreadyPlayed = false;
    }
}
