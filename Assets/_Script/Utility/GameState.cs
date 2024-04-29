using System;
using UnityEngine;

namespace BogBog.Utilities
{
    public abstract class GameState : MonoBehaviour
    {
        [SerializeField] protected GameObject[] objectsToActivate;
        protected AudioSource StateAudio;

        private void Awake()
        {
            StateAudio = gameObject.AddComponent<AudioSource>();
        }

        public virtual void StateEnter()
        {
            if (objectsToActivate == null) return;
            foreach (var obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }

        public virtual void StateUpdate()
        {
        }

        public virtual void StateExit()
        {
            if (objectsToActivate == null) return;
            foreach (var obj in objectsToActivate)
            {
                obj.SetActive(false);
            }
        }
    }
}