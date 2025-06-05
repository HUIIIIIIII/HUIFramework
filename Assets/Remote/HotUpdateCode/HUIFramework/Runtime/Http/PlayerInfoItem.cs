using System.Collections.Generic;
using HotUpdate.Data;
using HUIFramework.Common.Http;
using Newtonsoft.Json;
using UnityEngine;

namespace HUIFramework.Runtime.Http
{
    public class PlayerInfoItem : BaseHttpItem
    {
        public override string route => "get_player_info";
        public override void OnSuccess(string msg)
        {
            base.OnSuccess(msg);
            Debug.Log(msg);
        }

        public override void OnError(string msg)
        {
            base.OnError(msg);
            Debug.Log(msg);
        }

        public override Dictionary<string, object> GetUploadData()
        {
            var player_info = new PlayerInfo
            {
                Id = "test_id",
                Level = new LevelInfo[]
                {
                    new LevelInfo
                    {
                        LevelId = 1,
                        LevelName = "Level 1",
                    },
                    new LevelInfo
                    {
                        LevelId = 2,
                        LevelName = "Level 2",
                    }
                },
                Name = "Test Player",
                Rank = "Gold"
            };
            var upload_data = new Dictionary<string, object>
            {
                { "player_info","empty_str" },
                { "key", "test_key" },
            };
            return upload_data;
        }
    }
}