using UnityEngine;

public class Treasure : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddTreasure();
            Destroy(gameObject);
        }
    }
}