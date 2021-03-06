﻿using DotNet2019Challenge.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System;

namespace DotNet2019Challenge.Services.Movies
{
    public class MoviesService
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private static MoviesService _instance;

        public MoviesService()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public static MoviesService Instance => _instance ?? (_instance = new MoviesService());

        public async Task<SearchResponse<Movie>> GetPopularMoviesAsync(int pageNumber = 1, string language = "en")
        {
            var uri = $"{AppSettings.ApiUrl}movie/popular?api_key={AppSettings.ApiKey}&language={language}&page={pageNumber}";

            var httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<SearchResponse<Movie>>(serialized, _serializerSettings));

            return result;
        }

        public async Task<SearchResponse<Movie>> GetTopRatedMoviesAsync(int pageNumber = 1, string language = "en")
        {
            var uri = $"{AppSettings.ApiUrl}movie/top_rated?api_key={AppSettings.ApiKey}&language={language}&page={pageNumber}";

            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<SearchResponse<Movie>>(serialized, _serializerSettings));

            return result;
        }

        public async Task<SearchResponse<TVShow>> GetPopularShowsAsync(int pageNumber = 1, string language = "en")
        {
            var uri = $"{AppSettings.ApiUrl}tv/popular?api_key={AppSettings.ApiKey}&language={language}&page={pageNumber}";

            var httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            SearchResponse<TVShow> result = await Task.Run(() => JsonConvert.DeserializeObject<SearchResponse<TVShow>>(serialized, _serializerSettings));

            return result;
        }

        public async Task<SearchResponse<TVShow>> GetTopRatedShowsAsync(int pageNumber = 1, string language = "en")
        {
            var uri = $"{AppSettings.ApiUrl}tv/top_rated?api_key={AppSettings.ApiKey}&language={language}&page={pageNumber}";

            var httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(uri);

            await HandleResponse(response);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<SearchResponse<TVShow>>(serialized, _serializerSettings));

            return result;
        }

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception(content);
                }

                throw new HttpRequestException(content);
            }
        }
    }
}