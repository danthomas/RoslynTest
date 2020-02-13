namespace TaskRunner
{
    public interface IState
    {
        T GetState<T>() where T : class;
    }
}