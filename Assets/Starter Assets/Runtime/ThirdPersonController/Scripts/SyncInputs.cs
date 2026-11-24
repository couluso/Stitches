using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class FirstPersonSwitchController : MonoBehaviour
{
    [Header("Références Joueur 1")]
    public StarterAssetsInputs player1Inputs;
    private FirstPersonController controllerJ1;

    private StarterAssetsInputs player2Inputs;
    private FirstPersonController controllerJ2;

    [Header("Contrôles & Zones")]
    public Key switchKey = Key.Tab;
    public bool canLookJ2 = false;

    private bool isControllingJ1 = true;

    void Start()
    {
        player2Inputs = GetComponent<StarterAssetsInputs>();
        controllerJ2 = GetComponent<FirstPersonController>();

        // Recherche automatiquement le Joueur 1 dans la scène s'il n'est pas assigné
        if (player1Inputs == null)
        {
            PlayerInput p1 = FindAnyObjectByType<PlayerInput>();
            if (p1 != null) player1Inputs = p1.GetComponent<StarterAssetsInputs>();
        }

        if (player1Inputs != null)
        {
            controllerJ1 = player1Inputs.GetComponent<FirstPersonController>();
        }

        ApplyControlState();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[switchKey].wasPressedThisFrame)
        {
            isControllingJ1 = !isControllingJ1;
            ApplyControlState();
        }

        if (!isControllingJ1 && player1Inputs != null && player2Inputs != null)
        {
            player2Inputs.move = player1Inputs.move;
            player2Inputs.jump = player1Inputs.jump;
            player2Inputs.sprint = player1Inputs.sprint;
            player2Inputs.look = canLookJ2 ? player1Inputs.look : Vector2.zero;

            player1Inputs.jump = false;
        }
    }

    public void ForceControlJ1()
    {
        isControllingJ1 = true;
        ApplyControlState();
    }

    private void ApplyControlState()
    {
        if (controllerJ1 != null) controllerJ1.enabled = isControllingJ1;
        if (controllerJ2 != null) controllerJ2.enabled = !isControllingJ1;

        if (isControllingJ1 && player2Inputs != null)
        {
            player2Inputs.move = Vector2.zero;
            player2Inputs.look = Vector2.zero;
            player2Inputs.jump = false;
            player2Inputs.sprint = false;
        }
    }

    private void OnDestroy()
    {
        // Sécurité : si ce J2 est détruit, on s'assure que le J1 récupère bien ses contrôles
        if (controllerJ1 != null) controllerJ1.enabled = true;
    }
}
