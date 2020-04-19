using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailingGeneratorBll.Services;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using MailingsGeneratorBll.Services;
using Moq;
using NUnit.Framework;

namespace MailingGeneratorBllTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            // перед запуском тестов
        }

        [Test]
        public void Test1()
        {
            /*
            
            // AAA arrange act assert
            
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            service.UpdateControlEventAsync(new UpdateControlEventModel()
            {
                Id = 4,
                MaxMark = 5,
                Date = "11.01"
            });

            // assert
            Assert.AreEqual("11.01", dataBase[4].Date);
            
            /*
            Assert.Pass();
            Assert.IsTrue();
            Assert.IsEmpty();
            Assert.AreEqual();
            Assert.Throws();
            */
        }

        private async Task<(Mock<IControlEventRepository> repository, Dictionary<int, ControlEvent> dataBase)> GetMock()
        {/*
            var repository = new Mock<IControlEventRepository>(MockBehavior.Strict);
            var dataBase = new Dictionary<int, ControlEvent>()
                {
                    [4] = new ControlEvent()
                    {
                        ControlEventId = 4,
                        MaxMark = 5,
                        Date = "12.01",
                        Name = "lab 2"
                    }
                };            
            
            repository.Setup( r =>  r.GetControlEventAsync(It.IsAny<int>()))
                .Returns((int id) => await dataBase[id]);
            
            repository.Setup(r => r.UpdateAsync(It.IsAny<ControlEvent>()))
                .Callback((ControlEvent ce) => dataBase[ce.ControlEventId] = ce);

            return (repository, dataBase); */
        }
    }
    

}