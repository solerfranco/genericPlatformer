using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool freeze;
    private SpriteRenderer sprite;
    private BoxCollider2D box;
    public LayerMask floorLayer;

    private float direction;
    private float dirX;
    public int health;
    public int rewardPoints;
    public float right;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ReceiveDamage(Vector2 attackerPos)
    {
        freeze = true;
        rb.velocity = new Vector2(0,0);

        int dirX;
        if(attackerPos.x > transform.position.x)
        {
            dirX = -5;
        }
        else
        {
            dirX = 5;
        }
        rb.AddForce(new Vector2(dirX, 5), ForceMode2D.Impulse);

        health--;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ScoreSystem.instance.AddPoint(rewardPoints);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            freeze = false;
        }
    }

    IEnumerator Turn()
    {
        freeze = true;
        yield return new WaitForSeconds(.01f);
        freeze = false;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -6f) Die();
        if (freeze) return;
        if (thereIsWall()) right = -right;
        if (dirX < 0 != direction < 0)
        {
            direction = dirX;
            StartCoroutine("Turn");
        }
        else
        {
            //Vector2 orientation = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;
            Vector2 orientation = new Vector2(right, 0).normalized;

            dirX = 2 * orientation.x;
            sprite.flipX = orientation.x == 1;
        
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        }
    }

    bool thereIsWall()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(box.bounds.center - (new Vector3(box.size.x / 2 + .01f, 0, 0)) * -right, new Vector2(right/5, 0), box.bounds.extents.x/10 + extraHeightText, floorLayer);
        if(raycastHit.collider == null)
        {
            raycastHit = Physics2D.Raycast(box.bounds.center - new Vector3(0, box.size.y / 2 + 0.01f, 0) - (new Vector3(box.size.x / 2, 0, 0)) * -right, Vector2.down, box.bounds.extents.y + extraHeightText, floorLayer);
            return raycastHit.collider == null;
        }
        return raycastHit.collider != null;
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
    }
}
