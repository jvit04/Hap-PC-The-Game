using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class BotnetHealth : MonoBehaviour
{
    [Header("Atributos de Salud")]
    public int maxHealth = 1500;
    private int currentHealth;

    [Header("Referencias")]
    public Animator anim;

    [Header("Audio")]
    public AudioClip deathSound; 
    private AudioSource myAudioSource;

    public event Action OnBossDeath;
    private BotnetAtaques ataquesScript;
    [Header("Interfaz")]
    public TextMeshProUGUI textoVidaBotnet;
    void Start()
    {
        currentHealth = maxHealth;
        ataquesScript = GetComponent<BotnetAtaques>(); 
        
        myAudioSource = GetComponent<AudioSource>(); 
        
        if (anim == null)
        {
            Debug.LogError("Falta asignar el Animator en el script BotnetHealth");
        }
        if (textoVidaBotnet != null) textoVidaBotnet.text = "Botnet: " + currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        if (textoVidaBotnet != null) textoVidaBotnet.text = "Botnet: " + currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (ataquesScript != null) ataquesScript.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Reproducimos el sonido de muerte
        if (myAudioSource != null && deathSound != null)
        {
            myAudioSource.PlayOneShot(deathSound);
        }

        if (anim != null)
        {
            anim.SetTrigger("Die");
        }

        OnBossDeath?.Invoke();
    }
}