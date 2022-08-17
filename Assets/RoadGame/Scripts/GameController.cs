using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private float m_distanceMove;

    private bool m_gameJustStart;

    public static GameController _Instance = null;

    public float
        speedMove,
        distanceFactor;

    public GameObject obstaclesObj;

    public GameObject[] obstacleList;

    public bool Water_1_IsActive { get; set; }
    public bool Water_2_IsActive { get; set; }
    public bool Water_3_IsActive { get; set; }
    public bool CanActiveWater { get; set; }
    public bool ObstaclesIsActive { get; set; }

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
		Application.targetFrameRate = 60;
    }

    // Behaviour messages
    void Start()
    {
        m_gameJustStart = true;

        GetObstacles();
        StartCoroutine(SpawObstacles());
    }

    private void GetObstacles()
    {
        obstacleList = new GameObject[obstaclesObj.transform.childCount];

        for (int i = obstaclesObj.transform.childCount - 1; i >= 0; i--)
        {
            obstacleList[i] = obstaclesObj.GetComponentsInChildren<ObstacleScript>(true)[i].gameObject;
        }
    }

    private IEnumerator SpawObstacles()
    {
        while (true)
        {
            if (!PlayerController._Instance.IsDie)
            {
                if (!ObstaclesIsActive && !Water_1_IsActive && !Water_2_IsActive && !Water_3_IsActive)
                {
                    if (Random.value <= 0.85f)
                    {
                        int randomIndex = 0;
                        do
                        {
                            randomIndex = Mathf.RoundToInt(Random.Range(0.0f, 19.0f));
                        } while (obstacleList[randomIndex].activeInHierarchy);

                        obstacleList[randomIndex].SetActive(true);
                        ObstaclesIsActive = true;
                    }
                    else
                    {
                        CanActiveWater = true;
                    }
                }
            }
            yield return new WaitForSeconds(0.6f);
        }
    }

    // Behaviour messages
    void Update()
    {
        if (m_gameJustStart)
        {
            if (!PlayerController._Instance.IsDie)
            {
                if (speedMove < 12.0f)
                {
                    speedMove += Time.deltaTime * 5.0f;
                }
                else
                {
                    speedMove = 12.0f;
                    m_gameJustStart = false;
                }
            }
        }

        if (!PlayerController._Instance.IsDie)
        {
            Camera.main.transform.position += new Vector3(speedMove * Time.deltaTime, 0.0f, 0.0f);
            UpdateDistance();
        }
    }

    private void UpdateDistance()
    {
        m_distanceMove += Time.deltaTime * distanceFactor;
        float round = Mathf.Round(m_distanceMove);
        UIManager._Instance.scoreText.text = round + "";

        if (round >= 30.0f && round < 60.0f)
        {
            speedMove = 14.0f;
        }
        else if (round >= 60.0f)
        {
            speedMove = 16.0f;
        }
    }

    public void GameOver(float star)
    {
        float round = Mathf.Round(m_distanceMove);
        float bestScore = PlayerPrefs.GetFloat(Constants.BEST_SCORE, 0);
        
        float starAmount = PlayerPrefs.GetFloat(Constants.STAR_AMOUNT, 0);
        starAmount += star;
        PlayerPrefs.SetFloat(Constants.STAR_AMOUNT, starAmount);

        if (round > bestScore)
        {
            PlayerPrefs.SetFloat(Constants.BEST_SCORE, round);
            UIManager._Instance.UpdateGameOver(round, star);
        }
        else
        {
            UIManager._Instance.UpdateGameOver(bestScore, star);
        }

        UIManager._Instance.gameOverMenu.SetActive(true);

        // Sound
        if (PlayerPrefs.GetInt(Constants.MUSIC_STATE, 1) == 1)
        {
            UIManager._Instance.bgMusic.Stop();
            UIManager._Instance.gameOver.Play();
        }
		//if (AdsControl.Instance != null)
		//	AdsControl.Instance.showAds ();
    }

	public void ShareFB()
	{
		//if (FaceBookManager.Instance != null)
		//	FaceBookManager.Instance.ShareOnFB ();
	}
}
