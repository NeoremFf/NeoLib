using Neo.Random;

namespace Tests.Random
{
  public class ControllingRandomizer : IRandomizer<int>
  {
    public int Max;
    public int Min;
    public int? Target;

    private readonly System.Random random = new System.Random();
    
    public int Next()
    {
      if (Target.HasValue)
        return Target.Value;
      
      return random.Next(Min, Max);
    }

    public int Next(int maxValue) => Next();
  }
}