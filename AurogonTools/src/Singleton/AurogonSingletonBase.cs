using System;
namespace AurogonTools
{
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AurogonSingletonBase<T> where T : class,new()
    {
        private static T m_instance = null;
        private static readonly object m_lock = new object();

        public static T Instance
        {
            get
            {
                if(m_instance == null)
                {
                    lock(m_lock)
                    {
                        if(m_instance == null)
                        {
                            m_instance = new T();
                        }
                    }
                }

                return m_instance;
            }
        }


        protected AurogonSingletonBase() { }

        public virtual void OnInit() { }

        public virtual void OnClear() { }
    }
}
