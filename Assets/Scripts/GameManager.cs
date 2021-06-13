using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    [SerializeField] int maxShips = 100;
    [SerializeField] public int nbShips = 0;

    //Scoring
    [SerializeField] TextMeshProUGUI TMScore;
    [SerializeField] public float score;
    [SerializeField] public PlayerController player;
    [SerializeField] TextMeshProUGUI TMCrystals;
    [SerializeField] TextMeshProUGUI scoreFin;
    [SerializeField] public float crystals;

    [SerializeField] GameObject scorePanel;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject fireBar;


    public AudioMixer audioMixer;


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

    private void Start()
    {
        SetVolume(-40f);
    }

    private void Update()
    {
        score += Time.deltaTime;
        if (maxShips < 200 && score > 60)
        {
            maxShips = 200;
        } else if (maxShips < 250 && score > 120)
        {
            maxShips = 250;
        }
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
        scoreFin.text = "" + score;
        fireBar.SetActive(false);
        healthBar.SetActive(false);
        gamePanel.SetActive(false);
        scorePanel.SetActive(true);
    }

    public void ReloadLevel()
    {
        UnpauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

}
