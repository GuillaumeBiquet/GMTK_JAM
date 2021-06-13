using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] int maxShips = 100;
    [System.NonSerialized] public int nbShips = 0;

    //Scoring
    [SerializeField] TextMeshProUGUI TMScore;
    [SerializeField] public float score;
    [SerializeField] public PlayerController player;
    [SerializeField] TextMeshProUGUI TMCrystals;
    [SerializeField] public float crystals;

    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject gamePanel;



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
        float min = Mathf.Round(score / 60);
        float sec = score % 60;
        TMScore.text = min + ":" + sec.ToString("F0");
        TMCrystals.text = crystals.ToString();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        PauseGame();
        gamePanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    public void ReloadLevel()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
