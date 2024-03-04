using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZV
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxFocusPoints(float maxFocusPoints)
        {
            slider.maxValue = maxFocusPoints;
            slider.value = maxFocusPoints;
        }

        public void SetCurrentFocusPoins(float currentFocusPoints)
        {
            slider.value = currentFocusPoints;
        }
    }
}
