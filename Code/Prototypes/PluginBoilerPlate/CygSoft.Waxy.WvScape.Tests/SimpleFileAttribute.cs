using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace CygSoft.Waxy.WvScape.Tests
{
    public class SimpleFileAttribute : DataAttribute
    {
        private readonly string _filePath;

        /// <summary>
        /// Load data from a file as the data source for a theory
        /// </summary>
        /// <param name="filePath">The absolute or relative path to the file to load</param>
        public SimpleFileAttribute(string filePath)
        {
            _filePath = filePath;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null) { throw new ArgumentNullException(nameof(testMethod)); }

            // Get the absolute path to the file
            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            // Load the file
            var fileData = File.ReadAllText(_filePath);

            //if (string.IsNullOrEmpty(_propertyName))
            //{
            //    //whole file is the data
            //    return JsonConvert.DeserializeObject<List<object[]>>(fileData);
            //}

            //// Only use the specified property as the data
            //var allData = JObject.Parse(fileData);
            //var data = allData[_propertyName];
            return new List<object[]> { new object[] { fileData } };
        }
    }
}
