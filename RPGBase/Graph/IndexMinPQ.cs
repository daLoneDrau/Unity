using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.RPGBase.Graph
{
    public class IndexMinPQ<Key> : IEnumerable<int>, IEnumerable where Key : IComparable<Key>
    {
        private int NMAX;        // maximum number of elements on PQ
        private int N;           // number of elements on PQ
        private int[] pq;        // binary heap using 1-based indexing
        private int[] qp;        // inverse of pq - qp[pq[i]] = pq[qp[i]] = i
        private Key[] keys;      // keys[i] = priority of i
        public IndexMinPQ(int NMAX)
        {
            if (NMAX < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.NMAX = NMAX;
            keys = new Key[NMAX + 1];    // make this of length NMAX??
            pq = new int[NMAX + 1];
            qp = new int[NMAX + 1];                   // make this of length NMAX??
            for (int i = 0; i <= NMAX; i++)
            {
                qp[i] = -1;
            }
        }
        public bool IsEmpty { get { return N == 0; } }
        public bool Contains(int i)
        {
            if (i < 0 || i >= NMAX) throw new IndexOutOfRangeException();
            return qp[i] != -1;
        }
        public int Count { get { return N; } }
        public void Insert(int i, Key key)
        {
            if (i < 0 || i >= NMAX)
            {
                throw new IndexOutOfRangeException();
            }
            if (Contains(i))
            {
                throw new ArgumentException("index is already in the priority queue");
            }
            N++;
            qp[i] = N;
            pq[N] = i;
            keys[i] = key;
            Swim(N);
        }
        public int MinIndex
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("Priority queue underflow");
                }
                return pq[1];
            }
        }
        public Key MinKey
        {
            get
            {
                if (N == 0)
                {
                    throw new InvalidOperationException("Priority queue underflow");
                }
                return keys[pq[1]];
            }
        }
        public int DelMin()
        {
            if (N == 0)
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            int min = pq[1];
            Exch(1, N--);
            Sink(1);
            qp[min] = -1;            // delete
            keys[pq[N + 1]] = default(Key);    // to help with garbage collection
            pq[N + 1] = -1;            // not needed
            return min;
        }
        public Key KeyOf(int i)
        {
            if (i < 0 || i >= NMAX)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            else return keys[i];
        }
        public void Change(int i, Key key)
        {
            ChangeKey(i, key);
        }
        public void ChangeKey(int i, Key key)
        {
            if (i < 0 || i >= NMAX) throw new IndexOutOfRangeException();
            if (!Contains(i)) throw new InvalidOperationException("index is not in the priority queue");
            keys[i] = key;
            Swim(qp[i]);
            Sink(qp[i]);
        }
        public void DecreaseKey(int i, Key key)
        {
            if (i < 0 || i >= NMAX)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            if (keys[i].CompareTo(key) <= 0)
            {
                throw new ArgumentException("Calling DecreaseKey() with given argument would not strictly decrease the key");
            }
            keys[i] = key;
            Swim(qp[i]);
        }
        public void IncreaseKey(int i, Key key)
        {
            if (i < 0 || i >= NMAX)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            if (keys[i].CompareTo(key) >= 0)
            {
                throw new ArgumentException("Calling increaseKey() with given argument would not strictly increase the key");
            }
            keys[i] = key;
            Sink(qp[i]);
        }
        public void Delete(int i)
        {
            if (i < 0 || i >= NMAX)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            int index = qp[i];
            Exch(index, N--);
            Swim(index);
            Sink(index);
            keys[i] = default(Key);
            qp[i] = -1;
        }
        /**************************************************************
    * General helper functions
    **************************************************************/
        private bool Greater(int i, int j)
        {
            return keys[pq[i]].CompareTo(keys[pq[j]]) > 0;
        }
        private void Exch(int i, int j)
        {
            int swap = pq[i]; pq[i] = pq[j]; pq[j] = swap;
            qp[pq[i]] = i; qp[pq[j]] = j;
        }
        /**************************************************************
         * Heap helper functions
         **************************************************************/
        private void Swim(int k)
        {
            while (k > 1 && Greater(k / 2, k))
            {
                Exch(k, k / 2);
                k = k / 2;
            }
        }
        private void Sink(int k)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && Greater(j, j + 1)) j++;
                if (!Greater(k, j)) break;
                Exch(k, j);
                k = j;
            }
        }
        public IEnumerator<int> GetEnumerator()
        {
            return new HeapIEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        private class HeapIEnumerator : IEnumerator<int>
        {
            // create a new pq
            private IndexMinPQ<Key> copy;
            private IndexMinPQ<Key> innerPQ;

            // add all items to copy of heap
            // takes linear time since already in heap order so no keys move
            public HeapIEnumerator(IndexMinPQ<Key> minpq)
            {
                innerPQ = new IndexMinPQ<Key>(minpq.Count);
                for (int i = 1; i <= minpq.N; i++)
                    innerPQ.Insert(minpq.pq[i], minpq.keys[minpq.pq[i]]);
                copy = innerPQ;
            }
            private void Init() { }
            public int Current
            {
                get
                {
                    if (innerPQ.IsEmpty) throw new InvalidOperationException("Priority queue underflow");
                    return innerPQ.DelMin();
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return Current as object;
                }
            }
            public bool MoveNext()
            {
                if (innerPQ.IsEmpty) return false;
                else return true;
            }
            public void Reset()
            {
                innerPQ = copy;
            }
            public void Dispose() { }
        }
    }
}
