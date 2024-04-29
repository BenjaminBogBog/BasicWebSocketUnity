
    using System;
    using System.Collections.Generic;

    namespace BogBog.Utility
    {
        public static class Utility
        {
            private static readonly Random rng = new Random();  

            public static void Shuffle<T>(this IList<T> list)  
            {  
                int n = list.Count;  
                while (n > 1) {  
                    n--;  
                    int k = rng.Next(n + 1);  
                    (list[k], list[n]) = (list[n], list[k]);
                }  
            }
            
            public static float Remap (this float value, float from1, float to1, float from2, float to2) {
                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }
        }
    }
    