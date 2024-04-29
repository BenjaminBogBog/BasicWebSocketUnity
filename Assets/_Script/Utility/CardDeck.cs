using System.Collections;
using System.Collections.Generic;
using BogBog.Utility;
using UnityEngine;

namespace BogBog.Utilities
{
    public class CardDeck<T> : Queue
    {
        private readonly Queue<T> _stack;

        public delegate void DeckChange(Queue<T> stack);

        public event DeckChange OnDeckChange;
        public CardDeck(List<T> list)
        {
            list.Shuffle();
            _stack = new Queue<T>(list);
        }

        public override object Peek()
        {
            T newStack = _stack.Peek();
            OnDeckChange?.Invoke(_stack);
            return newStack;
        }
    
        public object Draw()
        {
            if (_stack.Count <= 0)
            {
                Debug.LogWarning("Empty Deck, No Cards to draw");
                return null;
            }
        
            T newStack = _stack.Dequeue();
            OnDeckChange?.Invoke(_stack);
            return newStack;
        }

        public void Insert(object obj)
        {
            _stack.Enqueue((T)obj);
            OnDeckChange?.Invoke(_stack);
        }
    }
}