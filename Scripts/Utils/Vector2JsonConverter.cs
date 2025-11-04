using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public sealed class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("x"); writer.WriteValue(value.x);
        writer.WritePropertyName("y"); writer.WriteValue(value.y);
        writer.WriteEndObject();
    }

    public override Vector2 ReadJson(JsonReader reader, Type type, Vector2 existing, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        float x = obj["x"]?.Value<float>() ?? 0f;
        float y = obj["y"]?.Value<float>() ?? 0f;
        return new Vector2(x, y);
    }
}
