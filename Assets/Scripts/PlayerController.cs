using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float playerJumpForce = 20f;
    public float playerSpeed = 5f;
    public Sprite[] mySprites;
    private int index = 0;

    private Rigidbody2D myrigidbody2D;
    private SpriteRenderer mySpriteRenderer;
    
    //Referencia al Animator
    private Animator myAnimator;
    public GameObject Bullet;
   public GameManager myGameManager;
   public Transform bulletSpawnPoint;

   //Variables para que deje de saltar infinitamente
   public Transform groundCheck;
   public float groundCheckRadius = 0.2f;
   public LayerMask groundLayer;
   private bool isGrounded;
   private bool isShooting;
 
    void Start()
    {
        myrigidbody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

       //Se guarda el componente Animator
        myAnimator = GetComponent<Animator>();

        StartCoroutine(WalkCoroutine()); 

      myGameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(groundCheck !=null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius, groundLayer);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            myrigidbody2D.linearVelocity = new Vector2(myrigidbody2D.linearVelocity.x, playerJumpForce);
        }
        myrigidbody2D.linearVelocity = new Vector2(playerSpeed, myrigidbody2D.linearVelocity.y);
        
        //Controlar la animación según el salto
        //Si la velcidad vertical (Y) no es cercana a 0, está en el aire
        bool isAirbone =! isGrounded;
        if (myAnimator !=null){
            myAnimator.SetBool("isJumping",isAirbone);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
           Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
           StartCoroutine(ShootTimer());
        }

        if(myAnimator != null)
        {
            myAnimator.SetTrigger("Shoot");
        }
        
        

    }

    IEnumerator ShootTimer()
    {
        isShooting = true;
        yield return new WaitForSeconds(0.25f);
        isShooting = false;
    }


    IEnumerator WalkCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            //Concidiconal para saber si esta saltando
            bool isAirborne = Mathf.Abs(myrigidbody2D.linearVelocity.y) > 0.1f;

            if(!isAirborne && mySprites !=null && mySprites.Length>0)
            { 
                mySpriteRenderer.sprite = mySprites[index];
                index++;
                
                if (index == 6)
                {
                    index = 0;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemGood"))
        {
            Destroy(collision.gameObject);
            myGameManager.AddScore();
        }
        else if (collision.CompareTag("ItemBad"))
        {
            Destroy(collision.gameObject);
            PlayerDeath();
        }
        else if(collision.CompareTag("DeathZone"))
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        SceneManager.LoadScene("SampleScene");
    }
}