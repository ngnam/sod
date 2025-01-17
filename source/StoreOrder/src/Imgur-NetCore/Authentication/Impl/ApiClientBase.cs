﻿using Imgur.API.Models;
using System;

namespace Imgur.API.Authentication.Impl
{
  /// <summary>Base Client class.</summary>
  public abstract class ApiClientBase : IApiClient
  {
    /// <summary>
    ///     Initializes a new instance of the ApiClientBase class.
    /// </summary>
    internal ApiClientBase()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the ApiClientBase class.
    /// </summary>
    /// <param name="clientId">The Imgur app's ClientId. </param>
    /// <exception cref="T:System.ArgumentNullException">
    ///     Thrown when a null reference is passed to a method that does not accept it as a
    ///     valid argument.
    /// </exception>
    protected ApiClientBase(string clientId)
    {
      if (string.IsNullOrWhiteSpace(clientId))
        throw new ArgumentNullException(nameof (clientId));
      this.ClientId = clientId;
    }

    /// <summary>
    ///     Initializes a new instance of the ApiClientBase class.
    /// </summary>
    /// <param name="clientId">The Imgur app's ClientId. </param>
    /// <param name="clientSecret">The Imgur app's ClientSecret.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///     Thrown when a null reference is passed to a method that does not accept it as a
    ///     valid argument.
    /// </exception>
    protected ApiClientBase(string clientId, string clientSecret)
      : this(clientId)
    {
      if (string.IsNullOrWhiteSpace(clientSecret))
        throw new ArgumentNullException(nameof (clientSecret));
      this.ClientSecret = clientSecret;
    }

    /// <summary>
    ///     Initializes a new instance of the ApiClientBase class.
    /// </summary>
    /// <param name="clientId">The Imgur app's ClientId. </param>
    /// <param name="oAuth2Token">OAuth2 credentials.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///     Thrown when a null reference is passed to a method that does not accept it as a
    ///     valid argument.
    /// </exception>
    protected ApiClientBase(string clientId, IOAuth2Token oAuth2Token)
      : this(clientId)
    {
      if (oAuth2Token == null)
        throw new ArgumentNullException(nameof (oAuth2Token));
      if (oAuth2Token.AccessToken == null)
        throw new ArgumentNullException("AccessToken");
      if (oAuth2Token.RefreshToken == null)
        throw new ArgumentNullException("RefreshToken");
      if (oAuth2Token.TokenType == null)
        throw new ArgumentNullException("TokenType");
      if (oAuth2Token.AccountId == null)
        throw new ArgumentNullException("AccountId");
      if (oAuth2Token.AccountUsername == null)
        throw new ArgumentNullException("AccountUsername");
      this.OAuth2Token = oAuth2Token;
    }

    /// <summary>
    ///     Initializes a new instance of the ApiClientBase class.
    /// </summary>
    /// <param name="clientId">The Imgur app's ClientId. </param>
    /// <param name="clientSecret">The Imgur app's ClientSecret.</param>
    /// <param name="oAuth2Token">OAuth2 credentials.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///     Thrown when a null reference is passed to a method that does not accept it as a
    ///     valid argument.
    /// </exception>
    protected ApiClientBase(string clientId, string clientSecret, IOAuth2Token oAuth2Token)
      : this(clientId, clientSecret)
    {
      if (oAuth2Token == null)
        throw new ArgumentNullException(nameof (oAuth2Token));
      if (oAuth2Token.AccessToken == null)
        throw new ArgumentNullException("AccessToken");
      if (oAuth2Token.RefreshToken == null)
        throw new ArgumentNullException("RefreshToken");
      if (oAuth2Token.TokenType == null)
        throw new ArgumentNullException("TokenType");
      if (oAuth2Token.AccountId == null)
        throw new ArgumentNullException("AccountId");
      if (oAuth2Token.AccountUsername == null)
        throw new ArgumentNullException("AccountUsername");
      this.OAuth2Token = oAuth2Token;
    }

    /// <summary>The Imgur app's ClientId.</summary>
    public virtual string ClientId { get; }

    /// <summary>The Imgur app's ClientSecret.</summary>
    public virtual string ClientSecret { get; }

    /// <summary>
    ///     The Endpoint Url.
    ///     https://api.imgur.com/3/ or https://imgur-apiv3.p.mashape.com/3/
    /// </summary>
    public abstract string EndpointUrl { get; }

    /// <summary>
    ///     An OAuth2 Token used for actions against a user's account.
    /// </summary>
    public virtual IOAuth2Token OAuth2Token { get; private set; }

    /// <summary>Remaining credits for the application.</summary>
    public virtual IRateLimit RateLimit { get; } = (IRateLimit) new Imgur.API.Models.Impl.RateLimit();

    /// <summary>Sets the oAuth2Token to be used.</summary>
    /// <param name="oAuth2Token">See <see cref="T:Imgur.API.Models.IOAuth2Token" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///     Thrown when a null reference is passed to a method that does not accept it as a
    ///     valid argument.
    /// </exception>
    public virtual void SetOAuth2Token(IOAuth2Token oAuth2Token)
    {
      if (oAuth2Token == null)
      {
        this.OAuth2Token = (IOAuth2Token) null;
      }
      else
      {
        if (oAuth2Token.AccessToken == null)
          throw new ArgumentNullException("AccessToken");
        if (oAuth2Token.RefreshToken == null)
          throw new ArgumentNullException("RefreshToken");
        if (oAuth2Token.TokenType == null)
          throw new ArgumentNullException("TokenType");
        if (oAuth2Token.AccountId == null)
          throw new ArgumentNullException("AccountId");
        if (oAuth2Token.AccountUsername == null)
          throw new ArgumentNullException("AccountUsername");
        this.OAuth2Token = oAuth2Token;
      }
    }
  }
}
