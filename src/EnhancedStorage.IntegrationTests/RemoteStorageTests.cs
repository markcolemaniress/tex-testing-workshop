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

            var server = FluentMockServer.Start();
            server.Given(Request.Create()
                            .WithPath("/remotestorage/test")
                            .UsingPost())
                  .RespondWith(Response.Create()
                            .WithStatusCode(200)
                            .WithHeader("Content-Type", "text/plain")
                            .WithBody(responseStorageId.ToString())
               );

            // Fake target contract data
            File.WriteAllText($"C:\\Temp\\{contractId}.dat", $"{server.Urls[0]}/remotestorage/test");

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
            }

        }
    }
}
