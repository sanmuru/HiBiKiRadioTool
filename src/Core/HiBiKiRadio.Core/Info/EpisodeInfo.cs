﻿// Licensed to the Qtyi under one or more agreements.
// The Qtyi licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Qtyi.HiBiKiRadio.Json;
using System.Diagnostics;

namespace Qtyi.HiBiKiRadio.Info;

/// <summary>回信息。</summary>
[DebuggerDisplay("{Name}")]
public class EpisodeInfo : JsonObjectInfo
{
    /// <summary>回的ID。</summary>
    public int ID => this.JsonObject.id;
    /// <summary>回的名称。</summary>
    public string Name => this.JsonObject.name;
    /// <summary>回的媒体类型。</summary>
    public int? MediaType => this.JsonObject.media_type;
    /// <summary>回的视频信息。</summary>
    public VideoInfo? Video => this.JsonObject.video is null ? null : new(this.JsonObject.video);
    /// <summary>回的附加（幕后）视频信息。</summary>
    public VideoInfo? AdditionalVideo => this.JsonObject.additional_video is null ? null : new(this.JsonObject.additional_video);
    /// <summary>回的介绍（HTML版本）。</summary>
    public string? HtmlDescription => this.JsonObject.html_description;
    public Uri? LinkUri => string.IsNullOrEmpty(this.JsonObject.link_url) ? default : new Uri(this.JsonObject.link_url, UriKind.Absolute);
    /// <summary>回的更新时间（UTC）。</summary>
    public DateTime? UpdatedTimeUtc => string.IsNullOrEmpty(this.JsonObject.updated_at) ? default : this.TryParseDateTimeUtc(this.JsonObject.updated_at, out var dt) ? dt : default;
    /// <summary>回的更新时间。</summary>
    public DateTime? UpdatedTime => this.UpdatedTimeUtc.HasValue ? UtcToLocal(this.UpdatedTimeUtc.Value) : default;
    public EpisodePartInfo[] EpisodeParts => this.JsonObject.episode_parts.OrderBy(ep => ep.sort_order).Select(ep => new EpisodePartInfo(ep)).ToArray() ?? new EpisodePartInfo[0];
    public ChapterInfo[] Chapters => this.JsonObject.chapters?.Select(c => new ChapterInfo(c)).ToArray() ?? new ChapterInfo[0];

    /// <summary>
    /// 初始化<see cref="EpisodeInfo"/>的新实例。
    /// </summary>
    /// <param name="jObject">包装的JSON对象。</param>
    public EpisodeInfo(episode jObject) : base(jObject) { }
}
