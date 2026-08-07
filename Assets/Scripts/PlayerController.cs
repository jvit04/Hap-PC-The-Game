using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float playerJumpForce = 20f;
    public float playerSpeed = 5f;
    public float fastFallSpeed = 12f;

    [Header("Animacion")]
    public Sprite[] mySprites;
    public float walkFrameDuration = 0.1f;
    public Sprite[] jumpSprites;
    public float jumpFrameDuration = 0.1f;
    public Sprite[] shootSprites;
    public float shootFrameDuration = 0.05f;

    [Header("Disparo")]
    public GameObject Bullet;
    public Transform bulletSpawnPoint;
    public float shootInterval = 0.25f;

    [Header("Deteccion del suelo")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Referencias")]
    public GameManager myGameManager;

    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    private Animator myAnimator;

    private float horizontalInput;
    private float nextShootTime;
    private float facingDirection = 1f;
    private float bulletSpawnPointX;
    private bool isGrounded;
    private bool jumpRequested;
    private bool fastFallRequested;
    private bool shootHeld;
    private bool isDead;
    private int spriteIndex;
    private int jumpSpriteIndex;
    private int shootSpriteIndex;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (bulletSpawnPoint != null)
        {
            bulletSpawnPointX = Mathf.Abs(bulletSpawnPoint.localPosition.x);
        }

        if (myGameManager == null)
        {
            myGameManager = FindFirstObjectByType<GameManager>();
        }

        if (mySpriteRenderer != null && mySprites != null && mySprites.Length > 0)
        {
            StartCoroutine(WalkCoroutine());
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0f)
        {
            facingDirection = Mathf.Sign(horizontalInput);

            if (mySpriteRenderer != null)
            {
                mySpriteRenderer.flipX = facingDirection < 0f;
            }

            if (bulletSpawnPoint != null)
            {
                Vector3 spawnPosition = bulletSpawnPoint.localPosition;
                spawnPosition.x = bulletSpawnPointX * facingDirection;
                bulletSpawnPoint.localPosition = spawnPosition;
            }
        }

        UpdateGroundedState();

        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.Space);

        if (jumpKeyPressed && isGrounded)
        {
            jumpRequested = true;
        }

        fastFallRequested = !isGrounded
            && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));

        shootHeld = Input.GetKey(KeyCode.E);

        if (shootHeld && Time.time >= nextShootTime)
        {
            Shoot();
        }

        if (myAnimator != null)
        {
            myAnimator.SetBool("isJumping", !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = myRigidbody2D.linearVelocity;
        velocity.x = horizontalInput * playerSpeed;

        if (jumpRequested)
        {
            velocity.y = playerJumpForce;
            jumpRequested = false;
        }
        else if (fastFallRequested && velocity.y > -fastFallSpeed)
        {
            velocity.y = -fastFallSpeed;
        }

        myRigidbody2D.linearVelocity = velocity;
    }

    private void UpdateGroundedState()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void Shoot()
    {
        if (Bullet == null || bulletSpawnPoint == null)
        {
            return;
        }

        nextShootTime = Time.time + Mathf.Max(0.05f, shootInterval);

        GameObject newBullet = Instantiate(
            Bullet,
            bulletSpawnPoint.position,
            bulletSpawnPoint.rotation
        );

        BulletController bulletController = newBullet.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.Initialize(facingDirection);
        }

        if (myAnimator != null)
        {
            myAnimator.SetTrigger("Shoot");
        }
    }

    private IEnumerator WalkCoroutine()
    {
        while (true)
        {
            if (shootHeld && shootSprites != null && shootSprites.Length > 0)
            {
                mySpriteRenderer.sprite = shootSprites[shootSpriteIndex];
                shootSpriteIndex = (shootSpriteIndex + 1) % shootSprites.Length;

                yield return new WaitForSeconds(Mathf.Max(0.03f, shootFrameDuration));
                continue;
            }

            shootSpriteIndex = 0;

            if (!isGrounded && jumpSprites != null && jumpSprites.Length > 0)
            {
                mySpriteRenderer.sprite = jumpSprites[jumpSpriteIndex];
                jumpSpriteIndex = (jumpSpriteIndex + 1) % jumpSprites.Length;

                yield return new WaitForSeconds(Mathf.Max(0.05f, jumpFrameDuration));
                continue;
            }

            jumpSpriteIndex = 0;

            bool isWalking = isGrounded && Mathf.Abs(horizontalInput) > 0.01f;
            if (!isWalking || mySprites == null || mySprites.Length == 0)
            {
                if (mySprites != null && mySprites.Length > 0)
                {
                    spriteIndex = 0;
                    mySpriteRenderer.sprite = mySprites[0];
                }
            }
            else
            {
                mySpriteRenderer.sprite = mySprites[spriteIndex];
                spriteIndex = (spriteIndex + 1) % mySprites.Length;
            }

            yield return new WaitForSeconds(Mathf.Max(0.05f, walkFrameDuration));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemGood"))
        {
            Destroy(collision.gameObject);

            if (myGameManager != null)
            {
                myGameManager.AddScore();
            }
        }
        else if (collision.CompareTag("ItemBad"))
        {
            Destroy(collision.gameObject);
            PlayerDeath();
        }
        else if (collision.CompareTag("DeathZone"))
        {
            PlayerDeath();
        }
        else if (collision.CompareTag("Finish"))
        {
            if (myGameManager != null)
            {
                myGameManager.LevelComplete();
            }
        }
    }

    private void PlayerDeath()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        // Deja de responder a los controles mientras se reinicia.
        horizontalInput = 0f;
        enabled = false;

        if (myGameManager != null)
        {
            myGameManager.LoseAttempt();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
