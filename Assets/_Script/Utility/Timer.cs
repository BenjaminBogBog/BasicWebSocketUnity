using System;
using UnityEngine;

namespace BogBog.Utility
{
    [Serializable]
    public class Timer
    {
        public Timer(float time)
        {
            TimeLeft = time;
            _startingTime = time;
            _running = false;
        }
        
        [field: SerializeField] public float TimeLeft { get; set; }
    
        private float _startingTime;
        private bool _running;
    
        public Action OnTimerCounted;
    
        public void Start()
        {
            _running = true;
        }
    
        public void Stop()
        {
            _running = false;
        }
    
        public void Reset()
        {
            _running = false;
            TimeLeft = _startingTime;
        }
        
        public float GetTimerProgress()
        {
            return 1 - (TimeLeft / _startingTime);
        }
        
        /// <summary>
        /// Run this method in the Update method to make it work. Make sure Start() has been called.
        /// </summary>
        public void RunTimer()
        {
            if (!_running) return;
            
            TimeLeft -= Time.deltaTime;
    
            if (TimeLeft <= 0)
            {
                TimeLeft = 0;
                OnTimerCounted?.Invoke();
                _running = false;
            }
        }
    }
}
