using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector]
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Invoke("DestroyAttack", 1.75f);
    }

    void FixedUpdate()
    {
        rb.AddForce(Vector2.right * speed, ForceMode2D.Force);
    }

    void DestroyAttack()
    {
        Destroy(gameObject);
    }
}
