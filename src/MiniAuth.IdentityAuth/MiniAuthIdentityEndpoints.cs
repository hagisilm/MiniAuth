﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniAuth.IdentityAuth.Helpers;
using MiniAuth.IdentityAuth.Models;
using System.Collections.Concurrent;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MiniAuth.Identity
{
    public class MiniAuthIdentityEndpoints
    {
        public static ConcurrentDictionary<string, RoleEndpointEntity> _endpointCache = new ConcurrentDictionary<string, RoleEndpointEntity>();
        public void MapEndpoints(IApplicationBuilder builder)
        {
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/miniauth/api/getAllEndpoints", async (HttpContext context,
                    ILogger<object> _logger,
                    MiniAuthIdentityDbContext _dbContext
                ) =>
                {
                    await OkResult(context, _endpointCache.Values.OrderByDescending<RoleEndpointEntity, string>(o => o.Id).ToJson());
                })
                .RequireAuthorization("miniauth-admin")
                ;

                endpoints.MapPost("/miniauth/login", async (HttpContext context
                    , ILogger<MiniAuthIdentityEndpoints> _logger
                    , MiniAuthIdentityDbContext _dbContext
                    , SignInManager<MiniAuthIdentityUser> signInManager
                    , UserManager<MiniAuthIdentityUser> userManager
                ) =>
                {
                    await Login(context, _logger, _dbContext, signInManager, userManager).ConfigureAwait(false);
                });

                endpoints.MapGet("/miniauth/api/getRoles", async (HttpContext context
                    , ILogger<MiniAuthIdentityEndpoints> _logger
                    , MiniAuthIdentityDbContext _dbContext
                    , SignInManager<MiniAuthIdentityUser> signInManager
                    , UserManager<MiniAuthIdentityUser> userManager
                ) =>
                {
                    var roles = (_dbContext.Roles.ToArray<MiniAuthIdentityRole>());
                    await OkResult(context, roles.ToJson());
                }).RequireAuthorization("miniauth-admin");

                endpoints.MapPost("/miniauth/api/saveRole", async (HttpContext context
                    , ILogger<MiniAuthIdentityEndpoints> _logger
                    , MiniAuthIdentityDbContext _dbContext
                    , SignInManager<MiniAuthIdentityUser> signInManager
                    , UserManager<MiniAuthIdentityUser> userManager
                ) =>
                {
                    await SaveRole(context, _dbContext);
                }).RequireAuthorization("miniauth-admin");


                endpoints.MapPost("/miniauth/api/deleteRole", async (HttpContext context
                    , ILogger<MiniAuthIdentityEndpoints> _logger
                    , MiniAuthIdentityDbContext _dbContext
                    , SignInManager<MiniAuthIdentityUser> signInManager
                    , UserManager<MiniAuthIdentityUser> userManager
                ) =>
                {
                    await deleteRole(context, _dbContext);
                }).RequireAuthorization("miniauth-admin");

            });
            InitEndpointsCache(builder);

        }

        private async Task deleteRole(HttpContext context, MiniAuthIdentityDbContext _dbContext)
        {
            JsonDocument bodyJson = await GetBodyJson(context);
            var root = bodyJson.RootElement;
            var id = root.GetProperty<string>("Id");
            var role = await _dbContext.Roles.FindAsync(id);
            if (role != null)
            {
                _dbContext.Roles.Remove(role);
                await _dbContext.SaveChangesAsync();
            }
            await OkResult(context, "".ToJson(code: 200, message: ""));
        }

        private async Task SaveRole(HttpContext context, MiniAuthIdentityDbContext _dbContext)
        {
            JsonDocument bodyJson = await GetBodyJson(context);
            var root = bodyJson.RootElement;
            var id = root.GetProperty<string>("Id");
            var name = root.GetProperty<string>("Name");
            var enable = root.GetProperty<bool>("Enable");
            var type = root.GetProperty<string>("Type");
            var role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
            {
                role = new MiniAuthIdentityRole(name);
                role.Id = id;
                role.Enable = enable;
                role.Type = type;
                await _dbContext.Roles.AddAsync(role);
            }
            else
            {
                role.Name = name;
                role.Enable = enable;
                role.Type = type;
            }
            await _dbContext.SaveChangesAsync();
            await OkResult(context, "".ToJson(code: 200, message: ""));
        }

        private async Task Login(HttpContext context, ILogger<MiniAuthIdentityEndpoints> _logger, MiniAuthIdentityDbContext _dbContext, SignInManager<MiniAuthIdentityUser> signInManager, UserManager<MiniAuthIdentityUser> userManager)
        {
            JsonDocument bodyJson = await GetBodyJson(context);
            var root = bodyJson.RootElement;
            var userName = root.GetProperty<string>("username");
            var password = root.GetProperty<string>("password");
            var remember = root.GetProperty<bool>("remember");
            var result = await signInManager.PasswordSignInAsync(userName, password, remember, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                var newToken = Guid.NewGuid().ToString();
                context.Response.Cookies.Append("X-MiniAuth-Token", newToken);
                await OkResult(context, $"{{\"X-MiniAuth-Token\":\"{newToken}\"}}");
                return;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        private async Task<JsonDocument> GetBodyJson(HttpContext context)
        {
            var reader = new StreamReader(context.Request.Body);
            var body = await reader.ReadToEndAsync();
            var bodyJson = JsonDocument.Parse(body);
            return bodyJson;
        }

        private static void InitEndpointsCache(IApplicationBuilder builder)
        {
            var endpointDataSource = builder.ApplicationServices.GetRequiredService<EndpointDataSource>();
            var endpoints = endpointDataSource.Endpoints;
            foreach (var endpoint in endpoints)
            {
                var routeEndpoint = endpoint as RouteEndpoint;
                if (routeEndpoint != null)
                {
                    var id = routeEndpoint.DisplayName; //TODO
                    var methods = routeEndpoint.Metadata?.GetMetadata<HttpMethodMetadata>()?.HttpMethods.ToArray();
                    var route = routeEndpoint?.RoutePattern.RawText;
                    var isApi = routeEndpoint.Metadata?.GetMetadata<Microsoft.AspNetCore.Mvc.ApiControllerAttribute>() != null;
                    var roleEndpoint = new RoleEndpointEntity
                    {
                        Id = routeEndpoint.DisplayName,
                        Type = "system",
                        Name = routeEndpoint.DisplayName,
                        Route = route,
                        Methods = methods,
                        Enable = true,
                        RedirectToLoginPage = !isApi
                    };
                    MiniAuthIdentityEndpoints._endpointCache.TryAdd(id, roleEndpoint);
                }
            }
        }
        private static async Task OkResult(HttpContext context, string result, string contentType = "application/json")
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = contentType;
            context.Response.ContentLength = result != null ? Encoding.UTF8.GetByteCount(result) : 0;
            await context.Response.WriteAsync(result).ConfigureAwait(false);
        }
    }
}
