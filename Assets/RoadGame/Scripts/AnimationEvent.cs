using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour
{
    // Animation Event
    public void EndChangeLine()
    {
        GetComponent<Animator>().SetBool("Move", false);
    }

    // Animation Event
    public void Jumping1()
    {
        PlayerController._Instance.playerRenderer.sortingOrder = 10;
    }

    // Animation Event
    public void Jumping2()
    {
        if (PlayerController._Instance.PlayerIsTop)
        {
            PlayerController._Instance.playerRenderer.sortingOrder = -1;
        }
        else
        {
            PlayerController._Instance.playerRenderer.sortingOrder = 10;
        }
    }

    // Animation Event
    public void EndJump()
    {
        GetComponent<Animator>().SetBool("Jump", false);

        // Sound
        if (PlayerPrefs.GetInt(Constants.SOUND_STATE, 1) == 1)
        {
            UIManager._Instance.bounce.Play();
        }
    }

    // Animation Event
    public void EndTrex()
    {
        this.gameObject.SetActive(false);
    }

    // Animation Event
    public void EndExplosion2()
    {
        this.gameObject.SetActive(false);
    }

    // Animation Event
    public void EndStarEffect()
    {
        this.gameObject.SetActive(false);
    }

    // Animation Event
    public void EndPauseMenu()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }
}
