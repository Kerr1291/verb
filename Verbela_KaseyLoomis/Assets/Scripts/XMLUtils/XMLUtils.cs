using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

namespace verb
{
    /// <summary>
    /// Utility class to make saving/loading a single ojbect from XML easier
    /// </summary>
    public class XMLUtils : MonoBehaviour
    {
        /// <summary>
        /// Write object to file as xml
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="path">File path</param>
        /// <param name="data">Source object for the data to write</param>
        /// <returns></returns>
        public static bool WriteDataToFile<T>(string path, T data) where T : class
        {
            bool result = false;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream fstream = null;
            try
            {
                fstream = new FileStream(path, FileMode.Create);
                serializer.Serialize(fstream, data);
                result = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error writing xml to " + path + ":" + e.Message);
            }
            finally
            {
                fstream.Close();
            }
            return result;
        }

        /// <summary>
        /// Read object from file as xml
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="path">File path</param>
        /// <param name="data">object to store loaded data into</param>
        /// <returns></returns>
        public static bool ReadDataFromFile<T>(string path, out T data) where T : class
        {
            data = null;

            if (!File.Exists(path))
            {
                Debug.LogError("No file found at " + path + "  ");
                return false;
            }

            bool returnResult = true;

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream fstream = null;
            try
            {
                fstream = new FileStream(path, FileMode.Open);
                data = serializer.Deserialize(fstream) as T;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading xml from " + path + " with error: " + e.Message);
                returnResult = false;
            }
            finally
            {
                fstream.Close();
            }

            return returnResult;
        }
    }
}
