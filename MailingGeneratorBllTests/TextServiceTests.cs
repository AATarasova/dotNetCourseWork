using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailingGeneratorBll.Addition;
using MailingGeneratorBll.Services;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using Moq;
using NUnit.Framework;

namespace MailingGeneratorBllTests
{
    public class TextServiceTests
    {
        
        private Mock<IMailingRepository> _mRepository;
        private Mock<ITextRepository> _mockRepository;
        private Dictionary<int, Text> _textDb;
        private TextService _service;
        
        [SetUp]
        public void Setup()
        {
            // перед запуском тестов
            var (mockRepository, mRepository, dataBase) = GetMock();
            _service        = new TextService(mockRepository.Object, mRepository.Object);
            _mRepository    = mRepository;
            _mockRepository = mockRepository;
            _textDb         = dataBase;
        }

        [Test]
        public async Task UpdateAsync_UpdatesDataBase()
        {
            //arrange 

            //act
            await _service.UpdateAsync(new UpdateTextModel()
            {
                Id = 2,
                InfomationPart = "test tomorrow",
                MailingId = 1
            });
            
            // assert
           Assert.AreEqual("test tomorrow", _textDb[2].InfomationPart);
           
        }        
        
        [Test, TestCaseSource("UpdateAsync_ThrowsExpected_Data")]
        public async Task UpdateAsync_ThrowsExpected(UpdateTextModel updateModel, 
            Type type, string message)
        {
            //arrange 

            //act
           var exception = Assert.ThrowsAsync(type, () => _service.UpdateAsync(updateModel));
            
            // assert
           Assert.AreEqual(message, exception.Message);
           Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] UpdateAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { new UpdateTextModel() {Id = 6, InfomationPart = "no"}, 
                typeof(ExceptionTypes.TextNotExistException), "Не существует текста с указанным id курса!"},
            
            new object[] { new UpdateTextModel() {Id = -3}, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},
            
            new object[] { null, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},
            
            new object[] { new UpdateTextModel() {Id = -1, InfomationPart = "hello"}, 
                typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            
            new object[] { new UpdateTextModel() {Id = 3, InfomationPart = "hi", MailingId = 98}, 
                typeof(ExceptionTypes.MailingNotExistException), "Не существует рассылки с указанным id курса!"},
        };

        
        
        
        [Test]
        public async Task GetTexttAsync_ResurnsEventIfItExist()
        {
            //arrange 

            // act
            var m = await _service.GetTextAsync(2);
            
            // assert
           Assert.IsTrue(_textDb.ContainsValue(m));
           Assert.AreEqual(2, m.TextId);
           
        }        
        
                
        [Test, TestCaseSource("GetTextAsync_ThrowsExpected_Data")]
        public async Task GetTextAsync_ThrowsExpected(int id, 
            Type type, string message)
        {
            //arrange 

            //act
            var exception = Assert.ThrowsAsync(type, () => _service.GetTextAsync(id));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }

        private static readonly object[] GetTextAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { 15, typeof(ExceptionTypes.TextNotExistException), 
                "Не существует текста с указанным id курса!"},
            new object[] { -1, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"}
        };
        
        
        
        
        [Test]
        public async Task CreateTextAsync_CreatesControlEventAndReturnsIt()
        {
            //arrange 

            //act
            var t = await _service.CreateTextAsync(new Text()
            {
                TextId = 6,
                InfomationPart = "info",
                MailId = 2
            });

            // assert
           Assert.IsTrue(_textDb.ContainsKey(t.TextId));
           Assert.IsTrue(t.Mail != null);

        } 
        
        [Test, TestCaseSource("CreateTextAsync_ThrowsExpected_Data")]      
        public async Task CreateTextAsync_ThrowsExpected(Text text, 
            Type type, string message)
        {
            //arrange 

            //act
            var exception = Assert.ThrowsAsync(type, () => _service.CreateTextAsync(text));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] CreateTextAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { new Text(){TextId = 5}, 
                typeof(ExceptionTypes.IncorrectNameException), "Имя не должно быть пустым!"},
            new object[] { null, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},
            new object[] { new Text(){TextId = 6, InfomationPart = "11111", MailId = 47}, 
                typeof(ExceptionTypes.MailingNotExistException), "Не существует рассылки с указанным id курса!"}
        };
        
        
        
        
        [Test]
        public async Task DeleteAsync_DeletesExistingControlEvent()
        {
            //arrange 

            //act
            await _service.DeleteTextAsync(3);

            // assert
           Assert.IsFalse(_textDb.ContainsKey(3));
           Assert.IsTrue(_textDb.ContainsKey(1));
           Assert.IsTrue(_textDb.ContainsKey(2));
        }
        
                
        [Test, TestCaseSource("DeleteTextAsync_ThrowsExpected_Data")]      
        public async Task DeleteTextAsync_ThrowsExpected(int id, 
            Type type, string message)
        {
            //arrange 

            //act
            var exception = Assert.ThrowsAsync(type, () => _service.DeleteTextAsync(id));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] DeleteTextAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { -1, typeof(ExceptionTypes.IncorrectIdException), 
                "id должен быть не меньше нуля!"},
            new object[] { 99, typeof(ExceptionTypes.TextNotExistException), 
                "Не существует текста с указанным id курса!"}
        };
        
        
        
        

        private (Mock<ITextRepository> repository, Mock<IMailingRepository> textRepository, Dictionary<int, Text> dataBase) GetMock()
        {
            var repository = new Mock<ITextRepository>(MockBehavior.Strict);
            var mailRepository = new Mock<IMailingRepository>(MockBehavior.Strict);
            var dataBase = new Dictionary<int, Text>()
                {
                    [1] = new Text()
                    {
                        TextId = 1,
                        InfomationPart = "text 1"
                    },
                    [2] = new Text()
                    {
                        TextId = 2,
                        InfomationPart = "text 2"
                    },
                    [3] = new Text()
                    {
                        TextId = 3,
                        InfomationPart = "text 3"
                    },
                };
            var mailingDb = CreateMailingDataBase();
            
            repository.Setup( r =>  r.GetTextAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) =>
                      {
                          if (dataBase.ContainsKey(id))
                          {
                              return dataBase[id];
                          }

                          return null;
                      });

            repository.Setup(r => r.UpdateAsync(It.IsAny<UpdateTextModel>()))
                      .Returns((UpdateTextModel t) =>
                      {
                          dataBase[t.Id].InfomationPart = t.InfomationPart;
                          return Task.CompletedTask;
                          });            
            
            repository.Setup(r => r.DeleteTextAsync(It.IsAny<int>()))
                      .Returns((int id) =>
                      {
                          dataBase.Remove(id);
                              return Task.CompletedTask;
                          });
            repository.Setup(r => r.ExistAsync(It.IsAny<int>())).ReturnsAsync((int id) => dataBase.ContainsKey(id));
            
            repository.Setup(r => r.CreateTextAsync(It.IsAny<Text>())).ReturnsAsync((Text t) =>
                {
                    dataBase.Add(t.TextId, t);
                    return t;
                });
            mailRepository.Setup( r =>  r.GetCourseAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    if (mailingDb.ContainsKey(id))
                    {
                        return mailingDb[id];
                    }
        
                    return null;
                });
            
            mailRepository.Setup(r => r.ExistAsync(It.IsAny<int>())).ReturnsAsync((int id) => mailingDb.ContainsKey(id));
            
            return (repository, mailRepository, dataBase); 
        }
        private Dictionary<int, MailingsGeneratorDomain.Models.Mailing> CreateMailingDataBase()
        {
            var dataBase = new Dictionary<int, MailingsGeneratorDomain.Models.Mailing>()
            {
                [1] = new MailingsGeneratorDomain.Models.Mailing()
                {
                    MailingId = 1,
                    StartDate = "12.01",
                    CourseName = "probability theory",
                    HelloText = "hello"
                },
                [2] = new MailingsGeneratorDomain.Models.Mailing()
                {
                    MailingId = 2,
                    StartDate = "02.02",
                    CourseName = "big data",
                },
                [3] = new MailingsGeneratorDomain.Models.Mailing()
                {
                    MailingId = 3,
                    StartDate = "3.03",
                    CourseName = "data management"
                }
            };
            return dataBase;
        }

    }
}