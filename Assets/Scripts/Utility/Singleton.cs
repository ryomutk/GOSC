using UnityEngine;
using Sirenix.OdinInspector;

public class Singleton<T>:MonoBehaviour
where T:Singleton<T>
{
    static T _instance = null;
    public static T instance{get{return _instance;} private set{_instance = value;}}

    virtual protected void Awake()
    {
        if(_instance != null)
        {
            #if DEBUG
            Debug.Log(this.name + " is singleton! breaking...");
            #endif
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }
}