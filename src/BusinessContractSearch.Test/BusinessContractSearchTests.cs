using System;
using System.ServiceModel;
using BusinessContractSearch.Entities;
using Exchange.Exweb.BusinessContractDataService.BusinessContractDataServiceProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessContractSearch.Test
{
    [TestClass]
    public class BusinessContractSearchTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void FindByName_SearchCriteriaIsNull_ThrowsArgumentNullException()
        {
            new BusinessContractSearch().FindByName(null);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void FindByName_SearchCriteriaNameIsNull_ThrowsArgumentException()
        {
            var searchCriteria = new SearchCriteria();
            new BusinessContractSearch().FindByName(searchCriteria);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void FindByName_SearchCriteriaNameIsEmpty_ThrowsArgumentException()
        {
            var searchCriteria = new SearchCriteria() { ContractName = string.Empty };
            new BusinessContractSearch().FindByName(searchCriteria);
        }

        [TestMethod]
        public void FindByName_ContractNotFound_ThrowsFaultExceptionOfBusinessContractNotFoundFault()
        {
            var searchCriteria = new SearchCriteria()
            {
                ContractName = "ghchgfscadhgfchasdfc",
                IntegratorName = "Demo",
                ServiceName = "GetWOLQuoteV2"
            };

            try
            {
                new BusinessContractSearch().FindByName(searchCriteria);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (FaultException<BusinessContractNotFoundFault> ex)
            {
                Assert.IsTrue(ex.Message.Contains("Business Contract cannot be found"));
            }
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

            Assert.IsNotNull(result);
            Assert.AreEqual(searchCriteria.ContractName, result.Contract.Name);
            Assert.AreNotEqual(Guid.Empty, result.Contract.BusinessContractId);
            Assert.IsTrue(result.SearchTime > 0);
        }
    }
}
