using System.Collections;
using System.Collections.Generic;
using LocalCode.HUIFramework.UI;
using UnityEngine;

public class BaseSystem<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //instance = new T();
            }
            return instance;
        }
    }
    public void OnAdd() { }
    public void OnUpdate() { }
    public void OnRemove() { }
    
}
