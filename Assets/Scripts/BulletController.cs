using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifetime = 0.3f;
    public GameManager myGameManager;

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
        if (myGameManager == null)
        {
            myGameManager = FindFirstObjectByType<GameManager>();
        }

        Destroy(gameObject, lifetime);
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
        if (collision.CompareTag("ItemBad"))
        {
            if (myGameManager != null)
            {
                myGameManager.AddScore();
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("ItemGood"))
        {
            // La moneda debe recogerla el jugador, no la bala.
            Destroy(gameObject);
        }
        else if (collision.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }
}
