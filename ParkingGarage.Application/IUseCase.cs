namespace ParkingGarage.Application;

public interface IUseCase<in TIn, TOut>
{
    Task<TOut> ExecuteAsync(TIn request);
}

public interface IUseCase<in TIn>
{
    Task ExecuteAsync(TIn request);
}