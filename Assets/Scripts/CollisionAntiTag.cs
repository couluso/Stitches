
using UnityEngine;
using System.Collections.Generic;

public class CollisionAntiTag : MonoBehaviour
{
    public string tagABloquer;

    [Tooltip("Distance autour du mur à laquelle on détecte les objets, avant qu'ils ne le touchent")]
    public float marge = 1f;

    private Collider mur;

    // Mémorise pour chaque collider s'il porte le tag à bloquer (évite de refaire la recherche)
    private Dictionary<Collider, bool> dejaVerifies = new Dictionary<Collider, bool>();
    private Collider[] proches = new Collider[64];

    private void Start()
    {
        mur = GetComponent<Collider>();
    }

    // Le joueur et le Stitch bougent avec un CharacterController, qui ne déclenche
    // jamais OnCollisionEnter : on cherche donc nous-mêmes les objets proches du mur
    // et on désactive la collision AVANT qu'ils ne le touchent.
    private void FixedUpdate()
    {
        Bounds zone = mur.bounds;
        zone.Expand(marge * 2f);

        int nb = Physics.OverlapBoxNonAlloc(zone.center, zone.extents, proches, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < nb; i++)
        {
            Collider autre = proches[i];
            if (autre == mur) continue;

            if (!dejaVerifies.TryGetValue(autre, out bool aBloquer))
            {
                aBloquer = PorteLeTag(autre.transform);
                dejaVerifies[autre] = aBloquer;
            }

            // Réappliqué à chaque fois : l'ignore est perdu si l'objet est désactivé puis réactivé
            if (!aBloquer)
            {
                Physics.IgnoreCollision(autre, mur, true);
            }
        }
    }

    // Vrai si l'objet ou un de ses parents a le tag, ou si le tag est sur un enfant
    // du corps qui bouge (ex : le tag "Stitch" est sur l'enfant "Force" du StitchCapsule)
    private bool PorteLeTag(Transform t)
    {
        for (Transform p = t; p != null; p = p.parent)
        {
            if (p.CompareTag(tagABloquer)) return true;

            if (p.GetComponent<CharacterController>() != null || p.GetComponent<Rigidbody>() != null)
            {
                foreach (Transform enfant in p.GetComponentsInChildren<Transform>(true))
                {
                    if (enfant.CompareTag(tagABloquer)) return true;
                }
            }
        }
        return false;
    }
}
