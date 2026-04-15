using BenchmarkDotNet.Running;
using PricingPlatform.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<PricingPipelineBenchmark>();
    }
}