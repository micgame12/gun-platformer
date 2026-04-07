using UnityEngine;
using PlayerSystem;
using PlayerSystem.Controller;

namespace PlayerSystem.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        [Header("References")]
        public PlayerController playerController;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            Instance = this;
        }
    }
}


