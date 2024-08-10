using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    public GameObject 
        pauseMenu,
        gameOverMenu;

    public RectTransform playerHPBar;

    public Text 
        coinText,
        distanceTraveledText,
        scoreText,
        bestScoreText;

    public Image soundBtn;

    public Sprite
        soundOn,
        soundOff;

    // Behaviour messages
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Behaviour messages
    void Start()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            soundBtn.sprite = soundOn;
        }
        else
        {
            soundBtn.sprite = soundOff;
        }
    }

    public void UpdateCoin(float coinAmout)
    {
        coinText.text = coinAmout + "";
    }

    public void UpdateDistance(float distance)
    {
        distanceTraveledText.text = distance + "m";
    }

    public void UpdateScore(float distance)
    {
        scoreText.text = "Score: " + distance;
        float best = PlayerPrefs.GetFloat(Constants.BEST_SCORE, 0);

        if (distance > best)
        {
            PlayerPrefs.SetFloat(Constants.BEST_SCORE, distance);
            bestScoreText.text = "Best: " + distance;
        }
        else
        {
            bestScoreText.text = "Best: " + best;
        }
    }

    public void UpdatePlayerHP(int amount)
    {   //血量更改處
        playerHPBar.localScale = new Vector3(Mathf.Clamp01(playerHPBar.localScale.x + (amount / 1500.0f)), 1.0f, 1.0f);
    }

    // Pause button is clicked
    public void PauseBtn_Onclick()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;

        SoundManager.Instance.PlaySound(Constants.CLICK_SOUND);
    }

    // Play button is clicked
    public void PlayBtn_Onlick()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;

        SoundManager.Instance.PlaySound(Constants.CLICK_SOUND);
    }

    // Restart button is clicked
    public void RestartBtn_Onclick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Play");

        SoundManager.Instance.PlaySound(Constants.CLICK_SOUND);
    }

    // Menu button is clicked
    public void MenuBtn_Onlick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");

        SoundManager.Instance.PlaySound(Constants.CLICK_SOUND);
    }

    public void GameOverShow()
    {
        

        gameOverMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    // Sound button is clicked
    public void SoundBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            PlayerPrefs.SetInt(Constants.SOUND_STATE, 0);
            soundBtn.sprite = soundOff;

            SoundManager.Instance.TurnOffSound();
        }
        else
        {
            PlayerPrefs.SetInt(Constants.SOUND_STATE, 1);
            soundBtn.sprite = soundOn;

            SoundManager.Instance.PlaySound(Constants.CLICK_SOUND);
            SoundManager.Instance.TurnOnSound();
        }
    }
}
