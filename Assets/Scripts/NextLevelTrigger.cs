using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "PlayerTag";

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "2LevelScene";

    [Header("Optional Condition")]
    [SerializeField] private GameObject buildingPlaceholder;
    [SerializeField] private bool requireBuilding = false;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        if (requireBuilding)
        {
            if (buildingPlaceholder == null || !buildingPlaceholder.activeSelf)
            {
                Debug.Log("Najpierw musisz zbudować budynek!");
                return;
            }
        }

        triggered = true;

        FreezePlayer(other);

        FadeManager fadeManager = FindFirstObjectByType<FadeManager>();

        if (fadeManager != null)
            StartCoroutine(fadeManager.FadeOut(nextSceneName));
        else
            SceneManager.LoadScene(nextSceneName);
    }

    private void FreezePlayer(Collider2D other)
    {
        PlayerMovement2D movement = other.GetComponentInParent<PlayerMovement2D>();
        Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();
        Animator animator = other.GetComponentInParent<Animator>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsGrounded", true);
            animator.ResetTrigger("JumpTrigger");
        }

        if (movement != null)
            movement.enabled = false;
    }
}