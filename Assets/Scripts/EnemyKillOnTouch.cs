using UnityEngine;

public class EnemyKillOnTouch : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        // Najprościej: wołamy system śmierci z gracza
        //var death = other.GetComponent<PlayerDeath>(); // jeśli tak nazwałeś
        //if (death != null)
        //{
        //    death.Die();
        //    return;
        //}

        // Jeśli nie masz PlayerDeath, to chociaż log:
        Debug.Log("Player touched enemy -> DEAD (brak PlayerDeath na graczu).");
    }
}
