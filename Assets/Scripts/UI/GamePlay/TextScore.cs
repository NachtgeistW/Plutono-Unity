using Plutono.GamePlay;
using TMPro;
using UnityEngine;

namespace Plutono.UI
{
    public class TextScore : MonoBehaviour
    {
        public TMP_Text scoreText;

        // Start is called before the first frame update
        void Start()
        {
            scoreText.text = "0 + 0";
        }

        // Update is called once per frame
        void Update()
        {
            //scoreText.text = GamePlayController.Instance.Status.BasicScore.ToString() + " + " + GamePlayController.Instance.Status.ComboScore.ToString();
        }
    }
}