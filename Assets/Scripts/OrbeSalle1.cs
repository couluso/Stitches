using UnityEngine;

public class OrbeCollect : MonoBehaviour
{
    public GameObject porte;
    public static int OrbesCollected = 0; // Variable partagée entre toutes les orbes
    public int Nb_Orbes = 5;

    private bool dejaRamasse = false;

    void Start()
    {
        // Réinitialise le compteur au lancement de la scène
        OrbesCollected = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dejaRamasse) return;

        if (other.CompareTag("Player") || other.CompareTag("Caisse") || other.CompareTag("Stitch"))
        {
            dejaRamasse = true;
            OrbesCollected++;

            // Vérifie directement au ramassage (plus propre et plus rapide que Update)
            if (OrbesCollected >= Nb_Orbes && porte != null)
            {
                porte.SetActive(false);
            }

            gameObject.SetActive(false);
        }
    }
}
