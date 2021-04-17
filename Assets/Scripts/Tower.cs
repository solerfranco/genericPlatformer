using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IEnemy
{
    public GameObject projectile;
    public Transform projectilePoint;
    private PlayerController player;
    private BoxCollider2D box;
    public LayerMask playerLayer;

    public float bulletSpeed;
    public float timer;
    private float currentTimer;
    public int health;
    public int rewardPoints;

    private float direction;

    private SpriteRenderer sprite;
    private Material original;

    public void TakeDamage(Vector2 attackerPos)
    {
        StartCoroutine(GameMaster.instance.Flash(sprite, original));
        currentTimer = timer;
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

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentTimer = timer;
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        original = sprite.material;
    }

    void Update()
    {
        direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        if (!ThereIsPlayer()) return;
        if (currentTimer > 0) currentTimer -= Time.deltaTime;
        else
        {
            GameObject bullet = Instantiate(projectile, projectilePoint.position, Quaternion.identity, null);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction, 0) * 10 * bulletSpeed);
            currentTimer = timer;
        }
    }

    bool ThereIsPlayer()
    {
        float extraWidthText = 0.2f;
        RaycastHit2D raycastHit = Physics2D.Raycast(box.bounds.center - new Vector3((box.size.x / 2 + extraWidthText) * direction, 0, 0), Vector2.left * -direction, 10, playerLayer);
        return raycastHit.collider != null;
    }

    void OnDrawGizmos()
    {
        if (box == null) return;

        float extraWidthText = 0.2f;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(box.bounds.center - new Vector3((box.size.x / 2 + extraWidthText) * direction, 0, 0), Vector2.left * -direction * 10);
    }
}
