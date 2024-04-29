using System;
using UnityEngine;

namespace BogBog.Utility
{
    public class LerpTransformTo : MonoBehaviour
    {
        [SerializeField] private Transform lerpTo;
        [SerializeField] private float lerpPositionSpeed;

        public bool lerpY;

        [SerializeField] private bool lerpRotation;
        [SerializeField] private float lerpRotationSpeed;
        private void Update()
        {
            var thisTransform = transform;
            var position = thisTransform.position;
            var rotation = thisTransform.rotation;

            transform.position = Vector3.Lerp(position, lerpY ? lerpTo.position : new Vector3(lerpTo.position.x, position.y, lerpTo.position.z), lerpPositionSpeed * Time.deltaTime);
            
            if(lerpRotation)
                transform.rotation = Quaternion.Slerp(rotation, lerpTo.rotation, lerpRotationSpeed * Time.deltaTime);
        }
    }
}