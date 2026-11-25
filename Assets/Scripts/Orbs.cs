
using UnityEngine;
using System.Collections.Generic;

public class OrbeCollect : MonoBehaviour
{
    public enum ActionType
    {
        DesactiverPorte,
        ChangerMaterial,
        LesDeux
    }

    [Header("Identification du groupe")]
    public string idGroupe;

    [Header("Choix de l'action")]
    public ActionType action = ActionType.DesactiverPorte;

    [Header("Option 1 : Désactiver un objet / porte")]
    public GameObject porte;

    [Header("Option 2 : Changer le Material d'un objet")]
    public Renderer objetAChanger;
    public Material nouveauMaterial;

    [Header("Nombre d'orbes nécessaires")]
    public int Nb_Orbes = 5;

    // Compteurs partagés par groupe
    private static Dictionary<string, int> compteurs = new Dictionary<string, int>();

    private bool dejaRamasse = false;


    private void Start()
    {
        compteurs.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dejaRamasse) return;

        if (other.CompareTag("Player") || other.CompareTag("Stitch"))
        {
            dejaRamasse = true;

            // Initialise le compteur si nécessaire
            if (!compteurs.ContainsKey(idGroupe))
            {
                compteurs[idGroupe] = 0;
            }

            // Incrémente le compteur du groupe
            compteurs[idGroupe]++;

            // Vérifie si toutes les orbes sont récupérées
            if (compteurs[idGroupe] >= Nb_Orbes)
            {
                if (action == ActionType.DesactiverPorte || action == ActionType.LesDeux)
                {
                    if (porte != null)
                        porte.SetActive(false);
                }

                if (action == ActionType.ChangerMaterial || action == ActionType.LesDeux)
                {
                    if (objetAChanger != null && nouveauMaterial != null)
                        objetAChanger.material = nouveauMaterial;
                }
            }

            gameObject.SetActive(false);
        }
    }

    // Permet de consulter le compteur depuis un autre script
    public static int GetCompteur(string id)
    {
        return compteurs.ContainsKey(id) ? compteurs[id] : 0;
    }
}
