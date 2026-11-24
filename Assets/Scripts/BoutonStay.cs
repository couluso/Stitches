using UnityEngine;
using System.Collections.Generic;

public class BoutonStay : MonoBehaviour
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
    public ActionType StayAction = ActionType.DesactiverObjet;

    [Header("Option 1 : Désactiver un objet / porte")]
    public GameObject StayDesavObjet;
    
    [Header("Option 1 : Activer un objet / porte")]
    public GameObject StayActivObjet;

    [Header("Option 2 : Changer le Material d'un objet")]
    public Renderer StayObjetAChanger;
    public Material StayNouveauMaterial;

    // Colliders valides actuellement sur le bouton (le joueur en a plusieurs)
    private HashSet<Collider> dessus = new HashSet<Collider>();

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

    private void OnTriggerStay(Collider other)
    {
        if (PeutAppuyer(other))
        {
            dessus.Add(other);

            // Exécute la désactivation de la porte si l'option est choisie
            if ((StayAction == ActionType.DesactiverObjet || StayAction == ActionType.Multi) && StayDesavObjet != null)
            {
                StayDesavObjet.SetActive(false);
            }

            if ((StayAction == ActionType.ActiverObjet || StayAction == ActionType.Multi) && StayActivObjet != null)
            {
                StayActivObjet.SetActive(true);
            }

            // Exécute le changement de matériau si l'option est choisie
            if ((StayAction == ActionType.ChangerMaterial || StayAction == ActionType.Multi) && StayObjetAChanger != null && StayNouveauMaterial != null)
            {
                StayObjetAChanger.material = StayNouveauMaterial;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        dessus.Remove(other);
        dessus.RemoveWhere(c => c == null || !c.enabled || !c.gameObject.activeInHierarchy);

        // On ne relâche le bouton que quand plus rien n'est dessus
        if (PeutAppuyer(other) && dessus.Count == 0)
        {
            // Exécute la désactivation de la porte si l'option est choisie
            if ((StayAction == ActionType.DesactiverObjet || StayAction == ActionType.Multi) && StayDesavObjet != null)
            {
                StayDesavObjet.SetActive(true);
            }

            if ((StayAction == ActionType.ActiverObjet || StayAction == ActionType.Multi) && StayActivObjet != null)
            {
                StayActivObjet.SetActive(false);
            }

            // Exécute le changement de matériau si l'option est choisie
            if ((StayAction == ActionType.ChangerMaterial || StayAction == ActionType.Multi) && StayObjetAChanger != null && StayNouveauMaterial != null)
            {
                StayObjetAChanger.material = StayNouveauMaterial;
            }
        }
    }
}
