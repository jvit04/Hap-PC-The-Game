using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int score;
    public TMP_Text textScore;
  public void AddScore()
    {
        score++;
        textScore.text = score.ToString();
    }
}
