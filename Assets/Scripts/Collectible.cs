using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotationSpeed; 
    public GameObject onCollectEffect;
    public AudioClip coinSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position);
            }

            // Add points to the player's score
            //ScoreManager.Instance.AddPoints(10); 
            // Destroy the collectible
            Destroy(gameObject);

            Instantiate(onCollectEffect, transform.position, transform.rotation);   
        }
        
        
    }
}
