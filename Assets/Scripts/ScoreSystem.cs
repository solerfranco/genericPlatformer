using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;
    public TextMeshProUGUI scoreText;
    public int score;

    private void Awake()
    {
        //Singleton pattern to ensure we have only one instance of this running at the same time.
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddPoint(int points)
    {
        score+=points;
        scoreText.text = score.ToString();
    }
}
