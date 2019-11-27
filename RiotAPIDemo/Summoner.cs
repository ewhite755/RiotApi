using Newtonsoft.Json;

namespace RiotAPIDemo
{
    public class Summoner
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("accountId")]
        public string AccountID { get; set; }

        [JsonProperty("puuid")]
        public string PuuId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profileIconId")]
        public int ProfileIconId { get; set; }

        [JsonProperty("revisionDate")]
        public long RevisionDate { get; set; }

        [JsonProperty("summonerLevel")]
        public long Level { get; set; }

    }
}

    //"id": "ChExhAUCl08qDgq6z6mHcyKQ2PJfJH5XSmdu8D6EKa554lk",
    //"accountId": "MXYElr-RLAMaGQnWOg8pacIMApgs58k9lfE2DaX3BSUuv3o",
    //"puuid": "JQkdopdkS3bdGdIGFB-kkJ3x9UVniHZ6lLy5f9qiyVUbuD6-bOasEPNI3CqfCzX2SVk-eT6ijZPE3A",
    //"name": "reg454",
    //"profileIconId": 691,
    //"revisionDate": 1574324004000,
    //"summonerLevel": 69