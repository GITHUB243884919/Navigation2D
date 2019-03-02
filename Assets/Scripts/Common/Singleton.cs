namespace UFrame.Common
{
    public class Singleton<T> where T : new() 
    {
        static public T GetInstance()
        {
            if (null == s_Instance)
            {
                s_Instance = new T();
            }
            return s_Instance;

        }

        static public void DestroyInstance()
        {
            s_Instance = default(T);
        }

        protected Singleton() { }

        private static T s_Instance = default(T);
    }


    public class SingletonMono<T> where T : UnityEngine.MonoBehaviour
    {
        static public T GetInstance()
        {
            return s_Instance;

        }

        static public void DestroyInstance()
        {
            s_Instance = default(T);
        }

        protected void Awake()
        {
            s_Instance = this as T;
        }
        protected SingletonMono() { }

        private static T s_Instance = default(T);
    }
}

