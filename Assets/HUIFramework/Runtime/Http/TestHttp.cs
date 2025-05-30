using System.Collections;
using System.Collections.Generic;
using HUIFramework.Common;
using UnityEngine;
using UnityEngine.UI;

namespace TestWeb {
    [ExecuteInEditMode]
    public class TestHttp : MonoBehaviour {
        public Text testGet;
        public Image testGetLoader;
        public RawImage testGetTexture;
        public Image testGetTextureLoader;
        public Text testPostJson;
        public Image testPostJsonLoader;
        public Text testPostJsonClass;
        public Image testPostJsonClassLoader;
        public Text testPost;
        public Image testPostLoader;
        public Text testPut;
        public Image testPutLoader;
        public Text testHeader;
        public Image testHeaderLoader;
        public Text testError;
        public Image testErrorLoader;
        public Text testTimeout;
        public Image testTimeoutLoader;

        public bool testExecuteInEditMode = false;

        private void Start () {
            EasyHttpSystem.debug = true;
        }

        private void Update () {
            if (testExecuteInEditMode) {
                testExecuteInEditMode = false;

                EasyHttpSystem.Get("https://soultri.site/test/testGet.php")
                .OnSuccess(result => {
                    Debug.Log(result.text);
                })
                .Send();
            }
        }

        public void TestGet () {
            testGet.text = "";
            testGetLoader.fillAmount = 0;

            EasyHttpSystem.Get("https://soultri.site/test/testGet.php")
                .OnProgress(progress => {
                    testGetLoader.fillAmount = progress;
                })
                .OnSuccess(result => {
                    testGetLoader.fillAmount = 1;
                    testGet.text = result.text;
                })
                .Send();
        }

        public void TestGetTexture () {
            testGetTexture.texture = null;
            testGetTextureLoader.fillAmount = 0;

            EasyHttpSystem.GetTexture("https://w.forfun.com/fetch/36/36b936cd1fb8b0523e029ab76bae3373.jpeg")
                .OnProgress(progress =>
                {
                    testGetTextureLoader.fillAmount = progress;
                })
                .OnSuccess(result =>
                {
                    testGetTextureLoader.fillAmount = 1;
                    testGetTexture.texture = result.texture;
                })
                .Send();
        }

        public void TestPostJson () {
            testPostJson.text = "";
            testPostJsonLoader.fillAmount = 0;

            var testJson = new TestJsonClass();
            testJson.count = 30;
            testJson.value = true;

            EasyHttpSystem.PostJson("https://soultri.site/test/testJson.php", JsonUtility.ToJson(testJson))
                .OnProgress(progress => {
                    testPostJsonLoader.fillAmount = progress;
                })
                .OnSuccess(result => {
                    testPostJsonLoader.fillAmount = 1;
                    testPostJson.text = result.text;
                })
                .Send();
        }

        public void TestPostClass () {
            testPostJsonClass.text = "";
            testPostJsonClassLoader.fillAmount = 0;

            var testJson = new TestJsonClass();
            testJson.count = 30;
            testJson.value = true;

            EasyHttpSystem.PostJson("https://soultri.site/test/testJson.php", JsonUtility.ToJson(testJson))
                .OnProgress(progress => {
                    testPostJsonClassLoader.fillAmount = progress;
                })
                .OnSuccess(result => {
                    testPostJsonClassLoader.fillAmount = 1;
                    testPostJsonClass.text = result.text;
                })
                .OnSuccessClass<TestJsonClass>(testJsonClass => {
                    Debug.Log(testJsonClass.count);
                    Debug.Log(testJsonClass.value);
                })
                .Send();
        }

        public void TestPost () {
            testPost.text = "";
            testPostLoader.fillAmount = 0;

            WWWForm testForm = new WWWForm();
            testForm.AddField("test", "test value");

            EasyHttpSystem.Post("https://soultri.site/test/testPost.php", testForm)
                .OnProgress(progress => {
                    testPostLoader.fillAmount = progress;
                })
                .OnSuccess(result => {
                    testPostLoader.fillAmount = 1;
                    testPost.text = result.text;
                })
                .Send();
        }

        public void TestPut () {
            testPut.text = "";
            testPutLoader.fillAmount = 0;

            EasyHttpSystem.Put("https://soultri.site/test/testPut.php", "Test PUT data")
                .OnProgress(progress => {
                    testPutLoader.fillAmount = progress;
                })
                .OnSuccess(result => {
                    testPutLoader.fillAmount = 1;
                    testPut.text = result.text;
                })
                .Send();
        }

        public void TestHeader () {
            testHeader.text = "";
            testHeaderLoader.fillAmount = 0;

            EasyHttpSystem.Get("https://soultri.site/test/testHeader.php")
                .SetHeader("test", "test value")
                .OnProgress(progress => {
                    testHeaderLoader.fillAmount = progress;
                })
                .OnSuccess(result => {
                    testHeaderLoader.fillAmount = 1;
                    testHeader.text = result.text;
                })
                .Send();
        }

        public void TestError () {
            testError.text = "";
            testErrorLoader.fillAmount = 0;

            EasyHttpSystem.Get("https://soultri.site/test/testError.php")
                .OnProgress(progress => {
                    testErrorLoader.fillAmount = progress;
                })
                .OnError(result => {
                    testErrorLoader.fillAmount = 1;
                    testError.text = result.error;
                })
                .Send();
        }

        public void TestTimeout () {
            testTimeout.text = "";
            testTimeoutLoader.fillAmount = 0;

            EasyHttpSystem.Get("https://soultri.site/test/testTimeout.php")
                .SetTimeout(1)
                .OnProgress(progress => {
                    testTimeoutLoader.fillAmount = progress;
                })
                .OnError(result => {
                    testTimeoutLoader.fillAmount = 1;
                    testTimeout.text = result.error;
                })
                .Send();
        }

        public void TestStartAll () {
            TestGet();
            TestGetTexture();
            TestHeader();
            TestError();
            TestPost();
            TestPut();
            TestPostJson();
            TestTimeout();
            TestPostClass();
        }
    }

    public class TestJsonClass {
        public int count;
        public bool value;
    }
}