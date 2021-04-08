/*
 * class Singleton -- C#单例类，用于管理只需生成一次的实例
 *
 * History:
 *  2021.04.07 CREATE
 */
using UnityEngine;

namespace Assets.Scripts.Util
{
    public class Singleton
    {
        private static Singleton _instance = new Singleton();

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("Can’t find " + typeof(Singleton) + "!");
                }
                return _instance;
            }
        }

        public Singleton() { }
    }
}