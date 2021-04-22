using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : MonoBehaviour, IEnemy
{
    private Animator anim;

    public GameObject projectile;
    public Transform projectilePoint;
    public float timer;
    private float currentTimer;
    public int health;
    public int rewardPoints;
    private SpriteRenderer sprite;
    private Material original;
    private float direction;

    private Transform player;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().transform;
        sprite = GetComponent<SpriteRenderer>();
        original = sprite.material;
        currentTimer = timer;
    }

    void Update()
    {
        sprite.flipX = direction < 0;
        direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        if (currentTimer > 0) currentTimer -= Time.deltaTime;
        else
        {
            anim.SetTrigger("Attack");
            GameObject bullet = Instantiate(projectile, projectilePoint.position, Quaternion.identity, null);
            bullet.GetComponent<DistanceAttack>().speed *= direction;
            currentTimer = timer;
        }

    }

    public void TakeDamage(Vector2 attackerPos)
    {
        StartCoroutine(GameMaster.instance.Flash(sprite, original));

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
}
