using System;
using System.ServiceModel;
using BusinessContractSearch.Entities;
using Exchange.Exweb.BusinessContractDataService.BusinessContractDataServiceProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace BusinessContractSearch.Test
{
    [TestClass]
    public class BusinessContractSearchTests
    {
        [TestMethod]
        public void FindByName_SearchCriteriaIsNull_ThrowsArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => new BusinessContractSearch().FindByName(null))
                .Message.ShouldContain("searchCriteria");
        }

        [TestMethod]
        public void FindByName_SearchCriteriaNameIsNull_ThrowsArgumentException()
        {
            var searchCriteria = new SearchCriteria();
            Should.Throw<ArgumentException>(() => new BusinessContractSearch().FindByName(searchCriteria))
                .Message.ShouldContain("ContractName");
        }

        [TestMethod]
        public void FindByName_SearchCriteriaNameIsEmpty_ThrowsArgumentException()
        {
            var searchCriteria = new SearchCriteria() { ContractName = string.Empty };
            Should.Throw<ArgumentException>(() => new BusinessContractSearch().FindByName(searchCriteria))
                .Message.ShouldContain("ContractName");
        }

        [TestMethod]
        public void FindByName_ContractNotFound_ThrowsFaultExceptionOfBusinessContractNotFoundFault()
        {
            var searchCriteria = new SearchCriteria() { ContractName = "adsjhgfasdjghfsadjhgf" };
            Should.Throw<FaultException<BusinessContractNotFoundFault>>(() => new BusinessContractSearch().FindByName(searchCriteria))
                .Message.ShouldContain("Business Contract cannot be found");
        }

        [TestMethod]
        public void FindByName_ContractFound_ContractDetailsReturned()
        {
            var searchCriteria = new SearchCriteria()
            {
                ContractName = "BrandingGroupX",
                IntegratorName = "Demo",
                ServiceName = "GetWOLQuoteV2"
            };

            var result = new BusinessContractSearch().FindByName(searchCriteria);

            result.ShouldNotBeNull();
            result.Contract.Name.ShouldBe(searchCriteria.ContractName);
            result.Contract.BusinessContractId.ShouldNotBe(Guid.Empty);
            result.SearchTime.ShouldBeGreaterThan(0);
        }
    }
}
