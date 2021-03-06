using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IEnemy
{
    private Rigidbody2D rb;
    private bool freeze;
    private SpriteRenderer sprite;
    private BoxCollider2D box;
    public LayerMask floorLayer;
    public float hitImpulse;

    private float direction;
    private float dirX;
    public float speed;
    public int health;
    public int rewardPoints;
    public float right;
    
    public Material white;
    private Material original;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        original = sprite.material;
    }

    public void TakeDamage(Vector2 attackerPos)
    {
        StartCoroutine(GameMaster.instance.Flash(sprite, original));
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
        rb.AddForce(new Vector2(dirX, 5 * hitImpulse), ForceMode2D.Impulse);

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
            Vector2 orientation = new Vector2(right, 0).normalized;

            dirX = 2 * orientation.x;
            sprite.flipX = orientation.x == 1;
        
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        }
    }

    bool thereIsWall()
    {
        float extraHeightText = 0.1f;
        Vector3 origin = (box.bounds.center - (new Vector3(box.size.x * transform.localScale.x / 2 + .01f, 0, 0)) * -right);
        RaycastHit2D raycastHit = Physics2D.Raycast(origin, new Vector2(right/5, 0), box.bounds.extents.x * transform.localScale.x / 20 + extraHeightText, floorLayer);
        if (raycastHit.collider == null)
        {
            raycastHit = Physics2D.Raycast(box.bounds.center - new Vector3(0, box.size.y * transform.localScale.y / 2 + 0.01f, 0) - (new Vector3(box.size.x * transform.localScale.x / 2, 0, 0)) * -right, Vector2.down, box.bounds.extents.y * transform.localScale.y + extraHeightText, floorLayer);
            return raycastHit.collider == null;
        }
        return raycastHit.collider != null;
    }

    void OnDrawGizmos()
    {
        if (!box) return;
        Vector3 origin = (box.bounds.center - (new Vector3(box.size.x / 2 + .01f, 0, 0)) * -right);
        Gizmos.DrawRay(origin, new Vector2(right / 5, 0));
        Gizmos.color = Color.red;
    }
}
