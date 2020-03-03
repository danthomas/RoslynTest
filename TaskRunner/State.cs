namespace Tests
{
    class State : IState
    {
        public T GetState<T>() where T : class
        {
            if (typeof(T) == typeof(Solution))
            {
                return Solution as T;
            }

            return default;
        }

        public Solution Solution { get; set; }
    }
}