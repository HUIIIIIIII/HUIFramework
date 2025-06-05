using Newtonsoft.Json;

namespace HotUpdate.Data
{
    public class PlayerInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("level")]
        public LevelInfo[] Level { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rank")]
        public string Rank { get; set; }
    }
    public class LevelInfo
    {
        [JsonProperty("level_id")]
        public int LevelId { get; set; }

        [JsonProperty("level_name")]
        public string LevelName { get; set; }
    }
}
