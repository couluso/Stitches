using UnityEngine;

public class BoutonToggle : MonoBehaviour
{
    // Liste des actions disponibles dans le menu déroulant de l'Inspecteur
    public enum ActionType
    {
        DesactiverObjet,
        ChangerMaterial,
        ActiverObjet,
        Multi
    }

    [Header("Choix de l'action")]
    public ActionType ToggleAction = ActionType.DesactiverObjet;

    [Header("Option 1 : Désactiver un objet / porte")]
    public GameObject ToggleDesavObjet;

    [Header("Option 1 : Activer un objet / porte")]
    public GameObject ToggleActivObjet;

    [Header("Option 2 : Changer le Material d'un objet")]
    public Renderer ToggleObjetAChanger;
    public Material ToggleNouveauMaterial;

    // Le tag peut être sur un parent : la capsule de détection du CharacterController
    // flotte ~0,2 m au-dessus du sol et ne touche pas les boutons fins, ce sont donc
    // les colliders enfants (ex : "Force") qui appuient dessus.
    private bool PeutAppuyer(Collider other)
    {
        for (Transform t = other.transform; t != null; t = t.parent)
        {
            if (t.CompareTag("Player") || t.CompareTag("Caisse") || t.CompareTag("Stitch")) return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PeutAppuyer(other))
        {
            // Exécute la désactivation de la porte si l'option est choisie
            if ((ToggleAction == ActionType.DesactiverObjet || ToggleAction == ActionType.Multi) && ToggleDesavObjet != null)
            {
                ToggleDesavObjet.SetActive(false);
            }

            if ((ToggleAction == ActionType.ActiverObjet || ToggleAction == ActionType.Multi) && ToggleActivObjet != null)
            {
                ToggleDesavObjet.SetActive(true);
            }

            // Exécute le changement de matériau si l'option est choisie
            if ((ToggleAction == ActionType.ChangerMaterial || ToggleAction == ActionType.Multi) && ToggleObjetAChanger != null && ToggleNouveauMaterial != null)
            {
                ToggleObjetAChanger.material = ToggleNouveauMaterial;
            }
        }
    }
}
