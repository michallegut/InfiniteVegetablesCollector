using System.Collections.Generic;
using TerrainGenerator;
using UnityEngine;
using UnityEngine.Rendering;

public class RootVegetablesController : MonoBehaviour
{
    public static RootVegetablesController instance;

    public GameObject beetPrefab;
    public GameObject daikonPrefab;
    public GameObject parsnipPrefab;
    public GameObject radishPrefab;
    public GameObject radishAltPrefab;

    private Dictionary<Vector2i, GameObject> loadaedVegetables;
    private int collectedVegetables;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loadaedVegetables = new Dictionary<Vector2i, GameObject>();
        collectedVegetables = 0;
    }

    public void loadVegetable(Vector2i coordinates, Vector3 startingPosition)
    {
        if (loadaedVegetables.ContainsKey(coordinates))
        {
            if (!isVegetableCollected(coordinates))
            {
                loadaedVegetables[coordinates].SetActive(true);
            }
        }
        else
        {
            GameObject vegetable = Instantiate(randomizeVegetable(), randomizePosition(startingPosition), Quaternion.identity);
            vegetable.AddComponent<LineRenderer>();
            drawVegetableBeacon(vegetable, vegetable.GetComponent<LineRenderer>());
            loadaedVegetables.Add(coordinates, vegetable);
        }
    }

    private bool isVegetableCollected(Vector2i coordinates)
    {
        return loadaedVegetables[coordinates] == null;
    }

    private GameObject randomizeVegetable()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                return beetPrefab;
            case 1:
                return daikonPrefab;
            case 2:
                return parsnipPrefab;
            case 3:
                return radishPrefab;
            default:
                return radishAltPrefab;
        }
    }

    private Vector3 randomizePosition(Vector3 startingPosition)
    {
        float x = startingPosition.x;
        float z = startingPosition.z;
        float y = Terrain.activeTerrain.SampleHeight(startingPosition) - 0.1f;
        return new Vector3(x, y, z);
    }

    private void drawVegetableBeacon(GameObject vegetable, LineRenderer vegetableBeacon)
    {
        vegetableBeacon.startWidth = 0.1f;
        vegetableBeacon.endWidth = 0.1f;
        vegetableBeacon.positionCount = 2;
        vegetableBeacon.SetPosition(0, vegetable.transform.position);
        vegetableBeacon.SetPosition(1, new Vector3(vegetable.transform.position.x, vegetable.transform.position.y + 40, vegetable.transform.position.z));
        vegetableBeacon.material.color = Color.red;
        vegetableBeacon.shadowCastingMode = ShadowCastingMode.Off;
    }

    public void hideVegetable(Vector2i coordinates)
    {
        if (loadaedVegetables[coordinates] != null)
        {
            loadaedVegetables[coordinates].SetActive(false);
        }
    }

    public void collectVegetable(GameObject vegetable)
    {
        Destroy(vegetable);
        collectedVegetables++;
        UIController.instance.updateCollectedVegetablesText(collectedVegetables);
    }
}