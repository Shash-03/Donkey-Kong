using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runningSprites = new Sprite[4];
    public Sprite climbingSprites;
    public int spriteIndex = 0;


    private Rigidbody2D mario;

    private Vector2 direction;

    private Collider2D[] collideObjects;
    private new Collider2D collider;

    public float speed = 1f;
    public float jump = 1f;
    public float gravityStrength = 1f;

    private bool isGrounded;
    private bool isClimbing;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mario = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        collideObjects = new Collider2D[5];
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite),1f/12f,1f/12f);  
    }

    private void OnDisable()
    {
        CancelInvoke();
    }



    private void CollisionDetection()
    {
        isGrounded = false;
        isClimbing = false;
        Vector2 size = collider.bounds.size;
        size.x /= 2f;
        size.y += 0.1f;

        int numObjects = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, collideObjects);

        for (int i = 0; i < numObjects; i++)
        {
            GameObject obj = collideObjects[i].gameObject;

            if (obj.layer == LayerMask.NameToLayer("Ground"))
            {
                isGrounded = obj.transform.position.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(collider, collideObjects[i], !isGrounded);
            }
            else if (obj.layer == LayerMask.NameToLayer("Ladder"))
            {
                isClimbing = true;
            }
        }
    }

    private void Update()
    {
        float characterSpeed = speed + 6.5f;
        float jumpStrength = jump + 4f;

        CollisionDetection();

        if (isClimbing)
        {
            direction.y = Input.GetAxis("Vertical") * characterSpeed;
        }
        else if (isGrounded && Input.GetButtonDown("Jump"))
        {
            direction.y = jumpStrength;
        }
        else
        {
            Vector2 gravity = Physics2D.gravity * (gravityStrength + 0.25f);
            direction += gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * characterSpeed;

        if (isGrounded)
        {
            direction.y = Mathf.Max(direction.y, -1f); // Prevents gravity from being too overpowering
        }

        if (direction.x > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }


    private void FixedUpdate()
    {
        mario.MovePosition(mario.position + direction * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Princess"))
        {
            enabled = false;
            FindObjectOfType<Manager>().LevelComplete();

        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<Manager>().LevelFailed();
        }
    }

    private void AnimateSprite()
    {
        if (isClimbing)
        {
            spriteRenderer.sprite = climbingSprites;
        }

        else if (direction.x != 0f)
        {
            spriteIndex++;

            if (spriteIndex >= runningSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runningSprites[spriteIndex]; 

        }
    }
}
