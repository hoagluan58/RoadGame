using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private float m_starAmount;

    private int
        m_moveHash,
        m_jumpHash;

    private bool
        m_changeLineIsPressed,
        m_jumpIsPressed;                            

    public static PlayerController _Instance = null;

    public GameObject 
        playerObj,
        shadow,
        waterDrop,
        explosion1,
        explosion2;

    public GameObject[] starEffect;

    public Vector3
        firstPosOfPlayer,
        secondPosOfPlayer;

    public Animator playerAnim;

    public SpriteRenderer 
        playerRenderer,
        trexRenderer;

    public bool PlayerIsTop { get; set; }
    public bool TrexTrigger { get; set; }
    public bool IsDie { get; set; }

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
        string path = "Blocky Dash/images/Hero/hero" + PlayerPrefs.GetInt(Constants.SELECTED_HERO, 0) + "_big";
        playerRenderer.sprite = Resources.Load<Sprite>(path);

        m_moveHash = Animator.StringToHash("Move");
        m_jumpHash = Animator.StringToHash("Jump");
    }

    // Behaviour messages
    void Update()
    {
        HandleChangeLine();
        HandleJump();
    }

    private void HandleChangeLine()
    {
        if (m_changeLineIsPressed)
        {
            m_changeLineIsPressed = false;

            playerAnim.SetBool(m_moveHash, true);

            if (!PlayerIsTop)
            {
                transform.localPosition = secondPosOfPlayer;
                PlayerIsTop = true;

                // Make realistic as you go through the obstacles
                playerRenderer.sortingOrder = -1;
                trexRenderer.sortingOrder = -1;
            }
            else
            {
                transform.localPosition = firstPosOfPlayer;
                PlayerIsTop = false;

                // Make realistic as you go through the obstacles
                playerRenderer.sortingOrder = 10;
                trexRenderer.sortingOrder = 10;
            }

            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                UIManager._Instance.move.Play();
            }
        }
    }

    private void HandleJump()
    {
        if (m_jumpIsPressed)
        {
            m_jumpIsPressed = false;

            playerAnim.SetBool(m_jumpHash, true);
            playerRenderer.sortingOrder = 10;

            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                UIManager._Instance.jump.Play();
            }
        }
    }

    // Event trigger
    public void ChangeLinePointerDown()
    {
        m_changeLineIsPressed = true;
    }

    // Event trigger
    public void JumpPointerDown()
    {
        m_jumpIsPressed = true;
    }

    // Behaviour messages
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water1" || other.tag == "Water2" || other.tag == "Water3")
        {
            if (TrexTrigger)
            {
                TrexTrigger = false;
                trexRenderer.gameObject.SetActive(false);
            }
            else
            {
                Die();
                waterDrop.SetActive(true);
            }

            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                UIManager._Instance.water.Play();
            }
        }
        if (other.tag == "Obstacle")
        {
            if (other.transform.parent.tag == "ObMiddle")
            {
                if (!TrexTrigger)
                {
                    DieWithObstacle(other);
                }
                else
                {
                    DestroyObstacle(other);
                }
            }
            else
            {
                if (other.transform.parent.tag == "ObTop")
                {
                    if (PlayerIsTop)
                    {
                        if (!TrexTrigger)
                        {
                            DieWithObstacle(other);
                        }
                        else
                        {
                            DestroyObstacle(other);
                        }
                    }
                }
                else if (other.transform.parent.tag == "ObBottom")
                {
                    if (!PlayerIsTop)
                    {
                        if (!TrexTrigger)
                        {
                            DieWithObstacle(other);
                        }
                        else
                        {
                            DestroyObstacle(other);
                        }
                    }
                }
            }
        }
        if (other.tag == "Trex")
        {
            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                UIManager._Instance.power.Play();
            }

            TrexTrigger = true;
            trexRenderer.gameObject.SetActive(true);
            other.gameObject.SetActive(false);

            StartCoroutine(TrexDuration());
        }
        if (other.tag == "Star")
        {
            for (int i = starEffect.Length - 1; i >= 0; i--)
            {
                if (!starEffect[i].activeInHierarchy)
                {
                    starEffect[i].transform.position = other.transform.position;
                    starEffect[i].SetActive(true);
                    break;
                }
            }
            other.gameObject.SetActive(false);

            // Sound
            if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
            {
                UIManager._Instance.coin.Play();
            }

            UpdateStar();
        }
    }

    private void UpdateStar()
    {
        m_starAmount++;
        UIManager._Instance.starText.text = m_starAmount + "";
    }

    private IEnumerator TrexDuration()
    {
        yield return new WaitForSeconds(7.0f);
        if (TrexTrigger)
        {
            TrexTrigger = false;
            trexRenderer.GetComponent<Animator>().SetTrigger("Die");
        }
    }

    private void DestroyObstacle(Collider2D other)
    {
        explosion2.transform.position = other.transform.position;

        explosion2.SetActive(false);    // turn off if it's playing
        explosion2.SetActive(true);

        other.gameObject.SetActive(false);

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            UIManager._Instance.hit.Play();
        }
    }

    private void Die()
    {
        IsDie = true;
        playerRenderer.enabled = false;
        playerObj.SetActive(false);
        shadow.SetActive(false);

        GameController._Instance.speedMove = 0.0f;
        GameController._Instance.GameOver(m_starAmount);
    }

    private void DieWithObstacle(Collider2D other)
    {
        Die();
        explosion1.transform.position = other.transform.position;
        explosion1.SetActive(true);
        other.gameObject.SetActive(false);

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            UIManager._Instance.hit.Play();
        }
    }
}
