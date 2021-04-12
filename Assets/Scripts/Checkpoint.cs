using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool active;

    private void Start()
    {
        if (GameMaster.instance.currentCheckpoint.x > transform.position.x)
        {
            SetActive();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !active)
        {
            GameMaster.instance.currentCheckpoint = transform.position;
            SetActive();
        }
    }

    private void SetActive()
    {
        active = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}
