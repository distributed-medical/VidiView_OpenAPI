using VidiView.Api.Access.Headers;
using VidiView.Api.DataModel;

namespace VidiView.Api.Access
{

    public static class HttpClientExtensions
    {
        static Dictionary<HttpClient, ApiHome> _cache = new();

        /// <summary>
        /// Helper extension to get the API starting point. The result will 
        /// be cached for subsequent calls on the specific HttpClient
        /// </summary>
        /// <param name="http"></param>
        /// <param name="forceReload">Force a reload, for instance after specifying an authentication header</param>
        /// <returns></returns>
        public static async Task<ApiHome> HomeAsync(this HttpClient http, bool forceReload = false)
        {
            if (!forceReload && _cache.TryGetValue(http, out var home))
                return home;

            HttpResponseMessage result;
            try
            {
                result = await http.GetAsync(""); // Utilizes BaseAddress
            }
            catch (Exception ex)
            {
                throw;
            }
            home = result.AssertSuccess().Deserialize<ApiHome>();

            _cache[http] = home;
            return home;
        }

        /// <summary>
        /// Invalidate the start page forcing a reload on next attempt
        /// </summary>
        /// <param name="http"></param>
        public static void InvalidateHome(this HttpClient http)
        {
            _cache.Remove(http);
        }

        /// <summary>
        /// Return the cached ApiHome (or null if no cached document exists)
        /// This is used internally
        /// </summary>
        /// <param name="http"></param>
        /// <returns></returns>
        internal static ApiHome? CachedHome(this HttpClient http)
        {
            if (_cache.TryGetValue(http, out var home))
                return home;
            else
                return null;
        }

        /// <summary>
        /// Helper method to get and response, verify success and then deserialize result
        /// </summary>
        /// <typeparam name="T">The type to deserialize</typeparam>
        /// <param name="http"></param>
        /// <param name="link">The link to read</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this HttpClient http, Link link)
        {
            var response = await http.GetAsync(link.ToUrl());
            if (typeof(T) == typeof(string))
            {
                // Don't deserialize
                return (T)(object)await response.AssertSuccess().Content.ReadAsStringAsync();
            }
            else
            {
                return response.AssertSuccess().Deserialize<T>();
            }
        }
    }
}

namespace VidiView.Api.Access.Authentication
{

    public static class HttpClientExtensions
    {
        /// <summary>
        /// Clear authentication
        /// </summary>
        /// <param name="http"></param>
        public static void ClearAuthentication(this HttpClient http)
        {
            http.DefaultRequestHeaders.Authorization = null;
            http.InvalidateHome();
        }

        /// <summary>
        /// Provide an api key
        /// </summary>
        /// <param name="http"></param>
        /// <param name="applicationId"></param>
        /// <param name="thumbprint"></param>
        /// <param name="key"></param>
        public static void SetApiKey(this HttpClient http, Guid applicationId, byte[] thumbprint, byte[] key)
        {
            SetApiKey(http, new ApiKeyHeader(applicationId, thumbprint, key));
        }

        /// <summary>
        /// Provide an api key
        /// </summary>
        /// <param name="http"></param>
        /// <param name="apikey"></param>
        public static void SetApiKey(this HttpClient http, ApiKeyHeader apikey)
        {
            http.DefaultRequestHeaders.Remove(apikey.Name);
            http.DefaultRequestHeaders.Add(apikey.Name, apikey.Value);
        }
    }
}