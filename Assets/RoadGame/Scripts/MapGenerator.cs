using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator _Instance = null;

    public GameObject
        roadPrefab,
        grassPrefab,
        ground1Prefab,
        ground2Prefab,
        ground3Prefab,
        ground4Prefab,
        grassBottomPrefab,
        landPrefab,
        land1Prefab,
        land2Prefab,
        land3Prefab,
        land4Prefab,
        bigGrassPrefab,
        bigGrassBottomPrefab,
        tree1Prefab,
        tree2Prefab,
        tree3Prefab,
        bigTreePrefab,
        water1Prefab,
        water2Prefab,
        water3Prefab;

    public GameObject
        roadHolder,
        topNearSideWalkHolder,
        topFarSideWalkHolder,
        bottomNearSideWalkHolder,
        bottomFarSideWalkHolder;

    public int
        startRoadTile,      // initialization number of 'road' tiles
        startGrassTile,     // initialization number of 'grass' tiles
        startGround3Tile,   // initialization number of 'ground3' tiles
        startLandTile;      // initialization number of 'land' tiles

    public List<GameObject>
        roadTiles,
        topNearGrassTiles,
        topFarGrassTiles,
        bottomNearGrassTiles,
        bottomFarLand_F1_Tiles,
        bottomFarLand_F2_Tiles,
        bottomFarLand_F3_Tiles,
        bottomFarLand_F4_Tiles,
        bottomFarLand_F5_Tiles;

    // Top
    [Tooltip("Positions for 'ground1' on top, from '0' to 'startGround3Tile'")]
    public int[] posForTopGround1;

    [Tooltip("Positions for 'ground2' on top, from '0' to 'startGround3Tile'")]
    public int[] posForTopGround2;

    [Tooltip("Positions for 'ground4' on top, from '0' to 'startGround3Tile'")]
    public int[] posForTopGround4;

    [Tooltip("Positions for big grass with tree on top near grass, from '0' to 'startGrassTile'")]
    public int[] posForTopBigGrass;

    [Tooltip("Positions for 'tree1' on top near grass, from '0' to 'startGrassTile'")]
    public int[] posForTopTree1;

    [Tooltip("Positions for 'tree2' on top near grass, from '0' to 'startGrassTile'")]
    public int[] posForTopTree2;

    [Tooltip("Positions for 'tree3' on top near grass, from '0' to 'startGrassTile'")]
    public int[] posForTopTree3;

    // Middle
    [Tooltip("Positions for 'water1' on road, from '0' to 'startRoadTile'")]
    public int posForWaterTile1;

    [Tooltip("Positions for 'water2' on road, from '0' to 'startRoadTile'")]
    public int posForWaterTile2;

    [Tooltip("Positions for 'water3' on road, from '0' to 'startRoadTile'")]
    public int posForWaterTile3;

    // Bottom
    [Tooltip("Positions for big grass with tree on bottom near grass, from '0' to 'startGrassTile'")]
    public int[] posForBottomBigGrass;

    [Tooltip("Positions for 'tree1' on bottom near grass, from '0' to 'startGrassTile'")]
    public int[] posForBottomTree1;

    [Tooltip("Positions for 'tree2' on bottom near grass, from '0' to 'startGrassTile'")]
    public int[] posForBottomTree2;

    [Tooltip("Positions for 'tree3' on bottom near grass, from '0' to 'startGrassTile'")]
    public int[] posForBottomTree3;

    [HideInInspector]
    public Vector3
        lastPosOfRoadTile,
        lastPosOfTopNearGrass,
        lastPosOfTopFarGrass,
        lastPosOfBottomNearGrass,
        lastPosOfBottomFarLand_F1,
        lastPosOfBottomFarLand_F2,
        lastPosOfBottomFarLand_F3,
        lastPosOfBottomFarLand_F4,
        lastPosOfBottomFarLand_F5;

    [HideInInspector]
    public int
        lastOrderOfRoad,
        lastOrderOfTopNearGrass,
        lastOrderOfTopFarGrass,
        lastOrderOfBottomNearGrass,
        lastOrderOfBottomFarLand_F1,
        lastOrderOfBottomFarLand_F2,
        lastOrderOfBottomFarLand_F3,
        lastOrderOfBottomFarLand_F4,
        lastOrderOfBottomFarLand_F5;

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
        Initialize();
    }

    private void Initialize()
    {
        // Initialize road
        InitializePlatform(roadPrefab, ref lastPosOfRoadTile, roadPrefab.transform.position,
            startRoadTile, roadHolder, ref roadTiles, ref lastOrderOfRoad, new Vector3(1.5f, 0.0f, 0.0f));

        // Initialize top near grass
        InitializePlatform(grassPrefab, ref lastPosOfTopNearGrass, grassPrefab.transform.position,
            startGrassTile, topNearSideWalkHolder, ref topNearGrassTiles, ref lastOrderOfTopNearGrass, new Vector3(1.2f, 0.0f, 0.0f));

        // Initialize top far grass
        InitializePlatform(ground3Prefab, ref lastPosOfTopFarGrass, ground3Prefab.transform.position,
            startGround3Tile, topFarSideWalkHolder, ref topFarGrassTiles, ref lastOrderOfTopFarGrass, new Vector3(4.8f, 0.0f, 0.0f));

        // Initialize bottom near grass
        InitializePlatform(grassBottomPrefab, ref lastPosOfBottomNearGrass, new Vector3(2.0f, grassBottomPrefab.transform.position.y, 0.0f),
            startGrassTile, bottomNearSideWalkHolder, ref bottomNearGrassTiles, ref lastOrderOfBottomNearGrass, new Vector3(1.2f, 0.0f, 0.0f));

        // Initialize bottom far land
        InitializeBottomFarLand();
    }

    private void InitializePlatform(GameObject prefab, ref Vector3 lastPos, Vector3 lastPosOfTile, int amountTile,
        GameObject holder, ref List<GameObject> listTile, ref int lastOrder, Vector3 offset)
    {
        int orderInLayer = 0;
        lastPos = lastPosOfTile;

        for (int i = amountTile - 1; i >= 0; i--)
        {
            GameObject clone = Instantiate(prefab, lastPos, prefab.transform.rotation) as GameObject;
            clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

            if (clone.tag == "TopNearGrass")
            {
                SetNearScene(bigGrassPrefab, ref clone, ref orderInLayer, posForTopBigGrass,
                    posForTopTree1, posForTopTree2, posForTopTree3);
            }
            else if (clone.tag == "Road")
            {
                CreateWater(ref clone, ref orderInLayer);
            }
            else if (clone.tag == "BottomNearGrass")
            {
                SetNearScene(bigGrassBottomPrefab, ref clone, ref orderInLayer, posForBottomBigGrass,
                    posForBottomTree1, posForBottomTree2, posForBottomTree3);
            }
            else if (clone.tag == "BottomFarLand2")
            {
                if (orderInLayer == 5)
                {
                    CreateTreeOrGround(bigTreePrefab, ref clone, new Vector3(-0.57f, -1.34f, 0.0f));
                }
            }
            else if (clone.tag == "TopFarGrass")
            {
                CreateGround(ref clone, ref orderInLayer);
            }

            clone.transform.SetParent(holder.transform);

            listTile.Add(clone);

            orderInLayer += 1;
            lastOrder = orderInLayer;

            lastPos += offset;
        }
    }

    // Create water
    private void CreateWater(ref GameObject tileClone, ref int orderInLayer)
    {
        GameObject clone = null;

        if (orderInLayer == posForWaterTile1)
        {
            clone = Instantiate(water1Prefab, tileClone.transform.position, water3Prefab.transform.rotation) as GameObject;
        }
        else if (orderInLayer == posForWaterTile2)
        {
            clone = Instantiate(water2Prefab, tileClone.transform.position, water3Prefab.transform.rotation) as GameObject;
        }
        else if (orderInLayer == posForWaterTile3)
        {
            clone = Instantiate(water3Prefab, tileClone.transform.position, water3Prefab.transform.rotation) as GameObject;
        }

        if (clone != null)
        {
            clone.transform.SetParent(tileClone.transform);
            clone.transform.localPosition = new Vector3(0.079f, -0.38f);
            clone.gameObject.SetActive(false);
        }
    }

    // Create Scene on grass
    private void SetNearScene(GameObject bigGrassPrefab, ref GameObject clone, ref int orderInLayer, int[] posForBigGrass,
        int[] posForTree1, int[] posForTree2, int[] posForTree3)
    {
        for (int i = posForBigGrass.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForBigGrass[i])
            {
                CreateScene(bigGrassPrefab, ref clone, orderInLayer);
                break;
            }
        }

        for (int i = posForTree1.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForTree1[i])
            {
                CreateTreeOrGround(tree1Prefab, ref clone, new Vector3(0.0f, 1.15f, 0.0f));
                break;
            }
        }

        for (int i = posForTree2.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForTree2[i])
            {
                CreateTreeOrGround(tree2Prefab, ref clone, new Vector3(0.0f, 1.15f, 0.0f));
                break;
            }
        }

        for (int i = posForTree3.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForTree3[i])
            {
                CreateTreeOrGround(tree3Prefab, ref clone, new Vector3(0.0f, 1.15f, 0.0f));
                break;
            }
        }
    }

    // Create big grass, tree
    private void CreateScene(GameObject bigGrassPrefab, ref GameObject tileClone, int orderInLayer)
    {
        GameObject clone = Instantiate(bigGrassPrefab, tileClone.transform.position, bigGrassPrefab.transform.rotation) as GameObject;
        clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
        clone.transform.SetParent(tileClone.transform);
        clone.transform.localPosition = new Vector3(-0.183f, 0.106f, 0.0f);

        CreateTreeOrGround(tree1Prefab, ref clone, new Vector3(0.0f, 1.52f, 0.0f));

        // Turn off parent tile to show childs tile
        tileClone.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Create only tree or ground
    private void CreateTreeOrGround(GameObject prefab, ref GameObject tileClone, Vector3 localPos)
    {
        GameObject clone = Instantiate(prefab, tileClone.transform.position, prefab.transform.rotation) as GameObject;

        SpriteRenderer tileCloneRender = tileClone.GetComponent<SpriteRenderer>();
        SpriteRenderer cloneRender = clone.GetComponent<SpriteRenderer>();

        if (tileCloneRender.sortingLayerName == "Top near ground")
        {
            cloneRender.sortingLayerName = "Tree top";
        }
        else if (tileCloneRender.sortingLayerName == "Bottom near ground")
        {
            cloneRender.sortingLayerName = "Tree bottom";
        }

        cloneRender.sortingOrder = tileCloneRender.sortingOrder;
        clone.transform.SetParent(tileClone.transform);
        clone.transform.localPosition = localPos;

        if (prefab == ground1Prefab || prefab == ground2Prefab || prefab == ground4Prefab)
        {
            tileCloneRender.enabled = false;
        }
    }

    // Create only grounds
    private void CreateGround(ref GameObject clone, ref int orderInLayer)
    {
        for (int i = posForTopGround1.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForTopGround1[i])
            {
                CreateTreeOrGround(ground1Prefab, ref clone, Vector3.zero);
                break;
            }
        }

        for (int i = posForTopGround2.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForTopGround2[i])
            {
                CreateTreeOrGround(ground2Prefab, ref clone, Vector3.zero);
                break;
            }
        }

        for (int i = posForTopGround4.Length - 1; i >= 0; i--)
        {
            if (orderInLayer == posForTopGround4[i])
            {
                CreateTreeOrGround(ground4Prefab, ref clone, Vector3.zero);
                break;
            }
        }
    }

    private void InitializeBottomFarLand()
    {
        // Floor 1
        InitializePlatform(landPrefab, ref lastPosOfBottomFarLand_F1, landPrefab.transform.position, startLandTile,
            bottomFarSideWalkHolder, ref bottomFarLand_F1_Tiles, ref lastOrderOfBottomFarLand_F1, new Vector3(1.6f, 0.0f, 0.0f));

        // Floor 2
        InitializePlatform(land1Prefab, ref lastPosOfBottomFarLand_F2, land1Prefab.transform.position, startLandTile - 3,
            bottomFarSideWalkHolder, ref bottomFarLand_F2_Tiles, ref lastOrderOfBottomFarLand_F2, new Vector3(1.6f, 0.0f, 0.0f));

        // Floor 3
        InitializePlatform(land2Prefab, ref lastPosOfBottomFarLand_F3, land2Prefab.transform.position, startLandTile - 4,
            bottomFarSideWalkHolder, ref bottomFarLand_F3_Tiles, ref lastOrderOfBottomFarLand_F3, new Vector3(1.6f, 0.0f, 0.0f));

        // Floor 4
        InitializePlatform(land3Prefab, ref lastPosOfBottomFarLand_F4, land3Prefab.transform.position, startLandTile - 7,
            bottomFarSideWalkHolder, ref bottomFarLand_F4_Tiles, ref lastOrderOfBottomFarLand_F4, new Vector3(1.6f, 0.0f, 0.0f));

        // Floor 5
        InitializePlatform(land4Prefab, ref lastPosOfBottomFarLand_F5, land4Prefab.transform.position, startLandTile - 10,
            bottomFarSideWalkHolder, ref bottomFarLand_F5_Tiles, ref lastOrderOfBottomFarLand_F5, new Vector3(1.6f, 0.0f, 0.0f));
    }
}
