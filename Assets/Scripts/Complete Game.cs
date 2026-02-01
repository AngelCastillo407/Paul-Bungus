using UnityEngine;

public class CompleteGame : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Jason Cat")
        {
            GameManager.Instance.summonConfetti();
            Destroy(gameObject);
        }
    }
}
