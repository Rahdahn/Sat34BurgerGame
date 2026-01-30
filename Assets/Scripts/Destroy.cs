using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}