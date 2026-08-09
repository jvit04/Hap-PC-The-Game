using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControllerlvl2 : MonoBehaviour
{
    [Header("Configuración")]
    public float bulletSpeed = 10f;
    public Level2_GameManager myGameManager;

    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private float moveDirection = 1f;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Se asegura de encontrar tu GameManager del Nivel 2
        if (myGameManager == null)
        {
            myGameManager = FindFirstObjectByType<Level2_GameManager>();
        }

    }

    private void FixedUpdate()
    {
        myRigidbody2D.linearVelocity = Vector2.right * (bulletSpeed * moveDirection);
    }

    public void Initialize(float direction)
    {
        moveDirection = direction < 0f ? -1f : 1f;

        if (mySpriteRenderer != null)
        {
            mySpriteRenderer.flipX = moveDirection < 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Validamos si la bala chocó exactamente contra el jefe
        if (collision.CompareTag("Boss"))
        {
            // Buscamos el script de salud del jefe
            BotnetHealth saludJefe = collision.GetComponentInParent<BotnetHealth>();
            
            if (saludJefe != null)
            {
                saludJefe.TakeDamage(10); // Le resta vida
                
                // Llama al sistema de puntos que agregamos al GameManager
                if (myGameManager != null)
                {
                    myGameManager.AddScore(); 
                }
            }

            // Destruye la bala al impactar, sin importar si tiene lifetime o no
            Destroy(gameObject);
        }
    }
}