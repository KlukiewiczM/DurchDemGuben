using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
 {
        [SerializeField] private Image[] hearts; // 3 obrazki

        // na razie tylko test:
        private void Start()
        {
            // upewniamy się że są widoczne
            SetHearts(3);
        }

        public void SetHearts(int count)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] != null)
                    hearts[i].enabled = i < count;
            }
        }
}

