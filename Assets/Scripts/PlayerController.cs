﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerActions;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Animator anim;
    public LayerMask floorLayer;

    private bool freeze;
    private bool attacking;
    private bool jumping;

    public Image energySlider;
    private float energy;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;

    public float speed;
    private Vector2 direction;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        playerActions = new PlayerControls();
        playerActions.Player.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        playerActions.Player.Move.canceled += ctx => direction = ctx.ReadValue<Vector2>();

        playerActions.Player.Jump.performed += ctx => Jump();
        playerActions.Player.Jump.canceled += ctx => CancelJump();

        playerActions.Player.Attack.performed += ctx => Attack();
    }

    private void Attack()
    {
        if (!attacking)
        {
            StartCoroutine("Attacking");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                energy += 0.025f;
                energySlider.fillAmount = energy;
                enemy.GetComponent<EnemyController>().ReceiveDamage(transform.position);
                if(attackPoint.localPosition.y < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0, 13), ForceMode2D.Impulse);
                }
            }
        }
    }

    IEnumerator Attacking()
    {
        anim.SetTrigger("attack");
        attacking = true;
        yield return new WaitForSeconds(0.3f);
        attacking = false;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 13), ForceMode2D.Impulse);
        }
    }

    private void CancelJump()
    {
        if(rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void FixedUpdate()
    {
        if (freeze) return;
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        if (!attacking)
        {
            Vector2 dirX = new Vector2(direction.x,0).normalized;

            if(dirX.x != 0)
            {
                direction.x = dirX.x;
                attackPoint.localPosition = new Vector2(0.75f * dirX.x, 0);
                sprite.flipX = dirX.x == 1;
            }

            if (direction.y > 0.5f)
            {
                attackPoint.localPosition = new Vector2(0, 1);
            }
            if (direction.y < -0.5f && jumping)
            {
                attackPoint.localPosition = new Vector2(0, -1.25f);
            }
        }
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -21, 1000 /* CAMBIAR */), transform.position.y, transform.position.z);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            freeze = false;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ReceiveDamage(collision.gameObject.transform.position);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ReceiveDamage(Vector2 attackerPos)
    {
        freeze = true;
        rb.velocity = new Vector2(0, 0);

        Vector2 orientation = new Vector2(transform.position.x - attackerPos.x, 0).normalized;

        rb.AddForce(new Vector2(7 * orientation.x, 7), ForceMode2D.Impulse);
    }

    bool isGrounded()
    {
        float extraHeightText = 0.2f;
        RaycastHit2D raycastHit = Physics2D.Raycast(box.bounds.center, Vector2.down, box.bounds.extents.y + extraHeightText, floorLayer);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(box.bounds.center, Vector2.down * (box.bounds.extents.y + extraHeightText), rayColor);
        return raycastHit.collider != null;
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null || box == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(box.bounds.center - new Vector3(0, box.size.y/2 + .1f, 0), Vector2.down/5 * (box.bounds.extents.y + 0.2f));
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
