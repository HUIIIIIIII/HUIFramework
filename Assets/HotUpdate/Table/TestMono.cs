using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HUIFramework.Common;
using Newtonsoft.Json;
using Table;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test1().Forget();
    }

    public async UniTaskVoid Test1()
    {
        await GameTable<TestValue>.LoadTableValue();
        var value = GameTable<TestValue>.TableValueDic;
        foreach (var pair in value)
        {
            Debug.Log(pair.Key + ":" + JsonConvert.SerializeObject(pair.Value));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
