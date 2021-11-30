using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace SockerServerTCP
{
    /// <summary>
    /// Generate message for request from dictionary
    /// </summary>
    internal class Message
    {
        private ConcurrentDictionary<IPEndPoint, int> _connectionDict;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionDic">Concurrent dictionary by which the search is perfomed</param>
        public Message (ConcurrentDictionary<IPEndPoint, int> connectionDic)
        {
            _connectionDict = connectionDic;
        }

        /// <summary>
        /// Create response message
        /// </summary>
        /// <param name="ip">IPEndPoint by the called client</param>
        /// <param name="text">Client entering the text </param>
        /// <returns>String</returns>
        public string GenerateMessage(IPEndPoint ip, string text)
        {
            text = text.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(text)) return @"Empty string";
            try
            {
                return text switch
                {
                    "list" => GetAllRecordsFromDict(),
                    "help" => GetExistCommands(),
                    _ => GetSum(ip, text)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return @"Error parsing string";
            }
        }
       
        /// <summary>
        /// Get exist command
        /// </summary>
        /// <returns>Discriptions command</returns>
        private string GetExistCommands()
        {
            return @"list - Get all list IP and sum";
        }

        /// <summary>
        /// Get all record from dictionary
        /// </summary>
        /// <returns></returns>
        private string GetAllRecordsFromDict()
        {
            if (_connectionDict.Count <= 0) return @"No records";
            var sb = new StringBuilder();
            try
            {
                foreach (var item in _connectionDict)
                {
                    sb.Append($"IP {item.Key.Address} - sum={item.Value} \n");
                }
                return sb.ToString();
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
                return @"Error from dictionary";
            }
        }

        /// <summary>
        /// Get Sum for client
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetSum(IPEndPoint ip, string str)
        {
            //TODO: You can add a check for the length of the int
            str = str.ToLower().Trim();
            int num = 0;
            if (!int.TryParse(str, out num)) return @"Not integer number";
            var dd = _connectionDict.AddOrUpdate(
                                            key: ip, 
                                            addValue: num, 
                                            updateValueFactory: (key, existValue) => existValue + num
                                            );
            return $"Sum {dd}";

        }
    }
}
