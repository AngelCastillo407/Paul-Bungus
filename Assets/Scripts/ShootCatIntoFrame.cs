using UnityEngine;
using System.Collections;

public class ShootCatIntoFrame : MonoBehaviour
{
    private Rigidbody rb;
    private string myCatName;
    private bool blastedOffAgain = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCatName = MainMenuManager.Instance.GetCatName();
        if (MainMenuManager.Instance.currentCatId == 0)
        {
            rb.AddForce(Vector3.left * 5f, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myCatName != MainMenuManager.Instance.GetCatName())
        {
            if (MainMenuManager.Instance.currentCatId == 0 && !blastedOffAgain)
            {
                rb.AddForce(Vector3.left * 5f, ForceMode.Impulse);
                StartCoroutine(DestroySelf());
                blastedOffAgain = true;
            }
            else if (MainMenuManager.Instance.currentCatId == 1 && !blastedOffAgain)
            {
                rb.AddForce(Vector3.right * 5f, ForceMode.Impulse);
                StartCoroutine(DestroySelf());
                blastedOffAgain = true;
            }
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
