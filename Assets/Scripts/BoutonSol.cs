using UnityEngine;

public class Interrupteur : MonoBehaviour
{
    public GameObject porte;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Caisse") || other.CompareTag("Stitch"))
        {
            porte.SetActive(false);
        }
    }
}
