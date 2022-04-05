namespace Demo;

public interface ISampleService
{
    Task<string> HelloWorld(CancellationToken cancellationToken);
}

public class SampleService : ISampleService
{
    public Task<string> HelloWorld(CancellationToken cancellationToken)
    {
        return Task.FromResult("Hello World");
    }
}