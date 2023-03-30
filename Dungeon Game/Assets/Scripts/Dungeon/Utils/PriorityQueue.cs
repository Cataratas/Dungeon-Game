using System;
using System.Collections.Generic;

namespace Dungeon.Utils {
    public class PriorityQueue<T> {
        private readonly List<Tuple<int, T>> items;

        public PriorityQueue() {
            items = new List<Tuple<int, T>>();
        }

        public int Count => items.Count;

        public void Enqueue(T item, int priority) {
            items.Add(Tuple.Create(priority, item));
            int i = items.Count - 1;
            while (i > 0) {
                int parent = (i - 1) / 2;
                if (items[parent].Item1 <= items[i].Item1) {
                    break;
                }
                Swap(i, parent);
                i = parent;
            }
        }

        public T Dequeue() {
            if (items.Count == 0) {
                throw new InvalidOperationException("Queue is empty.");
            }

            var item = items[0].Item2;
            items[0] = items[^1];
            items.RemoveAt(items.Count - 1);

            int i = 0;
            while (i < items.Count) {
                int leftChild = 2 * i + 1;
                int rightChild = 2 * i + 2;
                int minChild = i;

                if (leftChild < items.Count && items[leftChild].Item1 < items[minChild].Item1) {
                    minChild = leftChild;
                }

                if (rightChild < items.Count && items[rightChild].Item1 < items[minChild].Item1) {
                    minChild = rightChild;
                }

                if (minChild == i) {
                    break;
                }

                Swap(i, minChild);
                i = minChild;
            }

            return item;
        }

        private void Swap(int i, int j) {
            (items[i], items[j]) = (items[j], items[i]);
        }
    }
}
