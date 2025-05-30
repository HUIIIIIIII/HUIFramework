namespace HUIFramework.Common.Http
{
    public class HttpItem
    {
        private int connect_times = 0;
        private const int retry_times =3;
        protected virtual string url { get; }

        protected virtual void OnSuccess(string msg)
        {
        }

        private void ConnectError(string msg)
        {
            if (connect_times < retry_times)
            {
                Send();
            }
            else
            {
                OnError(msg);
            }
        }
        protected virtual void OnError(string msg)
        {
           
        }

        protected virtual void OnProgress(float progress)
        {
        }

        public void Send()
        {
            connect_times++;
            EasyHttpSystem.Get(url).OnSuccess(responce => { OnSuccess(responce.text); })
                .OnError(error => { ConnectError(error.error); }).OnProgress(progress => { OnProgress(progress); }).Send();
        }
    }
}