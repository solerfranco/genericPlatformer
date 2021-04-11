using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePlatforms : MonoBehaviour
{
    public Vector3 velocity;
    public float loopTime;
    private float currentTime;

    private void Awake()
    {
        currentTime = loopTime;
    }

    void FixedUpdate()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            velocity = velocity * -1;
            currentTime = loopTime;
        }
        transform.position += (velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isOk = collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy");
        if (isOk)
        {
            collision.transform.SetParent(transform);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        bool isOk = collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy");
        if (isOk)
        {
            collision.transform.SetParent(null);
        }
    }
}
