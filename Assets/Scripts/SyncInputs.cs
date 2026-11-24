using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class SyncInputs : MonoBehaviour
{
    [Header("Références Joueurs")]
    public FirstPersonController controllerJ1;
    private FirstPersonController controllerJ2;

    public StarterAssetsInputs player1Inputs;
    private StarterAssetsInputs player2Inputs;

    [Header("Caméras (Cinemachine Virtual Camera)")]
    public GameObject cameraJ1;
    public GameObject cameraJ2;

    [Header("Paramètres")]
    public Key switchKey = Key.Tab;
    private bool isControllingJ1 = true;

    void Start()
    {
        controllerJ2 = GetComponent<FirstPersonController>();
        player2Inputs = GetComponent<StarterAssetsInputs>();

        // La vue reste toujours sur la caméra du J1
        if (cameraJ1 != null) cameraJ1.SetActive(true);
        if (cameraJ2 != null) cameraJ2.SetActive(false);

        UpdateActiveController();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[switchKey].wasPressedThisFrame)
        {
            isControllingJ1 = !isControllingJ1;
            UpdateActiveController();
        }

        // Quand on contrôle le J2, on copie les entrées captées par le J1
        if (!isControllingJ1 && player1Inputs != null && player2Inputs != null)
        {
            player2Inputs.move = player1Inputs.move;
            player2Inputs.look = player1Inputs.look;
            player2Inputs.jump = player1Inputs.jump;
            player2Inputs.sprint = player1Inputs.sprint;
        }
    }

    void UpdateActiveController()
    {
        // Active uniquement le script de mouvement du personnage contrôlé
        if (controllerJ1 != null) controllerJ1.enabled = isControllingJ1;
        if (controllerJ2 != null) controllerJ2.enabled = !isControllingJ1;

        // Remet les entrées du J2 à zéro quand on repasse sur le J1
        if (isControllingJ1 && player2Inputs != null)
        {
            player2Inputs.move = Vector2.zero;
            player2Inputs.look = Vector2.zero;
            player2Inputs.jump = false;
            player2Inputs.sprint = false;
        }
    }
}
