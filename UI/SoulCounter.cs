using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ZV
{
    public class SoulCounter : MonoBehaviour
    {
        public TextMeshProUGUI soulCountText;

        public void SetSoulCountText(int soulCount)
        {
            soulCountText.text = soulCount.ToString();
        }
    }
}
