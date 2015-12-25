using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;
using System.Reflection;

namespace Spinpreach.MonsterGirlsDataBase
{
    public static class SessionWriter
    {

        public enum DOCUMENT
        {
            JSON,
            XML,
            REQUEST
        }

        public static string DocumentPath(DOCUMENT type, DateTime? time)
        {

            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            directory = Path.Combine(directory, "log");
            directory = Path.Combine(directory, Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().ManifestModule.Name));

            switch (type)
            {
                case DOCUMENT.JSON: directory = Path.Combine(directory, "JSON"); break;
                case DOCUMENT.XML: directory = Path.Combine(directory, "XML"); break;
                case DOCUMENT.REQUEST: directory = Path.Combine(directory, "REQUEST"); break;
            }

            if (time != null)
            {
                directory = Path.Combine(directory, "HISTORY");
                directory = Path.Combine(directory, DateTime.Now.ToString("yyyy-MM"));
                directory = Path.Combine(directory, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                directory = Path.Combine(directory, "TRANSACTION");
            }

            if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }

            return directory;
        }

        #region JsonWriter

        public static void JsonWriterTransaction(string api, string json)
        {
            try
            {
                var directory = DocumentPath(DOCUMENT.JSON, null);
                var file = string.Format("{0}.txt", api.Replace("/", "_"));
                var path = Path.Combine(directory, file);

                if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}.{1} >> {2}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name, ex.Message));
            }
        }

        public static void JsonWriterHistory(string api, string json, DateTime time)
        {
            try
            {
                var directory = DocumentPath(DOCUMENT.JSON, time);
                var file = string.Format("{0}_{1}.txt", api.Replace("/", "_"), DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
                var path = Path.Combine(directory, file);
                if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception : {0}.{1} >> {2}", ex.TargetSite.ReflectedType.FullName, ex.TargetSite.Name, ex.Message));
            }
        }

        #endregion

        #region XmlWriter

        public static void XmlWriterTransaction(string api, string json)
        {
            try
            {
                var directory = DocumentPath(DOCUMENT.XML, null);
                var file = string.Format("{0}.xml", api.Replace("/", "_"));
                var path = Path.Combine(directory, file);
                if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new XmlDictionaryReaderQuotas()))
                {
                    XDocument.Load(reader).Save(path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public static void XmlWriterHistory(string api, string json, DateTime time)
        {
            try
            {
                var directory = DocumentPath(DOCUMENT.XML, time);
                var file = string.Format("{0}_{1}.xml", api.Replace("/", "_"), DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
                var path = Path.Combine(directory, file);
                if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), new XmlDictionaryReaderQuotas()))
                {
                    XDocument.Load(reader).Save(path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}