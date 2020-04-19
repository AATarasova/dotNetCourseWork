using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using MailingGeneratorBll;
using MailingGeneratorBll.Addition;
using MailingGeneratorBll.Services;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using Moq;
using NUnit.Framework;
using Mailing = MailingsGeneratorDomain.Models.Mailing;

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
        public async Task UpdateControlEventAsync_UpdatesDataBase()
        {
            
            
            // AAA arrange act assert
            
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            await service.UpdateControlEventAsync(new UpdateControlEventModel()
            {
                Id = 4,
                MaxMark = 5,
                Date = "10.01"
            });
            
            //act
            await service.UpdateControlEventAsync(new UpdateControlEventModel()
            {
                Id = 3,
                MaxMark = 9,
                Date = "16.02"
            });

            //act
            await service.UpdateControlEventAsync(new UpdateControlEventModel()
            {
                Id = 5,
                MaxMark = 1,
                Date = "5.04"
            });

            
            // assert
           Assert.AreEqual(5, dataBase[4].MaxMark);
           Assert.AreEqual("16.02", dataBase[3].Date);
           Assert.AreEqual(1, dataBase[5].MaxMark);
           
        }        
        
        [Test, TestCaseSource("UpdateControlEventAsync_ThrowsExpected_Data")]
        public async Task UpdateControlEventAsync_ThrowsExpected(UpdateControlEventModel updateModel, 
            Type type, string message)
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
           var exception = Assert.ThrowsAsync(type, () => service.UpdateControlEventAsync(updateModel));
            
            // assert
           Assert.AreEqual(message, exception.Message);
           Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] UpdateControlEventAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { new UpdateControlEventModel(){Id = 6, MaxMark = 5, Date = "12.08"}, 
                typeof(ExceptionTypes.IdNotFoundException), "По такому идентификатору ничего не найдено."},
            
            new object[] { new UpdateControlEventModel(){Id = -3}, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},
            
            new object[] { null, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},
            
            new object[] { new UpdateControlEventModel(){Id = -1, MaxMark = 7}, 
                typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            
            new object[] { new UpdateControlEventModel(){Id = 3, MaxMark = -121}, 
                typeof(ExceptionTypes.IncorrectMarkException), "Оценка должна быть от 0 до 10!"},

            new object[] { new UpdateControlEventModel(){Id = 4, Date = "11111"}, 
                typeof(ExceptionTypes.IncorrectDateException), "Ожидается получить дату в формате dd.mm"}
            
        };

        
        
        
        [Test]
        public async Task GetControlEventAsync_ResurnsEventIfItExist()
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            // act
            var ce1 = await service.GetControlEventAsync(4);
            var ce3 = await service.GetControlEventAsync(3);
            var ce5 = await service.GetControlEventAsync(5);
            
            // assert
           Assert.AreEqual(4, ce1.ControlEventId);
           Assert.AreEqual("lab 2", (ce1.Name));
           Assert.AreEqual(5, ce5.ControlEventId);
           Assert.IsTrue(dataBase.ContainsKey(ce3.ControlEventId));
           
        }        
        
                
        [Test, TestCaseSource("GetControlEventAsync_ThrowsExpected_Data")]
        public async Task GetControlEventAsync_ThrowsExpected(int id, 
            Type type, string message)
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            var exception = Assert.ThrowsAsync(type, () => service.GetControlEventAsync(id));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }

        private static readonly object[] GetControlEventAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { 15, typeof(ExceptionTypes.ControlEventNotExistException), 
                "Не существует контрольного мероприятия с указанным id курса!"},
            new object[] { -1, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"}
        };
        
        
        
        
        [Test]
        public async Task CreateControlEventAsync_CreatesControlEventAndReturnsIt()
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            var ce = await service.CreateControlEventAsync(new ControlEvent()
            {
                ControlEventId = 1,
                MaxMark = 5,
                Date = "10.01",
                Name = "lab 1"
            });

            // assert
            // Assert.AreEqual("lab 2", ce.Name);
           Assert.IsTrue(dataBase.ContainsKey(1));
           Assert.IsTrue(dataBase.ContainsValue(ce));

            /*
            Assert.Pass();
            Assert.IsTrue();
            Assert.IsEmpty();
            Assert.AreEqual();
            Assert.Throws();
            */
        } 
        
        [Test, TestCaseSource("CreateControlEventAsync_ThrowsExpected_Data")]      
        public async Task CreateControlEventAsync_ThrowsExpected(ControlEvent controlEvent, 
            Type type, string message)
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            var exception = Assert.ThrowsAsync(type, () => service.CreateControlEventAsync(controlEvent));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] CreateControlEventAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { new ControlEvent(){Date = "12.08"}, 
                typeof(ExceptionTypes.IncorrectNameException), "Имя не должно быть пустым!"},
            new object[] { null, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},
            new object[] { new ControlEvent(){Name = "hello", Date = "11111"}, 
                typeof(ExceptionTypes.IncorrectDateException), "Ожидается получить дату в формате dd.mm"}
        };
        
        
        
        
        [Test]
        public async Task DeleteAsync_DeletesExistingControlEvent()
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            await service.DeleteAsync(12);

            //var ce = await service.GetControlEventAsync(4);
            
            // assert
            // Assert.AreEqual("lab 2", ce.Name);
           Assert.IsFalse(dataBase.ContainsKey(12));
           Assert.IsTrue(dataBase.ContainsKey(3));
           Assert.IsTrue(dataBase.ContainsKey(4));
           Assert.IsTrue(dataBase.ContainsKey(5));
        }
        
                
        [Test, TestCaseSource("DeleteAsync_ThrowsExpected_Data")]      
        public async Task DeleteAsync_ThrowsExpected(int id, 
            Type type, string message)
        {
            //arrange 
            var (mockRepository, dataBase) = GetMock();
            var service = new ControlEventService(mockRepository.Object);

            //act
            var exception = Assert.ThrowsAsync(type, () => service.DeleteAsync(id));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] DeleteAsync_ThrowsExpected_Data = new object[]
        {
            new object[] { -1, typeof(ExceptionTypes.IncorrectIdException), 
                "id должен быть не меньше нуля!"},
            new object[] { 99, typeof(ExceptionTypes.ControlEventNotExistException), 
                "Не существует контрольного мероприятия с указанным id курса!"}
        };
        
        
        
        

        private (Mock<IControlEventRepository> repository, Dictionary<int, ControlEvent> dataBase) GetMock()
        {
            var repository = new Mock<IControlEventRepository>(MockBehavior.Strict);
            var dataBase = new Dictionary<int, ControlEvent>()
                {
                    [4] = new ControlEvent()
                    {
                        ControlEventId = 4,
                        MaxMark = 2,
                        Date = "12.01",
                        Name = "lab 2"
                    },
                    [5] = new ControlEvent()
                    {
                        ControlEventId = 5,
                        MaxMark = 3,
                        Date = "15.01",
                        Name = "lab 5"
                    },
                    [3] = new ControlEvent()
                    {
                        ControlEventId = 3,
                        MaxMark = 1,
                        Date = "15.03",
                        Name = "lab 7"
                    },
                    [12] = new ControlEvent()
                    {
                        ControlEventId = 12,
                        MaxMark = 6,
                        Date = "16.06",
                        Name = "lab 6"
                    }
                };            
            
            repository.Setup( r =>  r.GetControlEventAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) =>
                      {
                          if (dataBase.ContainsKey(id))
                          {
                              return dataBase[id];
                          }

                          return null;
                      });

            repository.Setup(r => r.UpdateAsync(It.IsAny<UpdateControlEventModel>()))
                      .Returns((UpdateControlEventModel ce) =>
                          {
                              dataBase[ce.Id] = UpdateControlEventModelToControlEvent(ce);
                              return Task.CompletedTask;
                          });            
            repository.Setup(r => r.DeleteAsync(It.IsAny<int>()))
                      .Returns((int id) =>
                      {
                          dataBase.Remove(id);
                              return Task.CompletedTask;
                          });
            repository.Setup(r => r.ExistAsync(It.IsAny<int>())).ReturnsAsync((int id) => dataBase.ContainsKey(id));
            
            repository.Setup(r => r.CreateControlEventAsync(It.IsAny<ControlEvent>())).ReturnsAsync((ControlEvent ce) =>
                {
                    dataBase.Add(ce.ControlEventId, ce);
                    return ce;
                });
            
            return (repository, dataBase); 
        }

        private ControlEvent UpdateControlEventModelToControlEvent(UpdateControlEventModel model)
        {
            var ce = new ControlEvent();
            if (model.Date != null)
            {
                ce.Date = model.Date;
            }

            if (model.MaxMark.HasValue)
            {
                ce.MaxMark = model.MaxMark.Value;
            }
            ce.ControlEventId = model.Id;

            return ce;
        }
    }
    

}