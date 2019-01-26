using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class GameUI : MonoBehaviour
    {
        public TextMeshProUGUI Roles;

        public Slider Health;

        public void SetHealth (float value)
        {
            Health.value = value;
        }
    }
}
