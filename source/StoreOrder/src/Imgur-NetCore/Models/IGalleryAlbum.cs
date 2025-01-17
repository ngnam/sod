﻿
using Imgur.API.Enums;
using System;
using System.Collections.Generic;

namespace Imgur.API.Models
{
  /// <summary>The data model formatted for gallery albums.</summary>
  public interface IGalleryAlbum : IGalleryItem, IDataModel
  {
    /// <summary>The account ID or null if it's anonymous.</summary>
    int? AccountId { get; set; }

    /// <summary>The account username or null if it's anonymous.</summary>
    string AccountUrl { get; set; }

    /// <summary>Number of comments on the gallery item.</summary>
    int? CommentCount { get; set; }

    /// <summary>The ID of the album cover image.</summary>
    string Cover { get; set; }

    /// <summary>The height, in pixels, of the album cover image.</summary>
    int? CoverHeight { get; set; }

    /// <summary>The width, in pixels, of the album cover image.</summary>
    int? CoverWidth { get; set; }

    /// <summary>
    ///     Utc timestamp of when the album was inserted into the gallery, converted from epoch time.
    /// </summary>
    DateTimeOffset DateTime { get; set; }

    /// <summary>
    ///     OPTIONAL, the deletehash, if you're logged in as the album owner.
    /// </summary>
    string DeleteHash { get; set; }

    /// <summary>The description of the album in the gallery.</summary>
    string Description { get; set; }

    /// <summary>Number of downvotes for the item.</summary>
    int? Downs { get; set; }

    /// <summary>
    ///     Indicates if the current user favorited the image. Defaults to false if not signed in.
    /// </summary>
    bool? Favorite { get; set; }

    /// <summary>The ID for the album.</summary>
    string Id { get; set; }

    /// <summary>
    ///     A list of all the images in the album (only available when requesting the direct album).
    /// </summary>
    IEnumerable<IImage> Images { get; set; }

    /// <summary>The total number of images in the album.</summary>
    int ImagesCount { get; set; }

    /// <summary>The view layout of the album.</summary>
    AlbumLayout? Layout { get; set; }

    /// <summary>The URL link to the album.</summary>
    string Link { get; set; }

    /// <summary>
    ///     Indicates if the image has been marked as nsfw or not. Defaults to null if information is not available.
    /// </summary>
    bool? Nsfw { get; set; }

    /// <summary>
    ///     Order number of the album on the user's album page (defaults to 0 if their albums haven't been reordered).
    /// </summary>
    int Order { get; set; }

    /// <summary>Upvotes minus downvotes.</summary>
    int? Points { get; set; }

    /// <summary>
    ///     The privacy level of the album, you can only view public if not logged in as album owner.
    /// </summary>
    AlbumPrivacy? Privacy { get; set; }

    /// <summary>Imgur popularity score.</summary>
    int? Score { get; set; }

    /// <summary>
    ///     If the image has been categorized then this will contain the section the image belongs in. (funny, cats,
    ///     adviceanimals, wtf, etc)
    /// </summary>
    string Section { get; set; }

    /// <summary>The title of the album in the gallery.</summary>
    string Title { get; set; }

    /// <summary>Topic of the gallery item.</summary>
    string Topic { get; set; }

    /// <summary>Topic ID of the gallery item.</summary>
    int? TopicId { get; set; }

    /// <summary>Upvotes for the item.</summary>
    int? Ups { get; set; }

    /// <summary>The number of album views.</summary>
    int Views { get; set; }

    /// <summary>
    ///     The current user's vote on the item. null if not signed in or if the user hasn't voted on it.
    /// </summary>
    VoteOption? Vote { get; set; }
  }
}
