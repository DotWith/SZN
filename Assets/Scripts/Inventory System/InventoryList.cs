using System;
using System.Collections.Generic;

namespace Com.Dot.SZN.InventorySystem
{
    class Node<T>
    {
        public Node<T> Next;
        private readonly T Val;
        public T Value { get => Val; }
        public Node(T newVal)
        {
            Val = newVal;
        }
    }

    public class InventoryList<T>
    {
        Node<T> Head;

        public InventoryList()
        {
        }

        public InventoryList(T val)
        {
            Head = new Node<T>(val);
        }

        public T GetValue(int pos)
        {
            Node<T> t = Head;
            try
            {
                for (int i = 0; i < pos; i++)
                    t = t.Next;
                return t.Value;
            }
            catch
            {

            }

            return default(T);
        }

        public int GetCount()
        {
            try
            {
                Node<T> t = Head;
                var newList = new List<T>();

                while (t != null)
                {
                    newList.Add(t.Value);
                    t = t.Next;
                }

                return newList.Count;
            }
            catch
            {

            }

            return 0;
        }

        public void AddValue(int pos, T val)
        {
            try
            {
                if (Head == null && pos != 0)
                    throw new Exception();
                else if (Head == null)
                    Head = new Node<T>(val);
                else
                {
                    Node<T> t = Head;

                    for (int i = 0; i < pos - 1; i++)
                        if (t.Next != null)
                            t = t.Next;

                    Node<T> newNode = new Node<T>(val);
                    if (pos == 0)
                    {
                        newNode.Next = t;
                        Head = newNode;
                    }
                    else if (t != null)
                    {
                        newNode.Next = t.Next;
                        t.Next = newNode;
                    }
                }
            }
            catch
            { }
        }

        public T DeleteValue(int pos)
        {
            try
            {
                Node<T> t = Head;
                if (pos == 0)
                {
                    Head = t.Next;
                    return t.Value;
                }
                else
                {
                    while (t != null && pos - 1 != 0)
                    {
                        t = t.Next;
                        pos--;
                    }
                    Node<T> del = t.Next;
                    t.Next = del.Next;
                    return del.Value;
                }
            }
            catch
            {

            }
            return default(T);
        }

        /*public List<T> ToList()
        {
            Node<T> t = Head;
            var newList = new List<T>();

            while (t != null)
            {
                newList.Add(t.Value);
                t = t.Next;
            }

            return newList;
        }*/
    }
}
