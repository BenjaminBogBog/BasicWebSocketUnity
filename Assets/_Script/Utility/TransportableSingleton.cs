using UnityEngine;

namespace BogBog.Utility
{
    public class TransportableSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        private void Awake()
        {
            if (Instance == null)
            {
                var thisType = FindObjectOfType<T>(true);

                Instance = thisType;
                DontDestroyOnLoad(thisType);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static T Instance
        {
            get => _instance;
            set => _instance = value;
        }
        public static bool HasInstance() => _instance != null; 

        private static T _instance;

    }
}