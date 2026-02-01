using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canEscape)
        {
            Destroy(gameObject);
        }
    }
}
