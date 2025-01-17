﻿
using Imgur.API.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Imgur.API.Models.Impl
{
  /// <summary>Tag vote data.</summary>
  public class TagVotes : ITagVotes
  {
    /// <summary>The list of tags.</summary>
    [JsonConverter(typeof (TypeConverter<IEnumerable<TagVote>>))]
    public virtual IEnumerable<ITagVote> Tags { get; set; }
  }
}
