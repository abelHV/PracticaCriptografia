using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientCriptografia
{
    public class ClPacket
    {
        [JsonPropertyName("tipus")]
        public string Tipus { get; set; }

        [JsonPropertyName("dades")]
        public string DadesBase64 { get; set; }

        // Propietats opcionals per si es connecta al xat central multiusuari
        [JsonPropertyName("destinatari")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Destinatari { get; set; }

        [JsonPropertyName("emissor")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Emissor { get; set; }

        public ClPacket(string tipus, byte[] dadesRaw)
        {
            Tipus = tipus;
            DadesBase64 = Convert.ToBase64String(dadesRaw);
        }

        public ClPacket() { }

        public string ToJson() => JsonSerializer.Serialize(this);

        public static ClPacket FromJson(string json) => JsonSerializer.Deserialize<ClPacket>(json);
    }
}