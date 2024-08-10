using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject
        playSection,
        shopSection,
        loading;

    public AudioSource
        clickSource,
        gameMusic;

    public RectTransform loadingProgress;

    public RectTransform scrollContent;

    public GameObject[]
        checkedCharacter,
        characterPrice,
        availableCharacter;

    public GameObject purchaseOrNot;

    public Text coinAmount;

    private Vector2
        startPos,
        endPos;

    private float timeStartedLerp;

    private int currentPrice;

    private bool isLerping;

    // Behaviour messages
    void Start()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            gameMusic.volume = 1;
        }
        else
        {
            gameMusic.volume = 0;
        }

        coinAmount.text = PlayerPrefs.GetFloat(Constants.COIN, 0) + "";
        CheckCharacterState();
        SetChecked();
    }

    private void SetChecked()
    {
        int index = PlayerPrefs.GetInt(Constants.CHARACTER_SELECT, 1);

        checkedCharacter[index - 1].SetActive(true);
        for (int i = checkedCharacter.Length - 1; i >= 0; i--)
        {
            if (i != index)
            {
                if (checkedCharacter[i].activeInHierarchy)
                {
                    checkedCharacter[i].SetActive(false);
                }
            }
        }
    }

    private void CheckCharacterState()
    {
        if (PlayerPrefs.GetInt("CharState2", 0) == 1)
        {
            characterPrice[0].SetActive(false);
            availableCharacter[0].SetActive(true);
        }
        if (PlayerPrefs.GetInt("CharState3", 0) == 1)
        {
            characterPrice[1].SetActive(false);
            availableCharacter[1].SetActive(true);
        }
        if (PlayerPrefs.GetInt("CharState4", 0) == 1)
        {
            characterPrice[2].SetActive(false);
            availableCharacter[2].SetActive(true);
        }
        if (PlayerPrefs.GetInt("CharState5", 0) == 1)
        {
            characterPrice[3].SetActive(false);
            availableCharacter[3].SetActive(true);
        }
        if (PlayerPrefs.GetInt("CharState6", 0) == 1)
        {
            characterPrice[4].SetActive(false);
            availableCharacter[4].SetActive(true);
        }
        if (PlayerPrefs.GetInt("CharState7", 0) == 1)
        {
            characterPrice[5].SetActive(false);
            availableCharacter[5].SetActive(true);
        }
        if (PlayerPrefs.GetInt("CharState8", 0) == 1)
        {
            characterPrice[6].SetActive(false);
            availableCharacter[6].SetActive(true);
        }
    }

    // Left arrow button is clicked
    public void LeftArrowBtn_Onclick()
    {
        if (scrollContent.anchoredPosition.x >= -651.0f)
        {
            isLerping = true;
            timeStartedLerp = Time.time;
            startPos = scrollContent.anchoredPosition;
            endPos = new Vector2(scrollContent.anchoredPosition.x - 220.0f, scrollContent.anchoredPosition.y);
        }

        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }
    }

    // Right arrow button is clicked
    public void RightArrowBtn_Onclick()
    {
        if (scrollContent.anchoredPosition.x <= 651.0f)
        {
            isLerping = true;
            timeStartedLerp = Time.time;
            startPos = scrollContent.anchoredPosition;
            endPos = new Vector2(scrollContent.anchoredPosition.x + 220.0f, scrollContent.anchoredPosition.y);
        }

        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }
    }

    // Behaviour messages
    void Update()
    {
        if (isLerping)
        {
            float percentage = (Time.time - timeStartedLerp) / 0.5f;

            scrollContent.anchoredPosition = Vector2.Lerp(startPos, endPos, percentage);

            if (percentage >= 1.0f)
            {
                isLerping = false;
            }
        }
    }

    // Character button is clicked
    public void BuyCharacterBtn_Onclick(int price)
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }

        int exceptIndex = PlayerPrefs.GetInt(Constants.CHARACTER_SELECT, 1) - 1;

        if (price == 0)
        {
            PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 1);
            checkedCharacter[0].SetActive(true);
            exceptIndex = 0;
        }
        else if (price == 550)
        {
            if (PlayerPrefs.GetInt("CharState2", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 2);
                checkedCharacter[1].SetActive(true);
                exceptIndex = 1;
            }
            else
            {
                currentPrice = 550;
                purchaseOrNot.SetActive(true);
            }
        }
        else if (price == 650)
        {
            if (PlayerPrefs.GetInt("CharState3", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 3);
                checkedCharacter[2].SetActive(true);
                exceptIndex = 2;
            }
            else
            {
                currentPrice = 650;
                purchaseOrNot.SetActive(true);
            }
        }
        else if (price == 750)
        {
            if (PlayerPrefs.GetInt("CharState4", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 4);
                checkedCharacter[3].SetActive(true);
                exceptIndex = 3;
            }
            else
            {
                currentPrice = 750;
                purchaseOrNot.SetActive(true);
            }
        }
        else if (price == 850)
        {
            if (PlayerPrefs.GetInt("CharState5", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 5);
                checkedCharacter[4].SetActive(true);
                exceptIndex = 4;
            }
            else
            {
                currentPrice = 850;
                purchaseOrNot.SetActive(true);
            }
        }
        else if (price == 950)
        {
            if (PlayerPrefs.GetInt("CharState6", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 6);
                checkedCharacter[5].SetActive(true);
                exceptIndex = 5;
            }
            else
            {
                currentPrice = 950;
                purchaseOrNot.SetActive(true);
            }
        }
        else if (price == 1050)
        {
            if (PlayerPrefs.GetInt("CharState7", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 7);
                checkedCharacter[6].SetActive(true);
                exceptIndex = 6;
            }
            else
            {
                currentPrice = 1050;
                purchaseOrNot.SetActive(true);
            }
        }
        else if (price == 1150)
        {
            if (PlayerPrefs.GetInt("CharState8", 0) == 1)
            {
                PlayerPrefs.SetInt(Constants.CHARACTER_SELECT, 8);
                checkedCharacter[7].SetActive(true);
                exceptIndex = 7;
            }
            else
            {
                currentPrice = 1150;
                purchaseOrNot.SetActive(true);
            }
        }

        OffCheck(exceptIndex);
    }

    private void OffCheck(int except)
    {
        for (int i = checkedCharacter.Length - 1; i >= 0; i--)
        {
            if (i != except)
            {
                if (checkedCharacter[i].activeInHierarchy)
                {
                    checkedCharacter[i].SetActive(false);
                }
            }
        }
    }

    // Buy button is clicked
    public void BuyBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }

        float coin = PlayerPrefs.GetFloat(Constants.COIN, 0);
        if (coin >= currentPrice)
        {
            coin -= currentPrice;
            PlayerPrefs.SetFloat(Constants.COIN, coin);
            coinAmount.text = coin + "";

            int index = 0;

            if (currentPrice == 550)
            {
                PlayerPrefs.SetInt("CharState2", 1);
                index = 0;
            }
            else if (currentPrice == 650)
            {
                PlayerPrefs.SetInt("CharState3", 1);
                index = 1;
            }
            else if (currentPrice == 750)
            {
                PlayerPrefs.SetInt("CharState4", 1);
                index = 2;
            }
            else if (currentPrice == 850)
            {
                PlayerPrefs.SetInt("CharState5", 1);
                index = 3;
            }
            else if (currentPrice == 950)
            {
                PlayerPrefs.SetInt("CharState6", 1);
                index = 4;
            }
            else if (currentPrice == 1050)
            {
                PlayerPrefs.SetInt("CharState7", 1);
                index = 5;
            }
            else if (currentPrice == 1150)
            {
                PlayerPrefs.SetInt("CharState8", 1);
                index = 6;
            }

            characterPrice[index].SetActive(false);
            availableCharacter[index].SetActive(true);

            purchaseOrNot.SetActive(false);
        }
    }

    // Cancel button is clicked
    public void CancelBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }

        purchaseOrNot.SetActive(false);
    }

    // Menu button is clicked
    public void MenuBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }

        shopSection.SetActive(false);
        playSection.SetActive(true);
    }

    // Shop button is clicked
    public void ShopBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }

        playSection.SetActive(false);
        shopSection.SetActive(true);
    }

    // Quit button is clicked
    public void QuitBtn_Onclick()
    {
        Application.Quit();
    }

    // Play button is clicked
    public void PlayGameBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            clickSource.Play();
        }

        playSection.SetActive(false);
        loading.SetActive(true);

        StartCoroutine(LoadPLayScene());
    }

    private IEnumerator LoadPLayScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Play");

        while (!async.isDone)
        {
            loadingProgress.localScale = new Vector3(Mathf.Clamp01(async.progress), 1.0f, 1.0f);
            yield return null;
        }
    }
}
