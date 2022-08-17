using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour
{
    public GameObject[] childs;

    public float limitAxisX;

    public Vector3
        firstPos,
        secondPos;

    // Behaviour messages
    void Update()
    {
        transform.position += new Vector3(-GameController._Instance.speedMove * Time.deltaTime, 0.0f, 0.0f);

        if (transform.localPosition.x <= limitAxisX)
        {
            GameController._Instance.ObstaclesIsActive = false;
            this.gameObject.SetActive(false);
        }
    }

    // Behaviour messages
    void OnEnable()
    {
        // Active childrens
        for (int i = childs.Length - 1; i >= 0; i--)
        {
            if (!childs[i].activeInHierarchy)
            {
                childs[i].SetActive(true);
            }
        }

        // Random position appear on left or right side
        if (Random.value <= 0.5f)
        {
            transform.localPosition = firstPos;
            if (tag != "ObMiddle")
            {
                tag = "ObTop";
            }
        }
        else
        {
            transform.localPosition = secondPos;
            if (tag != "ObMiddle")
            {
                tag = "ObBottom";
            }
        }
    }
}
