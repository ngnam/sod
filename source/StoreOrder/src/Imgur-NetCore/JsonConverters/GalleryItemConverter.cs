﻿
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Imgur.API.JsonConverters
{
  /// <summary>Converts Gallery items to their appropriate type.</summary>
  public class GalleryItemConverter : JsonConverter
  {
    /// <summary>
    ///     Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns></returns>
    public override bool CanConvert(Type objectType)
    {
      return typeof (IGalleryItem).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns></returns>
    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.None)
        return (object) null;
      string str = JObject.Load(reader).ToString();
      if (str.Replace(" ", "").IndexOf("is_album\":true", StringComparison.OrdinalIgnoreCase) >= 0)
        return (object) JsonConvert.DeserializeObject<GalleryAlbum>(str);
      return (object) JsonConvert.DeserializeObject<GalleryImage>(str);
    }

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
