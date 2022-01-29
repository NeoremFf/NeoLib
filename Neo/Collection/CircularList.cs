using System;
using System.Collections;
using System.Collections.Generic;
using Neo.Collection.Exceptions;

namespace Neo.Collection
{
  public interface ICircularList<T> : ICollection<T>
  {
    int Position { get; }
    T Current { get; }

    ICollection<T> All  { get; }

    T First { get; }
    T Last { get; }

    void Reset();
    
    T Next();
    T Previously();
    
    T ShiftLeftAt(int count);
    T ShiftRightAt(int count);

    ICollection<T> GetElementsFromLeft(int getCount);
    ICollection<T> GetElementsFromRight(int getCount);
  }
  
  public class CircularList<T> : ICircularList<T>
  {
    public bool IsReadOnly { get; }

    public int Count => collection.Count;

    public int Position { get; private set; } = 0;

    public T Current => CurrentNode.Value;
    
    public T First => Count > 0 
      ? collection.First!.Value
      : throw new EmptyCollectionException(nameof(CircularList<T>));

    public T Last => Count > 0 
      ? collection.Last!.Value
      : throw new EmptyCollectionException(nameof(CircularList<T>));
    
    public ICollection<T> All => collection;
    
    private readonly LinkedList<T> collection;

    private LinkedListNode<T> CurrentNode
    {
      get => currentNode ?? MoveAt(Position);
      set => currentNode = value;
    }
    private LinkedListNode<T> currentNode;

    public CircularList() => 
      collection = new LinkedList<T>();

    public CircularList(IEnumerable<T> collection) => 
      this.collection = new LinkedList<T>(collection);

    public IEnumerator<T> GetEnumerator() => 
      collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
      collection.GetEnumerator();

    public void Add(T item) => 
      collection.AddLast(item);

    public bool Remove(T item) => 
      collection.Remove(item);
    
    public void Clear() => 
      collection.Clear();

    public bool Contains(T item) => 
      collection.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => 
      collection.CopyTo(array, arrayIndex);

    public void Reset()
    {
      Position = 0;
      CurrentNode = collection.First;
    }

    public T Next()
    {
      if (Count == 0)
        throw new EmptyCollectionException(nameof(CircularList<T>));
      
      CurrentNode = CurrentNode.Next;
      
      if (++Position == Count)
      {
        Position = 0;

        CurrentNode = collection.First;
        return CurrentNode!.Value;
      }

      return CurrentNode!.Value;
    }

    public T Previously()
    {
      if (Count == 0)
        throw new EmptyCollectionException(nameof(CircularList<T>));
      
      CurrentNode = CurrentNode.Previous;
      
      if (--Position < 0)
      {
        Position = Count - 1;

        CurrentNode = collection.Last;
        return CurrentNode!.Value;
      }

      return CurrentNode!.Value;
    }

    public T ShiftLeftAt(int count)
    {
      var position = Position - count;
      if (position < 0)
        position += Count;

      if (position >= Count || position < 0)
        throw new ArgumentOutOfRangeException(nameof(count));

      Position = position;
      CurrentNode = MoveAt(Position);
      
      return this[Position];
    }

    public T ShiftRightAt(int count)
    {
      var position = Position + count;
      if (position >= Count)
        position -= Count;

      if (position >= Count || position < 0)
        throw new ArgumentOutOfRangeException(nameof(count));

      Position = position;
      CurrentNode = MoveAt(Position);
      
      return this[Position];
    }

    public ICollection<T> GetElementsFromLeft(int getCount)
    {
      var position = Position;
      var node = CurrentNode;
      
      Reset();
      
      var result = new List<T>();
      for (var i = 0; i < getCount; i++) 
        result.Add(Previously());

      Position = position;
      CurrentNode = node;
      
      return result;
    }

    public ICollection<T> GetElementsFromRight(int getCount)
    {
      var position = Position;
      var node = CurrentNode;
      
      Reset();
      
      var result = new List<T>();
      for (var i = 0; i < getCount; i++) 
        result.Add(Next());
      
      Position = position;
      CurrentNode = node;
      
      return result;
    }
    
    public T this[int index]
    {
      get => MoveAt(index).Value;
      set
      {
        var item = MoveAt(index);
        item.Value = value;
      }
    }

    private LinkedListNode<T> MoveAt(int index)
    {
      if (Count == 0)
        throw new EmptyCollectionException(nameof(CircularList<T>));
      
      if (index >= Count)
        throw new IndexOutOfRangeException(nameof(CircularList<T>));
      
      var item = collection.First;
      for (var i = 0; i < index; i++) 
        item = item!.Next;

      return item;
    }
  }
}