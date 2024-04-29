using UnityEngine;

namespace BogBog.Utility
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

        public static T Instance {
            get {
                if (_instance == null) {
                    var newInstance = FindObjectOfType<T>(true);
                    if (newInstance == null)
                        Debug.LogError($"Failed to get singleton instance for {typeof(T).Name}");
                    else
                        _instance = newInstance;
                }

                return _instance;
            }
        }

        public static bool HasInstance() => _instance != null; 

        private static T _instance;

    }
}