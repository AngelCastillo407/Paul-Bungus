using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int itemsCollected = 0;
    public int healthPoints = 3;

    public bool canEscape = false;
    public bool didEscape = false;

    public GameObject redConfetti;
    public GameObject blueConfetti;
    public GameObject greenConfetti;
    public GameObject congratulationsMessage;
    public string catName = "";

    public GameObject mask1;
    public GameObject weapon1;

    public GameObject mask2;
    public GameObject weapon2;

    public GameObject mask3;
    public GameObject weapon3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    void Start()
    {
        catName = MainMenuManager.Instance ? MainMenuManager.Instance.GetCatName() : "jason";
        chooseCostume();
    }

    public void chooseCostume()
    {
        if (catName == "thief")
        {
            mask1.SetActive(true);
            weapon1.SetActive(true);
        }
        else if (catName == "robber") 
        {
            mask2.SetActive(true);
            weapon2.SetActive(true);
        }
        else
        {
            mask3.SetActive(true);
            weapon3.SetActive(true);
        }
    }

    public void summonConfetti()
    {
        blueConfetti.SetActive(true);
        greenConfetti.SetActive(true);
        redConfetti.SetActive(true);
        congratulationsMessage.SetActive(true);
        StartCoroutine(LoadLastScene());
    }

    IEnumerator LoadLastScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void collectItem()
    {
        itemsCollected += 1;
        if (itemsCollected == 5)
        {
            canEscape = true;
        }
    }

    public void takeDamage()
    {
        healthPoints -= 1;
        if (healthPoints == 0)
        {
            // do stuff here
        }
    }
}
