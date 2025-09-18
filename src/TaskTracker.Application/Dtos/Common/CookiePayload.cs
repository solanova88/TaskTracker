using Microsoft.AspNetCore.Http;

namespace TaskTracker.Application.Dtos.Common;

public record CookiePayload(string Jwt, CookieOptions  CookieOptions);