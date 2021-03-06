﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

/*
 * url do poczytania = https://www.codeproject.com/Tips/397574/Use-Csharp-to-get-JSON-Data-from-the-Web-and-Map-i 
 *
 * 
 */

/*HOW TO USE
    List<ClearDataPerson> persons = new List<ClearDataPerson>();
    persons.Add(GetDataFromJson.GetClearPerson(Gender.RANDOM));
    Thread.Sleep(10);
 */

namespace ExternalAPI
{
    public enum Gender { MALE, FEMALE, RANDOM };
    public static class GetDataFromJson
    {
        private static string _randomPeopleUrl = @"https://api.namefake.com/polish-poland/";
        private static string _geoUrl = @"https://api.geoapify.com/v1/geocode/search?text=";
        private static string _geoKEY = "&apiKey=78cad425e0b745aaaf94c02f457ed95a";
        private static T _download_serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }

        }

        public static ClearDataPerson getClearPerson(char sex)
        {
            Logger.jsonPersonStart();
            string secondName = null;
            try
            {
               while(true)
                {
                    var person = _download_serialized_json_data<PersonFromAPI>(_randomPeopleUrl + ((sex == 'm') ? "male/" : ((sex == 'f') ? "female/" : "")));

                    if (RandomNumber.Draw(0, 1) == 0)
                    {
                        var addName = _download_serialized_json_data<PersonFromAPI>(_randomPeopleUrl + ((sex == 'm') ? "male/" : ((sex == 'f') ? "female/" : "")));
                        secondName = addName.name;
                    }
                    if(person.name != null)
                        return new ClearDataPerson(person.name, secondName, person.address);
                    Logger.clearPersonNameNull();
                }

            }
            catch (Exception e)
            {
                Logger.log(e);
                return null;
            }
        }


        public static ClearGeoAPI getGeo(string city, string street)
        {
            Logger.jsonGeoStart();
            try
            {
                var place = _download_serialized_json_data<GeoAPI>($"{_geoUrl}{street},{city}{_geoKEY}");

                return new ClearGeoAPI(place.features[0].properties.city, place.features[0].properties.street,
                                           place.features[0].properties.state, place.features[0].properties.country, place.features[0].properties.lon.ToString(),
                                           place.features[0].properties.lat.ToString());

            }
            catch (Exception e)
            {
                Logger.log(e);
                return new ClearGeoAPI("null", "null", "null", "null", "null", "null");
            }
        }


    }
}
