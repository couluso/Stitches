using UnityEngine;

public class Interrupteur : MonoBehaviour
{
    // Liste des actions disponibles dans le menu déroulant de l'Inspecteur
    public enum ActionType
    {
        DesactiverPorte,
        ChangerMaterial,
        LesDeux
    }

    [Header("Choix de l'action")]
    public ActionType action = ActionType.DesactiverPorte;

    [Header("Option 1 : Désactiver un objet / porte")]
    public GameObject porte;

    [Header("Option 2 : Changer le Material d'un objet")]
    public Renderer objetAChanger;
    public Material nouveauMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Caisse") || other.CompareTag("Stitch"))
        {
            // Exécute la désactivation de la porte si l'option est choisie
            if ((action == ActionType.DesactiverPorte || action == ActionType.LesDeux) && porte != null)
            {
                porte.SetActive(false);
            }

            // Exécute le changement de matériau si l'option est choisie
            if ((action == ActionType.ChangerMaterial || action == ActionType.LesDeux) && objetAChanger != null && nouveauMaterial != null)
            {
                objetAChanger.material = nouveauMaterial;
            }
        }
    }
}
