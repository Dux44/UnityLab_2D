using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groudCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator playerAnimations;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    public AudioSource audiJump;
    public AudioSource collectCoin;

    private int score = 0;
    public Text scoreText;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimations = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreText.text = "Score: "+ score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groudCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");
        if (direction > 0f)
        {
            
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);
            transform.localScale = new Vector2(-1f,1f);
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            audiJump.Play();
        }

        playerAnimations.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimations.SetBool("OnGround", isTouchingGround);

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        else if(collision.tag == "Bronse")
        {
            score += 1;
            collision.gameObject.SetActive(false);
            collectCoin.Play();

        }
        else if(collision.tag == "Silver")
        {
            score += 3;
            collision.gameObject.SetActive(false);
            collectCoin.Play();
        }
        else if(collision.tag == "Gold")
        {
            score += 5;
            collision.gameObject.SetActive(false);
            collectCoin.Play();
        }
        
        scoreText.text = "Score: " + score.ToString();

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Spike")
        {
            healthBar.Damage(0.002f);
        }
    }


}
