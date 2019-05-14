using BusinessContractSearch.Entities;
using Exchange.Exweb.BusinessContractDataService.ClientLibrary;
using Exchange.Exweb.BusinessContractDataService.ClientLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessContractSearch
{
    public class BusinessContractSearch
    {
        public SearchResults FindByName(SearchCriteria searchCriteria, bool logSearch = false)
        {
            Stopwatch sw = Stopwatch.StartNew();

            ValidateSearchCriteria(searchCriteria);

            BusinessContractSearchKeys keys = MapRequest(searchCriteria);

            var response = new BusinessContractDataServiceClientLibrary().GetBusinessContractDetails(keys);

            var result = MapResponse(response);

            sw.Stop();
            result.SearchTime = sw.ElapsedMilliseconds;

            return result;
        }

        private static BusinessContractSearchKeys MapRequest(SearchCriteria searchCriteria)
        {
            return new BusinessContractSearchKeys()
            {
                ContractName = searchCriteria.ContractName,
                IntegratorName = searchCriteria.IntegratorName,
                ServiceName = searchCriteria.ServiceName
            };
        }

        private static SearchResults MapResponse(BusinessContract response)
        {
            return new SearchResults()
            {
                Contract = new ContractDetails()
                {
                    BusinessContractId = response.BusinessContractId,
                    Name = response.ContractName
                }
            };
        }

        private static void ValidateSearchCriteria(SearchCriteria searchCriteria)
        {
            if (searchCriteria == null)
            {
                throw new ArgumentNullException(nameof(searchCriteria));
            }

            if (string.IsNullOrEmpty(searchCriteria.ContractName))
            {
                throw new ArgumentException(nameof(searchCriteria.ContractName));
            }
        }
    }
}
