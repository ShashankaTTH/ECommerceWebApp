using System;
using System.Collections;
using System.Collections.Generic;

public class Shashank<T> : ICollection<T>, IEnumerable<T>, IReadOnlyList<T>
{
    
    private class Node
    {
        public T Value;   
        public Node Next; 

        public Node(T value)
        {
            Value = value;
            Next = null;
        }
    }

    private Node head; 
    private Node tail; 
    private int count; 


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
                if (previous == null) 
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

    // Check if the list contains an item
    public bool Contains(T item)
    {
        foreach (var element in this)
        {
            if (EqualityComparer<T>.Default.Equals(element, item))
                return true;
        }
        return false;
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
