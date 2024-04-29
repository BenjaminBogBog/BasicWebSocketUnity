using UnityEngine;

namespace BogBog.Utilities
{
    public class BillboardUI : MonoBehaviour
    {
        void Update()
        {
            if (Camera.main != null) transform.LookAt(Camera.main.transform);
        }
    }
}
