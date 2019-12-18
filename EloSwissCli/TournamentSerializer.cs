using System.Collections.Generic;
using System.IO;
using System.Linq;
using EloSwiss;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace EloSwissCli
{
    public static class TournamentSerializer
    {
        public static void ToJson(this Tournament tournament, string outputPath)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.ContractResolver = new IgnorePropertiesResolver(new[] { "HadBye", "IsBye"});

            using var stream = new StreamWriter(outputPath);
            using var writer = new JsonTextWriter(stream); 
            serializer.Serialize(writer, tournament);
        }

        public static Tournament FromJson(this string path)
        {
            using var file = File.OpenText(path);
            using var reader = new JsonTextReader(file);
            var json = (JObject)JToken.ReadFrom(reader);
            var playerJson = (JArray)json["players"];
            var players = playerJson.ToObject<IList<string>>();
            var tournament = new Tournament
            {
                Name = json["name"].Value<string>(),
                Players = players.Select(p => new Player(p)).ToList()
            };
            return tournament;
        }
    }
}