using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace ZV
{
    public class FPSCounter : MonoBehaviour
    {
        private TextMeshProUGUI fpsCounterText;
        [SerializeField] private GameObject fpsCounter;
        private float _timer;

        [SerializeField] private float _hudRefreshRate = 1f;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        // Start is called before the first frame update
        private void Start()
        {
            fpsCounter = GameObject.Find("FPSCounterText");
            fpsCounterText = fpsCounter.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            // Show FPS counter if it's not null
            if (fpsCounter != null && Time.unscaledTime > _timer)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                fpsCounterText.text = "FPS: " + fps.ToString();

                // Change text color based on FPS range
                if (fps < 30)
                {
                    fpsCounterText.color = Color.red;
                }
                else if (fps >= 30 && fps < 60)
                {
                    fpsCounterText.color = Color.yellow;
                }
                else
                {
                    fpsCounterText.color = Color.green;
                }

                _timer = Time.unscaledTime + _hudRefreshRate;
            }
        }
    }
}
