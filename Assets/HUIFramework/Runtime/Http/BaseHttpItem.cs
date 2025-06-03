using System.Collections.Generic;
using UnityEngine;

namespace HUIFramework.Common.Http
{
    public class BaseHttpItem
    {
        public virtual string route { get; }

        public virtual void OnSuccess(string msg)
        {
        }

        public virtual void OnProgress(float progress)
        {
        }

        public virtual void OnError(string msg)
        {
        }

        public virtual Dictionary<string, object> GetUploadData()
        {
            return null;
        }
    }
}