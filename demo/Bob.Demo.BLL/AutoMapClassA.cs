using AutoMapper;

namespace Bob.Demo.BLL
{
    public class AutoMapClassA
    {
        public string Name { get; set; }
    }

    [AutoMapFrom(typeof(AutoMapClassA))]
    public class AutoMapClassB
    {
        public string Name { get; set; }
    }

    public class TestAutoMap
    {
        public static string Test()
        {
            AutoMapClassA a = new AutoMapClassA() { Name = "一" };
            return a.MapTo<AutoMapClassB>().Name;
        }
    }
}
