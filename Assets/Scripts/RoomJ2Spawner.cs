using UnityEngine;

public class RoomJ2Spawner : MonoBehaviour
{
    [Header("Configuration du Spawn J2")]
    [Tooltip("Glissez le Prefab du Joueur 2 (avec sa caméra RenderTexture)")]
    public GameObject player2Prefab;
    [Tooltip("Un objet vide placé là où le J2 doit apparaître dans cette salle")]
    public Transform spawnPoint;

    [Header("Option Caméra")]
    [Tooltip("Cochez si cette salle possède un écran permettant de tourner la caméra du J2")]
    public bool allowCameraLookInThisRoom = true;

    // Variable partagée entre toutes les salles pour garder la trace du J2 actuel
    private static FirstPersonSwitchController currentJ2Instance;
    private bool hasSpawnedForThisEntry = false;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est bien le Joueur 1 qui entre (via son tag "Player")
        if (other.CompareTag("Player") && !hasSpawnedForThisEntry)
        {
            hasSpawnedForThisEntry = true;
            SpawnNewJ2();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Tant que le J1 est dans cette zone, on applique le réglage caméra de cette salle
        if (other.CompareTag("Player") && currentJ2Instance != null)
        {
            currentJ2Instance.canLookJ2 = allowCameraLookInThisRoom;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Dès que le J1 sort (même en reculant), on verrouille la caméra et on réinitialise le spawn
        if (other.CompareTag("Player"))
        {
            hasSpawnedForThisEntry = false;
            if (currentJ2Instance != null)
            {
                currentJ2Instance.canLookJ2 = false;
            }
        }
    }

    private void SpawnNewJ2()
    {
        // 1. Détruit le parent complet (StitchParent) de tous les J2 existants
        FirstPersonSwitchController[] existingJ2s = FindObjectsByType<FirstPersonSwitchController>();
        foreach (FirstPersonSwitchController j2 in existingJ2s)
        {
            Destroy(j2.transform.root.gameObject);
        }

        // 2. Fait apparaître le nouveau prefab (StitchParent + J2) au point exact de la salle
        if (player2Prefab != null && spawnPoint != null)
        {
            GameObject newJ2Root = Instantiate(player2Prefab, spawnPoint.position, spawnPoint.rotation);

            // Cherche le script sur le parent OU sur ses enfants (PlayerCapsule)
            currentJ2Instance = newJ2Root.GetComponentInChildren<FirstPersonSwitchController>();
            if (currentJ2Instance != null)
            {
                currentJ2Instance.canLookJ2 = allowCameraLookInThisRoom;
            }
        }
    }

}
