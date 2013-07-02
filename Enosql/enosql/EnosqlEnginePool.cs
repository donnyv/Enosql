using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enosql
{
    /// <summary>
    /// This is a Singleton class
    /// </summary>
    internal sealed class EnosqlEnginePool
    {
        static readonly EnosqlEnginePool instance = new EnosqlEnginePool();
        static readonly object padlock = new object();
        static readonly Dictionary<string, EnosqlEngine> _queue = new Dictionary<string, EnosqlEngine>();

        EnosqlEnginePool()
        {

        }

        //public sealed class Singleton
        //{
        //    private static readonly Singleton instance = new Singleton();

        //    private Singleton() { }

        //    public static Singleton Instance
        //    {
        //        get
        //        {
        //            return instance;
        //        }
        //    }
        //}

        public static EnosqlEnginePool Instance
        {
            get
            {
                return instance;
            }
        }

        public static EnosqlEngine GetInstance(string dbasePath)
        {
            EnosqlEngine engineInstance;
            if (_queue.TryGetValue(dbasePath.ToUpper(), out engineInstance))
                return engineInstance;
            else
            {
                engineInstance = new EnosqlEngine(dbasePath.ToUpper());
                _queue.Add(dbasePath.ToUpper(), engineInstance);
                return engineInstance;
            }
        }
    }
}
