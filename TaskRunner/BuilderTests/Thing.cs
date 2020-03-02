namespace TaskRunner.BuilderTests
{
    public class Thing
    {
        public void SetIntProp(int i)
        {
            IntProp = i;
        }

        public void SetStringProp<T>()
        {
            StringProp = typeof(T).Name;
        }

        public void SetStringProp<T>(T t)
        {
            StringProp = $"{typeof(T).Name} {t}";
        }

        public string StringProp { get; set; }

        public int IntProp { get; set; }

        public Runner GetService<T>()
        {
            return new Runner(typeof(T).Name);
        }
    }

    public class Runner
    {
        private readonly string _name;

        public Runner(string name)
        {
            _name = name;
        }

        public string Run(string s)
        {
            return $"{_name} {s}";
        }
    }
}