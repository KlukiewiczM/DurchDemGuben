using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
 {
        [SerializeField] private Image[] hearts;
       
        private void Start()
        {            
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

