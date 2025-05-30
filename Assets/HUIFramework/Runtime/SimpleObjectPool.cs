using System;
using System.Collections.Generic;
using System.Linq;

namespace HUIFramework.Common
{
    public class SimpleObjectPool<T> where T : class
    {
        private readonly Stack<T> object_pool = new Stack<T>();
        private readonly Func<T> create_func;

        public SimpleObjectPool(Func<T> createFunc, int initialCount = 10)
        {
            create_func = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            for (int i = 0; i < initialCount; i++)
            {
                object_pool.Push(create_func());
            }
        }

        public T Get()
        {
            return object_pool.Count > 0 ? object_pool.Pop() : create_func();
        }
        
        public void Release(T obj)
        {
            if (obj == null) return;
            object_pool.Push(obj);
        }

        public void Clear()
        {
            object_pool.Clear();
        }

        public int Count => object_pool.Count;
    }
}