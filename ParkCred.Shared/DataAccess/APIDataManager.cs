using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParkCred.Shared.Entities.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParkCred.Shared.DataAccess
{
    public class APIDataManager
    {
        public static async Task<string> GetSuggestedAddress(string lt, string lg)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://geocode-maps.yandex.ru/1.x/?lang=ru_RU&geocode=" + lg.Replace(",", ".") + "," + lt.Replace(",", ".") + "&kind=house&results=1&format=json");
                request.Timeout = 5000;
                HttpWebResponse responseSB = (HttpWebResponse)request.GetResponse();
                Stream dataStreamSB = responseSB.GetResponseStream();
                StreamReader readerSB = new StreamReader(dataStreamSB);
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(readerSB));
                string responseFromServer = readerSB.ReadToEnd();

                AddressSuggestModel geo = BuildGeoInfoShort(o, new AddressSuggestModel());
                readerSB.Close();
                dataStreamSB.Close();
                responseSB.Close();

                AddressModel result = new AddressModel();
                if (geo == null || string.IsNullOrWhiteSpace(geo.Address))
                {

                    request = WebRequest.Create("https://geocode-maps.yandex.ru/1.x/?lang=ru_RU&geocode=" + lg.Replace(",", ".") + "," + lt.Replace(",", ".") + "&kind=locality&results=1&format=json");
                    request.Timeout = 5000;
                    HttpWebResponse responseLoc = (HttpWebResponse)request.GetResponse();

                    Stream dataStreamLoc = responseLoc.GetResponseStream();

                    StreamReader readerLoc = new StreamReader(dataStreamLoc);
                    o = (JObject)JToken.ReadFrom(new JsonTextReader(readerLoc));

                    geo = BuildGeoInfoShort(o, new AddressSuggestModel());

                    readerLoc.Close();
                    dataStreamLoc.Close();
                    responseLoc.Close();
                }

                string address = string.Empty;
                if (geo.Address != null && geo.Description != null)
                {
                    string city = string.Empty;
                    int index = geo.Description.IndexOf(',');
                    city = geo.Description.Substring(0, index);

                    address = city + ", " + geo.Address;
                }

                return address;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static AddressSuggestModel BuildGeoInfoShort(JToken obj, AddressSuggestModel address)
        {
            try
            {
                if (obj is JProperty)
                {
                    JProperty jpr = obj as JProperty;
                    var tagName = jpr.Name;
                    if (tagName == "kind")
                    {
                        address.Kind = jpr.First.ToString();
                    }
                    if (tagName == "description")
                    {
                        address.Description = jpr.First.ToString();
                    }
                    if (tagName == "name")
                    {
                        address.Address = jpr.First.ToString();
                    }
                    if (tagName == "pos")
                    {
                        string[] coords = jpr.First.ToString().Split(new char[] { ' ' });
                        if (coords.Length == 2)
                        {
                            address.Lat = coords[1];
                            address.Lon = coords[0];
                        }
                    }
                }
                JEnumerable<JToken> joChildren = obj.Children();
                foreach (var item in joChildren)
                {
                    BuildGeoInfoShort(item, address);
                }
                return address;
            }
            catch (Exception ex)
            {
                return address;
            }
        }
    }
}
