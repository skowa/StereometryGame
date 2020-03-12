using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Assets.Scripts.DataLoading
{
    public class JsonDataLoader : IDataLoader
    {
        private readonly string _filePath;

        public JsonDataLoader(string filePath)
        {
            _filePath = !string.IsNullOrWhiteSpace(filePath)
                ? filePath
                : throw new ArgumentNullException(nameof(filePath));
        }

        public Level LoadLevel(int number)
        {
            var fileContent = Resources.Load<TextAsset>(_filePath);
            JObject parsedLevels = JObject.Parse(fileContent.text);
            return parsedLevels.GetValue("Levels").First().ToObject<Level[]>()
                .FirstOrDefault(level => level.Number == number);
        }
    }
}