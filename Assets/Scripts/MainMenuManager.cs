using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    private string currentCat = "thief";
    public int currentCatId = 0;

    public GameObject thiefPrefab;
    public GameObject robberPrefab;
    public GameObject jasonPrefab;

    public GameObject left;
    public GameObject right;
    public GameObject title;
    public GameObject preview;
    public GameObject confirmButton;
    public GameObject chooseBackground;

    private GameObject currentCatPrefab;

    public bool selectCharacter = false;

    void Start()
    {
        currentCatId = 0;
        selectCharacter = false;
        currentCat = "thief";
        currentCatPrefab = thiefPrefab;
    }

    public void SetCat(int value)
    {
        if (value == 0 && currentCat == "thief")
        {
            currentCat = "jason";
            currentCatId = 0;
        }
        else if (value == 1 && currentCat == "thief")
        {
            currentCat = "robber";
            currentCatId = 1;
        }
        else if (value == 0 && currentCat == "robber")
        {
            currentCat = "thief";
            currentCatId = 0;
        }
        else if (value == 1 && currentCat == "robber")
        {
            currentCat = "jason";
            currentCatId = 1;
        }
        else if (value == 0 && currentCat == "jason")
        {
            currentCat = "robber";
            currentCatId = 0;
        }
        else if (value == 1 && currentCat == "jason")
        {
            currentCat = "thief";
            currentCatId = 1;
        }

        SpawnObject();
    }

    public void openSelectScreen()
    {
        left.SetActive(true);
        right.SetActive(true);
        title.SetActive(true);
        preview.SetActive(true);
        confirmButton.SetActive(true);
        chooseBackground.SetActive(true);
    }

    public void SpawnObject()
    {
        currentCatPrefab = (currentCat == "robber") ? robberPrefab : (currentCat == "thief" ? thiefPrefab : jasonPrefab);
        Vector3 position = currentCatId == 0 ? new Vector3(5f, 0.4f, -8.388f) : new Vector3(-5f, 0.4f, -8.388f);
        Vector3 rotationAngles = new Vector3(0f, 157f, 0f);
        Quaternion spawnRotation = Quaternion.Euler(rotationAngles);
        Instantiate(currentCatPrefab, position, spawnRotation);
    }

    public string GetCatName()
    {
        return currentCat;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
