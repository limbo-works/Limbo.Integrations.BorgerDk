using System;
using Limbo.Integrations.BorgerDk.Exceptions;
using Newtonsoft.Json;

namespace Limbo.Integrations.BorgerDk.Json;

public class BorgerDkJsonConverter : JsonConverter {

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        BorgerDkMunicipality? municipality = value as BorgerDkMunicipality;
        writer.WriteValue(municipality?.Code ?? 0);
    }

    public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer) {
        if (type != typeof(BorgerDkMunicipality)) throw new BorgerDkException($"Unsupported type {type}");
        return reader.TokenType == JsonToken.Integer ? BorgerDkMunicipality.GetFromCode((int) (long) reader.Value) : BorgerDkMunicipality.NoMunicipality;
    }

    public override bool CanConvert(Type type) {
        return type == typeof(BorgerDkMunicipality);
    }

}