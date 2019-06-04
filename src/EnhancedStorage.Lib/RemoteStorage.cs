using Exchange.EntLib.Communications.Entities.Http;
using Exchange.EntLib.HttpConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnhancedStorage.Lib
{
    public class RemoteStorage
    {
        public Guid Store(string content, Guid contractId)
        {
            // Look up transmission data
            string endpointUrl = GetTransmissionData(contractId);

            // Configure connector
            var connector = new HttpConnector()
            {
                AuthenticationScheme = AuthenticationSchemeType.Anonymous,
                RequestUri = new Uri(endpointUrl),
                Timeout = new TimeSpan(0, 0, 5)
            };

            // Send
            var response = connector.Transmit(content);

            // Grab the response
            return new Guid(response);
        }

        private static string GetTransmissionData(Guid contractId)
        {
            // Fake target contract data
            return File.ReadAllText($"C:\\Temp\\{contractId}.dat");
        }
    }
}
