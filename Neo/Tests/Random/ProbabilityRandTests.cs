using Neo.Random;
using NUnit.Framework;

namespace Tests.Random
{
  [TestFixture]
  public class ProbabilityRandTests
  { 
    [Test]
    public void WhenCreateWithSameProportionThanShouldBeEqual()
    {
      var randomizer = new ControllingRandomizer();
      
      int result;
      var rand = new ProbabilityRand<int>(randomizer,
        new ProbabilityRand<int>.State(1, 1),
        new ProbabilityRand<int>.State(1, 2),
        new ProbabilityRand<int>.State(1, 3));
      
      TestCase(1);
      TestCase(2);
      TestCase(3);
      
      void TestCase(int target)
      {
        randomizer.Target = target;
        result = rand.Next();
        Assert.AreEqual(result, target);
      }
     }

    [Test]
    public void WhenChanceIs10PercentShouldGenerateWith10Percent()
    {
      var randomizer = new ControllingRandomizer();
      
      var rand = new ProbabilityRand<int>(randomizer,
        new ProbabilityRand<int>.State(90, 1),
        new ProbabilityRand<int>.State(10, 2));

      randomizer.Min = 91;
      randomizer.Max = 100;

      for (int i = 0; i < 100000; i++)
      {
        var result = rand.Next();
        Assert.AreEqual(2, result);
      }
    }
  }
}