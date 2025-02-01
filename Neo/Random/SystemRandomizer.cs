using System.Security.Cryptography;

namespace Neo.Random
{
  public class SystemRandomizer : IRandomizer<int>
  {
    private readonly System.Random random;
    
    public int Next() => random.Next();

    public int Next(int maxValue) => random.Next(maxValue);
  }
}