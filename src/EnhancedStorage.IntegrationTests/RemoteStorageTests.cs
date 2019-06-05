using System;
using System.IO;
using EnhancedStorage.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace EnhancedStorage.IntegrationTests
{
    [TestClass]
    public class RemoteStorageTests
    {
        [TestMethod]
        public void Store_ValidContent_ReturnsStorageId()
        {
            // Arrange
            Guid responseStorageId = Guid.NewGuid();
            Guid contractId = Guid.NewGuid();

            var server = CreateServer(responseStorageId);

            CreateTargetContract(contractId, 
                                $"{server.Urls[0]}/remotestorage/test");

            try
            {
                // Act
                var storageId = new RemoteStorage().Store("My test content", contractId);

                // Assert
                storageId.ShouldBe(responseStorageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                server.Stop();
                RemoveTargetContract(contractId);
            }

        }

        private FluentMockServer CreateServer(Guid responseStorageId)
        {
            var server = FluentMockServer.Start();

            server.Given(Request.Create()
                            .WithPath("/remotestorage/test")
                            .UsingPost())
                  .RespondWith(Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "text/plain")
                            .WithBody(responseStorageId.ToString())
               );

            return server;
        }

        private void CreateTargetContract(Guid contractId, string url)
        {
            // Store fake target contract data
            File.WriteAllText($"C:\\Temp\\{contractId}.dat", url);
        }

        private void RemoveTargetContract(Guid contractId)
        {
            // Store fake target contract data
            File.Delete($"C:\\Temp\\{contractId}.dat");
        }
    }
}
