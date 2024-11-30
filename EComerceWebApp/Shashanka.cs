using System;
using System.Collections;
using System.Collections.Generic;

public class Shashank<T> : ICollection<T>, IEnumerable<T>, IReadOnlyList<T>
{
    // Node class to represent each element in the linked list
    private class Node
    {
        public T Value;    // Value stored in the node
        public Node Next;  // Reference to the next node

        public Node(T value)
        {
            Value = value;
            Next = null;
        }
    }

    private Node head; // Head (start) of the list
    private Node tail; // Tail (end) of the list
    private int count; // Number of elements in the list

    // Constructor
    public Shashank()
    {
        head = null;
        tail = null;
        count = 0;
    }


    public void Add(T item)
    {
        var newNode = new Node(item);
        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            tail = newNode;
        }
        count++;
    }


    public bool Remove(T item)
    {
        Node current = head;
        Node previous = null;

        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, item))
            {
                if (previous == null) // Removing the head
                {
                    head = current.Next;
                    if (head == null) // If list becomes empty
                        tail = null;
                }
                else
                {
                    previous.Next = current.Next;
                    if (current.Next == null) // Removing the tail
                        tail = previous;
                }

                count--;
                return true;
            }

            previous = current;
            current = current.Next;
        }

        return false;
    }


    public void Insert(int index, T item)
    {
        if (index < 0 || index > count)
            throw new ArgumentOutOfRangeException(nameof(index));

        var newNode = new Node(item);

        if (index == 0) // Insert at the beginning
        {
            newNode.Next = head;
            head = newNode;
            if (tail == null) // If list was empty
                tail = newNode;
        }
        else
        {
            Node current = head;
            for (int i = 0; i < index - 1; i++)
                current = current.Next;

            newNode.Next = current.Next;
            current.Next = newNode;

            if (newNode.Next == null) // If inserted at the end
                tail = newNode;
        }

        count++;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0) // Remove the head
        {
            head = head.Next;
            if (head == null) // If list becomes empty
                tail = null;
        }
        else
        {
            Node current = head;
            for (int i = 0; i < index - 1; i++)
                current = current.Next;

            current.Next = current.Next.Next;
            if (current.Next == null) // If removed the tail
                tail = current;
        }

        count--;
    }


    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Node current = head;
            for (int i = 0; i < index; i++)
                current = current.Next;

            return current.Value;
        }
    }


    public bool Contains(T item)
    {
        foreach (var element in this)
        {
            if (EqualityComparer<T>.Default.Equals(element, item))
                return true;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < count)
            throw new ArgumentException("The destination array is too small.");

        Node current = head;
        while (current != null)
        {
            array[arrayIndex++] = current.Value;
            current = current.Next;
        }
    }

    // Clear the list
    public void Clear()
    {
        head = null;
        tail = null;
        count = 0;
    }


    public int Count => count;


    public bool IsReadOnly => false;


    public IEnumerator<T> GetEnumerator()
    {
        Node current = head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    // IEnumerable implementation
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}