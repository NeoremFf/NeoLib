using System.Linq;

namespace Neo.Random;

public class ProbabilityRand<TResult>
{
  public struct State(int probability, TResult result)
  {
    public readonly int Probability = probability;
    public readonly TResult Result = result;
  }

  private IRandomizer<int> Randomizer => randomizer ??= new SystemRandomizer();
  private State[] States { get; set; }

  private IRandomizer<int> randomizer;

  public ProbabilityRand(params State[] states) => 
    States = states;

  public ProbabilityRand(IRandomizer<int> randomizer, params State[] states) : this(states) =>
    this.randomizer = randomizer;

  public TResult Next()
  {
    var max = States.Sum(x => x.Probability) + 1;

    var range = 0;
    var index = Randomizer.Next(max);
    for (var i = 0; i < States.Length; i++)
    {
      range += States[i].Probability;
      if (index <= range)
        return States[i].Result;
    }

    return default;
  }
}