using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private void Start()
    {
        var selected = CharacterSelectController.GetSavedCharacter();
        Debug.Log("Selected character: " + selected);

        // później: spawn odpowiedniego prefaba (male/female)
    }
}
