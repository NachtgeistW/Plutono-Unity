/*
 * class MonoSingleton<T> -- 继承了MonoBehaviour的单例类，用于管理只需生成一次的实例
 *
 * History:
 *  2020.7.29 CREATE
 */

using UnityEngine;

namespace Util
{
    public class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {

        //此类的单个实例
        private static T _instance;

        //访问器。第一次调用时，将设置_instance
        //如果找不到合适的对象，将记录一个错误
        public static T Instance
        {
            get
            {
                //如果还没有设置_instance……
                if (_instance == null)
                {
                    //尝试找到该对象
                    _instance = FindObjectOfType<T>();
                    //Tell unity not to destroy this object when loading a new scene!
                    DontDestroyOnLoad(_instance.gameObject);

                    //如果找不到，就记录错误
                    if (_instance == null)
                    {
                        Debug.LogError("Can’t find " + typeof(T) + "!");
                    }
                }

                //返回实例供使用
                return _instance;
            }
        }

    }
}

/*
 * Singleton 类的工作方式如下：其他类将是这个模板类的子类，并获得一个名为 instance 的静态属性。
 * 此属性始终指向该类的共享实例。这意味着当其他脚本请求 InputManager.instance 时，总是会得到单例 InputManager。
 * 这种方法的优点是，需要 InputManager 的脚本不需要连接到 InputManager 的变量。
 */

//此类允许其他对象引用单个共享对象
//GameManager和InputManager使用此类

//为使用此类，需要进行继承，如：
// public class MyManager : Singleton<MyManager> {  }

//然后就可以访问此类的单个共享实例，如：
// MyManager.instance.DoSomething();