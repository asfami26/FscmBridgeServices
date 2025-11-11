using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FscmBridgeServices.DTOS;
using FscmBridgeServices.Util;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;


namespace FscmBridgeServices.Helper
{
    public class JsonHelper
    {
        static HttpClient  _httpclient=new HttpClient();
        static DatabaseContext dbelo = new DatabaseContext();
        public static string ToJSON(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ParseJSON<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static async Task<CommonResponse<T>> SendJsonDataToUrlAsync<T>(string url, string req) where T : new()
        {
            CommonResponse<T> commonResponse = new CommonResponse<T>();
            T responseObj = new T();

            try
            {
                var dataString = new StringContent(req, Encoding.UTF8, "application/json");
                var response = await _httpclient.PostAsync(url, dataString);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseObj = JsonHelper.ParseJSON<T>(responseString);

                    commonResponse.StatusCode = (int)response.StatusCode;
                    commonResponse.Message = response.ReasonPhrase;
                    commonResponse.data = responseObj;
                }
                else
                {
                    var errorResponseString = await response.Content.ReadAsStringAsync();
                    responseObj = JsonHelper.ParseJSON<T>(errorResponseString);

                    commonResponse.StatusCode = (int)response.StatusCode;
                    commonResponse.Message = response.ReasonPhrase;
                    commonResponse.data = responseObj;
                }

                return commonResponse;
            }
            catch (HttpRequestException ex)
            {
                commonResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                commonResponse.Message = ex.Message;
                commonResponse.data = responseObj;

                return commonResponse;
            }
        }
        public static async Task<CommonResponse<T>> SendJsonDataToUrlAsync<T>(string url, object req) where T : new()
        {
            CommonResponse<T> commonResponse = new CommonResponse<T>();
            T responseObj = new T();

            try
            {
                var dataString = new StringContent(JsonHelper.ToJSON(req), Encoding.UTF8, "application/json");
                var response = await _httpclient.PostAsync(url, dataString);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseObj = JsonHelper.ParseJSON<T>(responseString);

                    commonResponse.StatusCode = (int)response.StatusCode;
                    commonResponse.Message = response.ReasonPhrase;
                    commonResponse.data = responseObj;
                }
                else
                {
                    var errorResponseString = await response.Content.ReadAsStringAsync();
                    responseObj = JsonHelper.ParseJSON<T>(errorResponseString);

                    commonResponse.StatusCode = (int)response.StatusCode;
                    commonResponse.Message = response.ReasonPhrase;
                    commonResponse.data = responseObj;
                }

                return commonResponse;
            }
            catch (HttpRequestException ex)
            {
                commonResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                commonResponse.Message = ex.Message;
                commonResponse.data = responseObj;

                return commonResponse;
            }
        }
        public static async Task<string> GetOrganizationUuidAsync(string contractUuid, string type, string baseUrl, string token)
        {
            string encodedContractUuid = Uri.EscapeDataString(contractUuid);
            string encodedType = Uri.EscapeDataString(type);
            string url = $"{baseUrl}?contractUuid={encodedContractUuid}&type={encodedType}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"Bearer {token}");

            using var response = await _httpclient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            string organizationUuid = root
                .GetProperty("content")[0]
                .GetProperty("uuid")
                .GetString();

            return organizationUuid ?? string.Empty;
        }
        
        public static async Task<(HttpStatusCode StatusCode, string ResponseBody)> UpdateJsonToUrl(
            string baseUrl, string buyerContractParticipantUuid, string token, object requestBody, string headerfscm)
        {
            string url = $"{baseUrl}/{buyerContractParticipantUuid}";
            string jsonBody = JsonConvert.SerializeObject(requestBody);

            using var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpclient.DefaultRequestHeaders.Add("Organization", headerfscm);

            using var response = await _httpclient.PutAsync(url, requestContent);
            string responseMessage = await response.Content.ReadAsStringAsync();

            return (response.StatusCode, responseMessage);
        }


       
        public static CommonResponse<List<string>> SendJsonDataToUrl(string url, object req, string token, string? organizationHeader)
        {
            var commonResponse = new CommonResponse<List<string>>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    if (!string.IsNullOrEmpty(organizationHeader))
                    {
                        httpClient.DefaultRequestHeaders.Add("Organization", organizationHeader);
                    }

                    var toJson =JsonHelper.ToJSON(req);
                    var content = new StringContent(toJson, Encoding.UTF8, "application/json");

                    using (var response = httpClient.PostAsync(url, content).Result)
                    {
                        var jsonResponse = response.Content.ReadAsStringAsync().Result;

                        commonResponse.StatusCode = (int)response.StatusCode;

                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                commonResponse.data = JsonConvert.DeserializeObject<List<string>>(jsonResponse) ?? new List<string>();
                            }
                            catch (JsonException)
                            {
                                commonResponse.Message = "Failed to deserialize response.";
                                return commonResponse;
                            }
                        }
                        else
                        {
                          
                            try
                            {
                                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                                {
                                    var root = doc.RootElement;

                                    if (root.TryGetProperty("error", out JsonElement errorElement))
                                    {
                                        commonResponse.Message = $"Error: {response.StatusCode} - {errorElement.GetString()}";
                                    }
                                    else if (root.TryGetProperty("errors", out JsonElement errorsElement) &&
                                             errorsElement.TryGetProperty("default", out JsonElement defaultElement) &&
                                             defaultElement.TryGetProperty("message", out JsonElement messageElement))
                                    {
                                        commonResponse.Message = $"Error: {response.StatusCode} - {messageElement.GetString()}";
                                    }
                                    else if (root.TryGetProperty("message", out JsonElement msgElement))
                                    {
                                        commonResponse.Message = $"Error: {response.StatusCode} - {msgElement.GetString()}";
                                    }
                                    else if (root.TryGetProperty("error", out JsonElement errorMsgElement))
                                    {
                                        commonResponse.Message = $"Error: {response.StatusCode} - {errorMsgElement.GetString()}";
                                    }
                                    else
                                    {
                                        commonResponse.Message = $"Error: {response.StatusCode} - {jsonResponse}";
                                    }
                                }
                            }
                            catch (JsonException)
                            {
                                commonResponse.Message = $"Error: {response.StatusCode} - Invalid JSON response: {jsonResponse}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commonResponse.StatusCode = 500;
                commonResponse.Message = $"Exception: {ex.Message}";
            }

            return commonResponse;
        }



        public static async Task<string> GetTokenSikp()
        {
            var res = new CommonResponse<AuthTokenFscm>();
            string token = string.Empty;
            string url_get_token = string.Empty;

          
            url_get_token = dbelo.Enummoduleparams
                .Where(a => a.MKey == "URL_GET_TOKEN_FSCM")
                .Select(a => a.MValue)
                .FirstOrDefault();

            token = dbelo.Enummoduleparams
                .Where(a => a.MKey == "USER_GET_TOKEN_FSCM")
                .Select(a => a.MValue) 
                .FirstOrDefault();

            
            if (string.IsNullOrEmpty(url_get_token) || string.IsNullOrEmpty(token))
            {
                throw new Exception("URL atau token tidak ditemukan.");
            }

          
            res = await SendJsonDataToUrlAsync<AuthTokenFscm>(url_get_token, token);

         
            if (res.StatusCode == 200)
            {
                return res.data.Access_Token.ToString();
            }

            return "";
        }

    }
}