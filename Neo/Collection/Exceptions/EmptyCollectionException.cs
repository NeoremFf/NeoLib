using System;

namespace Neo.Collection.Exceptions
{
  public class EmptyCollectionException : Exception
  {
    public EmptyCollectionException(string collectionName) : base($"Collection is empty: {collectionName}.")
    { }
  }
}