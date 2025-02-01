namespace Neo.Random
{
  public interface IRandomizer<out TType>
  {
    TType Next();
    TType Next(int maxValue);
  }
}