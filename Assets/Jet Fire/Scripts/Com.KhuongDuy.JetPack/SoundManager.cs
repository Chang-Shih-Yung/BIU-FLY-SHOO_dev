using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    public AudioSource
        gameMusic,
        click,
        coin,
        playerShoot,
        enemyShoot,
        explosion;

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
        gameMusic.Play();
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            gameMusic.volume = 1;
        }
        else
        {
            gameMusic.volume = 0;
        }
    }

    public void PlaySound(string soundName)
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            switch (soundName)
            {
                case Constants.CLICK_SOUND:
                    click.Play();
                    break;
                case Constants.COIN_SOUND:
                    coin.Play();
                    break;
                case Constants.PLAYER_SHOOT_SOUND:
                    playerShoot.Play();
                    break;
                case Constants.ENEMY_SHOOT_SOUND:
                    enemyShoot.Play();
                    break;
                case Constants.EXPLOSION_SOUND:
                    explosion.Play();
                    break;
            }
        }
    }

    public void TurnOffSound()
    {
        gameMusic.volume = 0;
    }

    public void TurnOnSound()
    {
        gameMusic.volume = 1;
    }
}
