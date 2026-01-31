using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneResetter : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 2.5f, rb.linearVelocity.z);
        StartCoroutine(ResetAfterDelay(2f));
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
