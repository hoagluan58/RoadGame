using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour
{
    private int currentHero;

    public static HomeScript _Instance = null;

    public int totalHeroes;

    [Tooltip("price for heroes, start from 1 because hero 0 are already own")]
    public float[] heroesPrice;

    public ScrollSnapRect scrollSnapRect;

    public GameObject
        mainMenu,
        heroMenu,
        lockObj,
        starOfSelectBtn;

    public RectTransform scrollContent;

    public Text
        starText,
        selectText,
        priceText;

    public Image
        musicBtn,
        soundBtn,
        heroImage,
        selectBtn;

    public Sprite[]
        musicBtnSprite,
        soundBtnSprite,
        selectBtnSprite,
        heroImagesSprite;

    public AudioSource 
        bgMusic,
        click,
        buy;

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
    }

    // Behaviour messages
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetFloat(Constants.STAR_AMOUNT, 10000.0f);
        SetUp();
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

        starText.text = PlayerPrefs.GetFloat(Constants.STAR_AMOUNT, 0.0f) + "";
        scrollSnapRect.startingPage = PlayerPrefs.GetInt(Constants.SELECTED_HERO, 0);

        SetHeroStatusFirstTime();

        GetHeroImages();
    }

    // Set up the state of heroes for the first time play game
    private void SetHeroStatusFirstTime()
    {
        if (PlayerPrefs.GetFloat(Constants.BEST_SCORE, 0.0f) == 0.0f)
        {
            PlayerPrefs.SetInt("StateOfHero0", 1);
            for (int i = 1; i < totalHeroes; i++)
            {
                string heroStatus = "StateOfHero" + i;
                PlayerPrefs.SetInt(heroStatus, 0);
            }
        }
    }

    private void GetHeroImages()
    {
        heroImagesSprite = new Sprite[totalHeroes];
        for (int i = 0; i < heroImagesSprite.Length; i++)
        {
            string path = "Blocky Dash/images/Hero/hero" + i;
            heroImagesSprite[i] = Resources.Load<Sprite>(path);
        }

        heroImage.sprite = heroImagesSprite[PlayerPrefs.GetInt(Constants.SELECTED_HERO, 0)];
    }

    // Update buttons for hero page
    public void UpdateUIHeroPage(int heroIndex)
    {
        currentHero = heroIndex;

        string heroStatus = "StateOfHero" + heroIndex;
        if (PlayerPrefs.GetInt(heroStatus, 0) == 1)
        {
            if (heroIndex == PlayerPrefs.GetInt(Constants.SELECTED_HERO, 0))
            {
                SetUpHeroPageButtons(selectBtnSprite[0], "SELECTED", true, false, false, false);
            }
            else
            {
                SetUpHeroPageButtons(selectBtnSprite[1], "SELECT", true, false, false, false);
            }
        }
        else
        {
            SetUpHeroPageButtons(selectBtnSprite[1], "SELECT", false, true, true, true);
            priceText.text = heroesPrice[heroIndex - 1] + "";
        }
    }

    private void SetUpHeroPageButtons(Sprite sprite, string str, bool selectTextEnabled,
        bool priceTextEnabled, bool starOfSelectBtnActive, bool lockObjActive)
    {
        selectBtn.sprite = sprite;
        selectText.text = str;
        selectText.enabled = selectTextEnabled;
        priceText.enabled = priceTextEnabled;
        starOfSelectBtn.SetActive(starOfSelectBtnActive);
        lockObj.SetActive(lockObjActive);
    }

    // Play button is clicked
    public void PlayBtn_Onclick()
    {
        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }
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

    // Hero button is clicked
    public void HeroBtn_Onclick()
    {
        mainMenu.SetActive(false);
        heroMenu.SetActive(true);

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }
    }

    // Home button is clicked
    public void HomeBtn_Onclick()
    {
        heroMenu.SetActive(false);

        heroImage.sprite = heroImagesSprite[PlayerPrefs.GetInt(Constants.SELECTED_HERO, 0)];
        mainMenu.SetActive(true);

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }
    }

    // Select button is clicked
    public void SelectBtn_Onclick()
    {
        string heroStatus = "StateOfHero" + currentHero;
        if (PlayerPrefs.GetInt(heroStatus, 0) == 1)
        {
            SetUpHeroPageButtons(selectBtnSprite[0], "SELECTED", true, false, false, false);
            PlayerPrefs.SetInt(Constants.SELECTED_HERO, currentHero);
        }
        else
        {
            float starAmount = PlayerPrefs.GetFloat(Constants.STAR_AMOUNT, 0.0f);
            if (starAmount >= heroesPrice[currentHero - 1])
            {
                SetUpHeroPageButtons(selectBtnSprite[1], "SELECT", true, false, false, false);
                PlayerPrefs.SetInt(heroStatus, 1);
                starAmount -= heroesPrice[currentHero - 1];
                PlayerPrefs.SetFloat(Constants.STAR_AMOUNT, starAmount);
                starText.text = starAmount + "";

                // Sound
                if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
                {
                    buy.Play();
                }
            }
        }

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            click.Play();
        }
    }

	public void FacebookShare()
	{
		//FaceBookManager.Instance.ShareOnFB ();
		click.Play ();
	}

	public void MoreGame()
	{
		click.Play ();
	}
}
