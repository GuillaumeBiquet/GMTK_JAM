using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] int maxShips = 100;
    [System.NonSerialized] public int nbShips = 0;
    [SerializeField] public float coins;

    //Scoring
    [SerializeField] TextMeshProUGUI TMScore;
    [SerializeField] public float score;
    float timer = 0f;

    public bool TooManyShips { get { return nbShips >= maxShips; } }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        score += Time.deltaTime;
        /* timer += Time.deltaTime;
         if (timer > 1)
         {
             score += 10;
             timer = 0;
         }*/
        float min = Mathf.Round(score / 60);
        float sec = score % 60;
        //TMScore.text = min + ":" + sec.ToString("F0");
    }


}
