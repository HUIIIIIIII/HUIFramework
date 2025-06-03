using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Best.HTTP;
using Best.HTTP.Request.Upload;
using Best.HTTP.Request.Upload.Forms;
using Cysharp.Threading.Tasks;
using HotUpdate.Data;
using HUIFramework.Common.Http;
using Newtonsoft.Json;

namespace HUIFramework.Common
{
    public class EasyHttpSystem : SingletonMonoBase<EasyHttpSystem>
    {
        private const string base_route = "http://localhost:5000/";

        public override async UniTask InitAsync()
        {
        }

        public void Post(BaseHttpItem http_item)
        {
            var route = base_route + http_item.route;
            
            var request = HTTPRequest.CreatePost(route,((req, resp) =>
            {
                if (req.State == HTTPRequestStates.Finished && resp.IsSuccess)
                {
            
                    http_item.OnSuccess(JsonConvert.SerializeObject(resp.DataAsText));
                }
                else
                {
                    http_item.OnError(JsonConvert.SerializeObject(resp));
                }
            } ));
            var data_stream = new MultipartFormDataStream();
            foreach (var item in http_item.GetUploadData())
            {
                data_stream.AddField(item.Key, JsonConvert.SerializeObject(item.Value));
            }
            request.UploadSettings.UploadStream = data_stream;
            request.Send();
        }

        public void Get(BaseHttpItem http_item)
        {
        }
    }
}