using UnityEngine;

public class Collect_Item : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Jason Cat")
        {
            GameManager.Instance.collectItem();
            Destroy(gameObject);
        }

    }
}
