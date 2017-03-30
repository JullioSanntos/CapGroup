using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Client.Infrastructure
{
    public class CircularList<T> 
    {
        public int MaximumCapacity { get; private set; }

        public readonly T[] CircularArray;

        private int _nextCell = 0;
        public CircularList(int capacity) 
        {
            if (capacity <=0 ) throw new ArgumentException(@"Invalid capacity", nameof(capacity));
            MaximumCapacity = capacity;
            CircularArray = new T[MaximumCapacity];
        }

        public int Count;

        public void Add(T item)
        {
            CircularArray[_nextCell] = item;
            _nextCell = Next(_nextCell);
            Count = Math.Min(Count + 1, MaximumCapacity);
        }

        public void Remove(T item)
        {
            var foundIndex = Find(item);
            if (foundIndex == -1) throw new ArgumentException(@"Item not found", nameof(item));
            CircularArray[foundIndex] = default(T);
            if (CircularArray[foundIndex] == null) Console.WriteLine(@"Remove: is null: ");
            _nextCell = Previous(_nextCell);
            if (_nextCell == foundIndex) return; //no need to re-arrange cells. Removed value was queue's last one
            var currIndex = Next(foundIndex);
            while (currIndex != foundIndex)
            {
                CopyToPrevious(currIndex);
                currIndex = Next(currIndex);
            }
            Count = Math.Min(Count - 1, 0);
        }

        private void CopyToPrevious(int currentIndex)
        {
            var previousIndex = Previous(currentIndex);
            CircularArray[previousIndex] = CircularArray[currentIndex];
        }

        public int Find(T item)
        {
            var index = _nextCell;
            for (int i = 0; i < MaximumCapacity; i++)
            {
                if (item.Equals(CircularArray[index])) break;
                index = Previous(index);
            }
            if (!item.Equals(CircularArray[index])) return -1; //not found
            return index;
        }

        private int Next(int index)
        {
            var nextIndex = index + 1;
            if (nextIndex >= MaximumCapacity) nextIndex = 0;
            return nextIndex;
        }

        private int Previous(int index)
        {
            var prevIndex = index - 1;
            if (prevIndex == -1) prevIndex = MaximumCapacity - 1;
            return prevIndex;
        }

        public T Peek(int backwardIndex = 0)
        {
            if (backwardIndex == 0) return CircularArray[Previous(_nextCell)];
            var index = _nextCell - 1;
            for (int i = 0; i < Math.Abs(backwardIndex); i++)
            {
                index = Previous(index);
            }
            return CircularArray[index];
        }
    }
}
