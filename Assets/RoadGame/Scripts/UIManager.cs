using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Animator pauseMenuAnim;

    public static UIManager _Instance = null;

    public GameObject 
        pauseMenu,
        gameOverMenu,
        tutorial;

    public Text 
        scoreText,
        starText,
        gameOverScoreText,
        gameOverBestScoreText,
        gameOverStarText;

    public Sprite[] 
        musicBtnSprite,
        soundBtnSprite;

    public Image
        musicBtn,
        soundBtn;

    public AudioSource
        bgMusic,
        gameOver,
        click,
        jump,
        bounce,
        move,
        coin,
        hit,
        power,
        water;

    // Behaviour messages
    void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else if (_Instance != null)
        {
            Destroy(this.gameObject);
        }

        pauseMenuAnim = pauseMenu.GetComponent<Animator>();
    }

    // Behaviour messages
    void Start()
    {
        StartCoroutine(TutorialDuration());
        SetUp();
    }

    private IEnumerator TutorialDuration()
    {
        yield return new WaitForSeconds(3.0f);
        tutorial.SetActive(false);
    }

    private void SetUp()
    {
        // Music
        if (PlayerPrefs.GetInt(Constants.MUSIC_STATE, 1) == 1)
        {
            musicBtn.sprite = musicBtnSprite[0];
            bgMusic.Play();
        }
        else
        {
            musicBtn.sprite = musicBtnSprite[1];
        }

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            soundBtn.sprite = soundBtnSprite[0];
        }
        else
        {
            soundBtn.sprite = soundBtnSprite[1];
        }
    }

    // Pause button is clicked
    public void PauseBtn_Onclick()
    {
        if (!PlayerController._Instance.IsDie)
        {
            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                click.Play();
            }

            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
    }

    public void UpdateGameOver(float bestScore, float starAmount)
    {
        gameOverScoreText.text = scoreText.text;
        gameOverBestScoreText.text = "BEST: " + bestScore;
        gameOverStarText.text = starAmount + "";
    }

    // Home button is clicked
    public void HomeBtn_Onclick()
    {
        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Home");
    }

    // Play button is clicked
    public void ResumeBtn_Onclick()
    {
        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }

        pauseMenuAnim.SetTrigger("End");
    }

    // Restart button is clicked
    public void RestartBtn_Onclick()
    {
        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Play");
    }

    // Music button is clicked
    public void MusicBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.MUSIC_STATE, 1) == 1)
        {
            musicBtn.sprite = musicBtnSprite[1];
            PlayerPrefs.SetInt(Constants.MUSIC_STATE, 0);
            bgMusic.Stop();
        }
        else
        {
            musicBtn.sprite = musicBtnSprite[0];
            PlayerPrefs.SetInt(Constants.MUSIC_STATE, 1);
            bgMusic.Play();
        }

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }
    }

    // Sound button is clicked
    public void SoundBtn_Onclick()
    {
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            soundBtn.sprite = soundBtnSprite[1];
            PlayerPrefs.SetInt(Constants.SOUND_STATE, 0);
        }
        else
        {
            soundBtn.sprite = soundBtnSprite[0];
            PlayerPrefs.SetInt(Constants.SOUND_STATE, 1);

            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                click.Play();
            }
        }
    }
}
