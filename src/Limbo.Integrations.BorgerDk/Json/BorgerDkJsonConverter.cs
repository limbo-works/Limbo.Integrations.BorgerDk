using System;
using Newtonsoft.Json;

namespace Limbo.Integrations.BorgerDk.Json {

    public class BorgerDkJsonConverter : JsonConverter {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {

            BorgerDkMunicipality municipality = value as BorgerDkMunicipality;
            
            writer.WriteValue(municipality?.Code ?? 0);

        }

        public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer) {

            if (type == typeof(BorgerDkMunicipality)) {

                if (reader.TokenType == JsonToken.Integer) {
                    return BorgerDkMunicipality.GetFromCode((int) (long) reader.Value);
                }

                return BorgerDkMunicipality.NoMunicipality;

            }

            throw new Exception("Unsupported type " + type);

        }

        public override bool CanConvert(Type type) {
            return type == typeof(BorgerDkMunicipality);
        }

    }

}