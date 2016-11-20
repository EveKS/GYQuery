using AngleSharp.Parser.Html;
using GYQuery.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GYQuery.Utility
{
    interface IGoogleQuery
    {
        Task<IEnumerable<QueryModel>> PageParser(string pageUri);
    }
    class GoogleQuery : IGoogleQuery
    {
        private const string DEFAULT_USER_AGENT_STRING =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
        private async Task<string> GetHttp(string requestUri)
            => await Task.Run(async () =>
            {
                HttpClientHandler handler = new HttpClientHandler { Proxy = null };
                handler.UseProxy = false;

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
                requestMessage.Headers.UserAgent.ParseAdd(DEFAULT_USER_AGENT_STRING);

                using (var client = new HttpClient(handler))
                {
                    HttpResponseMessage response = await client.SendAsync(requestMessage);
                    return await response.Content.ReadAsStringAsync();
                }
            });

        public async Task<IEnumerable<QueryModel>> PageParser(string pageUri)
            => await Task.Run(() =>
            Enumerable.Range(0, 1)
            .Select(_ =>
                GetHttp(pageUri))
            .Select(async content =>
            {
                var parser = new HtmlParser();
                var document = parser.Parse(await content.ConfigureAwait(false));

                var infoSelect = "div[class='srg'] div[class='g']";
                var info = document.QuerySelectorAll(infoSelect);
                return
                info.Select(element =>
                {
                    var tmp =
                        element.QuerySelector("cite")
                        .TextContent;
                    tmp = tmp.Contains(' ') ? tmp.Remove(tmp.IndexOf(' ')) : tmp;
                    var domen = tmp.Replace(@"https://", "").Replace(@"http://", "");
                    domen = domen.Contains('/') ? domen.Remove(domen.IndexOf('/')) : domen;
                    return new QueryModel
                    {
                        FindPade = tmp,
                        DomenName = domen
                    };
                }).Distinct();

            }).First()
            );
    }
}
