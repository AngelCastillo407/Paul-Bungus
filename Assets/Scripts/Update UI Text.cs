using UnityEngine;
using TMPro;

public class UpdateUIText : MonoBehaviour
{
    public TMP_Text itemsCollectedCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemsCollectedCounter.text = GameManager.Instance.itemsCollected + " / 5";
    }

    // Update is called once per frame
    void Update()
    {
        itemsCollectedCounter.text = GameManager.Instance.itemsCollected + " / 5";
    }
}
