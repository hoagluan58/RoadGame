using UnityEngine;
using System.Collections;

public class OffScreen : MonoBehaviour
{
    private SpriteRenderer
        m_spriteRenderer,
        m_childRenderer;

    // Behaviour messages
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Behaviour messages
    void Start()
    {
        if (transform.childCount > 0)
        {
            m_childRenderer = transform.GetComponentsInChildren<SpriteRenderer>(true)[1];
        }
        else
        {
            m_childRenderer = null;
        }
    }

    // Behaviour messages
    void Update()
    {
        // Check if tile become invisible
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (!GeometryUtility.TestPlanesAABB(planes, m_spriteRenderer.bounds))
        {
            if (transform.position.x - Camera.main.transform.position.x < 0.0f)
            {
                CheckTile();
            }
        }
    }

    private void CheckTile()
    {
        if (this.tag == "Road")
        {
            Change(ref MapGenerator._Instance.lastPosOfRoadTile,
                new Vector3(1.5f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfRoad);
        }
        else if (this.tag == "TopNearGrass")
        {
            Change(ref MapGenerator._Instance.lastPosOfTopNearGrass,
                new Vector3(1.2f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfTopNearGrass);
        }
        else if (this.tag == "TopFarGrass")
        {
            Change(ref MapGenerator._Instance.lastPosOfTopFarGrass,
                new Vector3(4.8f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfTopFarGrass);
        }
        else if (this.tag == "BottomNearGrass")
        {
            Change(ref MapGenerator._Instance.lastPosOfBottomNearGrass,
                new Vector3(1.2f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfBottomNearGrass);
        }
        else if (this.tag == "BottomFarLand1")
        {
            Change(ref MapGenerator._Instance.lastPosOfBottomFarLand_F1,
                new Vector3(1.6f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfBottomFarLand_F1);
        }
        else if (this.tag == "BottomFarLand2")
        {
            Change(ref MapGenerator._Instance.lastPosOfBottomFarLand_F2,
                new Vector3(1.6f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfBottomFarLand_F2);
        }
        else if (this.tag == "BottomFarLand3")
        {
            Change(ref MapGenerator._Instance.lastPosOfBottomFarLand_F3,
                new Vector3(1.6f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfBottomFarLand_F3);
        }
        else if (this.tag == "BottomFarLand4")
        {
            Change(ref MapGenerator._Instance.lastPosOfBottomFarLand_F4,
                new Vector3(1.6f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfBottomFarLand_F4);
        }
        else if (this.tag == "BottomFarLand5")
        {
            Change(ref MapGenerator._Instance.lastPosOfBottomFarLand_F5,
                new Vector3(1.6f, 0.0f, 0.0f),
                ref MapGenerator._Instance.lastOrderOfBottomFarLand_F5);
        }
    }

    private void Change(ref Vector3 pos, Vector3 offset, ref int orderLayer)
    {
        transform.position = pos;
        pos += offset;

        m_spriteRenderer.sortingOrder = orderLayer;

        // Check if this object have child
        if (m_childRenderer != null)
        {
            if (m_childRenderer.sortingLayerName != "Tree top" && m_childRenderer.sortingLayerName != "Tree bottom")
            {
                m_childRenderer.sortingOrder = orderLayer;
            }

            // Random hide or show childs, just apply for top grass, bottom grass and bottom land
            if (Random.value >= 0.45f)
            {
                if (m_childRenderer.sortingLayerName != "Tree top" && m_childRenderer.sortingLayerName != "Tree bottom")
                {
                    m_spriteRenderer.enabled = false;
                }
                m_childRenderer.gameObject.SetActive(true);
            }
            else
            {
                m_spriteRenderer.enabled = true;
                m_childRenderer.gameObject.SetActive(false);
            }

            // Random hide or show water
            if (m_childRenderer.sortingLayerName == "Road")
            {
                if (GameController._Instance.CanActiveWater)
                {
                    GameController._Instance.CanActiveWater = false;

                    m_spriteRenderer.enabled = false;
                    m_childRenderer.GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = orderLayer + 1;
                    m_childRenderer.gameObject.SetActive(true);

                    if (m_childRenderer.tag == "Water1")
                    {
                        GameController._Instance.Water_1_IsActive = true;
                    }
                    else if (m_childRenderer.tag == "Water2")
                    {
                        GameController._Instance.Water_2_IsActive = true;
                    }
                    else if (m_childRenderer.tag == "Water3")
                    {
                        GameController._Instance.Water_3_IsActive = true;
                    }
                }
                else
                {
                    m_spriteRenderer.enabled = true;
                    m_childRenderer.gameObject.SetActive(false);

                    if (m_childRenderer.tag == "Water1")
                    {
                        GameController._Instance.Water_1_IsActive = false;
                    }
                    else if (m_childRenderer.tag == "Water2")
                    {
                        GameController._Instance.Water_2_IsActive = false;
                    }
                    else if (m_childRenderer.tag == "Water3")
                    {
                        GameController._Instance.Water_3_IsActive = false;
                    }
                }
            }
        }
        else
        {
            CheckTileBehindWaterTile(GameController._Instance.Water_1_IsActive, MapGenerator._Instance.posForWaterTile1);
            CheckTileBehindWaterTile(GameController._Instance.Water_2_IsActive, MapGenerator._Instance.posForWaterTile2);
            CheckTileBehindWaterTile(GameController._Instance.Water_3_IsActive, MapGenerator._Instance.posForWaterTile3);
        }

        orderLayer++;
    }

    private void CheckTileBehindWaterTile(bool waterIsActive, int posForWaterTile)
    {
        int index = posForWaterTile;
        if (index + 1 == MapGenerator._Instance.startRoadTile)
        {
            index = -1;
        }

        if (MapGenerator._Instance.roadTiles[posForWaterTile + 1] == this.gameObject)
        {
            if (waterIsActive)
            {
                m_spriteRenderer.enabled = false;
            }
            else
            {
                m_spriteRenderer.enabled = true;
            }
        }
    }
}
