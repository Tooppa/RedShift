using UnityEngine;

/// <summary>
/// This is a one off script to make the Mörkö "climb" up from the lair. The Mörkö is teleporter to the edge of the cliff
/// when the player arrives at the designated trigger which fires <see cref="TryClimbUp"/>>.
/// </summary>
public class MorkoChaseClimbUp : MonoBehaviour
{
    public GameObject morko;

    private readonly Vector3 _positionMorkoMovesTo = new Vector3(381, -31,0); // Approximately the edge of the cliff in world coordinates

    private void Start()
    {
        if (morko == null)
        {
            Debug.LogWarning("{morko.gameObject.name} not active!");
        }
    }

    public void TryClimbUp()
    {
        // Mörkö has had been activated. Otherwise it has not been woken up yet and is not in a chase
        if (!morko.activeSelf) return;
        
        morko.transform.position = _positionMorkoMovesTo;
        
        gameObject.SetActive(false); // Disable additional triggers
    }
}
