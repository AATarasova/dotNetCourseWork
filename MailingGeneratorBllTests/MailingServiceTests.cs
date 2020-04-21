using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using MailingGeneratorBll.Addition;
using MailingGeneratorBll.Services;
using MailingGeneratorDal.Repository;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using MailingGeneratorDomain.Services;
using Moq;
using NUnit.Framework;
using Mailing = MailingsGeneratorDomain.Models.Mailing;

namespace MailingGeneratorBllTests
{
    public class MailingServiceTests
    {
        private Mock<IMailingRepository> _mRepository;
        private Mock<IControlEventRepository> _ceRepository;
        private Mock<ITextRepository> _tRepository;
        private Dictionary<int, MailingsGeneratorDomain.Models.Mailing> _mailingsDb;
        private MailingService _service;
        private static List<int> _incorrectId = new List<int>();
        public static int newTextCount = 1;
        [SetUp]
        public void Setup()
        {
            // перед запуском тестов
            var (mockMRepository, mockCeRepository, mockTRepository, dataBase) = GetMock();
            _mRepository  = mockMRepository;
            _ceRepository = mockCeRepository;
            _tRepository  = mockTRepository;
            _mailingsDb   = dataBase;
            _service = new MailingService(_mRepository.Object, _ceRepository.Object, _tRepository.Object);
            _incorrectId.Add(1);
            _incorrectId.Add(10);

        }

        
        // Olga, Change
        [Test]
        public async Task UpdateMailingAsync_UpdatesDataBase()
        {
            //arrange 
            var worksId = new List<int>();
            worksId.Add(5);

            var oldWorkCount = _mailingsDb[1].Works.Count;
            var oldTextCount = _mailingsDb[1].MailText.Count;
            
            var model = new UpdateMailingModel()
            {
                MailingId = 1,
                StartDate = "10.01",
                WorksId   = worksId 
            };
            
            //act
            await _service.UpdateAsync(model);
            
            // assert
           Assert.AreEqual("10.01", _mailingsDb[1].StartDate);
           Assert.IsTrue(_mailingsDb[1].Works.Count > oldWorkCount);
           Assert.IsTrue(_mailingsDb[1].MailText.Count > oldTextCount);
        }        
        
        [Test, TestCaseSource("UpdateAsync_ThrowsExpected_Data")]
        public async Task UpdateControlEventAsync_ThrowsExpected(UpdateMailingModel updateModel, 
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
            new object[] { new UpdateMailingModel() {MailingId = 6, StartDate = "12.08"}, 
                typeof(ExceptionTypes.MailingNotExistException), "Не существует рассылки с указанным id курса!"},
            
            new object[] {  new UpdateMailingModel() {MailingId = 1, StartDate = "12.08", TextId = _incorrectId}, 
                typeof(ExceptionTypes.TextNotExistException), "Не существует текста с указанным id курса!"},
            
            new object[] { new UpdateMailingModel()  {MailingId = 2, StartDate = "12.08", WorksId = _incorrectId}, 
                typeof(ExceptionTypes.ControlEventNotExistException), "Не существует контрольного мероприятия с указанным id курса!"},
            
            new object[] { null, typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},

            new object[] {new UpdateMailingModel()  {MailingId = -2, StartDate = "12.08", WorksId = _incorrectId}, 
                typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},

            new object[] { new UpdateMailingModel()  {MailingId = 2, StartDate = "1208", WorksId = _incorrectId}, 
                typeof(ExceptionTypes.IncorrectDateException), "Ожидается получить дату в формате dd.mm"}
            
        };
        
        
        
         // Olga, Change      
        [Test]
        public async Task CreateMailingAsync_CreatesMailingAndReturnsIt()
        {
            //arrange 
            var worksId = new List<int>();
            worksId.Add(3);
            
            var model = new MailingsGeneratorDomain.Models.Mailing()
            {
                StartDate = "10.01",
                WorksId   = worksId, 
                FinishWorkId = 3,
                MailingId    = 14,
                CourseName   = "course1"
            };
            
            //act
            var m = await _service.CreateMailingAsync(model);

            // assert
            // Assert.AreEqual("lab 2", ce.Name);
           Assert.IsTrue( _mailingsDb.ContainsKey(m.MailingId));
           Assert.IsTrue(_mailingsDb[m.MailingId].MailText.Count >= _mailingsDb[m.MailingId].Works.Count);
           Assert.IsTrue(_mailingsDb[m.MailingId].Works.Count == worksId.Count);
           Assert.IsTrue( _mailingsDb.ContainsValue(m));
           } 
        
        [Test, TestCaseSource("CreateMailingAsync_ThrowsExpected_Data")]      
        public async Task CreateControlEventAsync_ThrowsExpected(MailingsGeneratorDomain.Models.Mailing mailing, 
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.CreateMailingAsync(mailing));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] CreateMailingAsync_ThrowsExpected_Data = new object[]
        {  
            
            new object[] { null, typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},

            new object[] { new MailingsGeneratorDomain.Models.Mailing() {MailingId = 6, StartDate = "12.08"}, 
                typeof(ExceptionTypes.IncorrectNameException), "Имя не должно быть пустым!"},

            new object[] { new MailingsGeneratorDomain.Models.Mailing()  {MailingId = 8, StartDate = "1208", 
                    CourseName = "course", WorksId = _incorrectId}, 
                typeof(ExceptionTypes.IncorrectDateException), "Ожидается получить дату в формате dd.mm"},
            
            new object[] {  new UpdateMailingModel() {MailingId = 1, CourseName = "course",StartDate = "12.08", TextId = _incorrectId}, 
                typeof(ExceptionTypes.TextNotExistException), "Не существует текста с указанным id курса!"},
            
            new object[] { new UpdateMailingModel()  {MailingId = 3, CourseName = "course", StartDate = "12.08", WorksId = _incorrectId}, 
                typeof(ExceptionTypes.ControlEventNotExistException), "Не существует контрольного мероприятия с указанным id курса!"},
            
            new object[] { new UpdateMailingModel()  {MailingId = 9, CourseName = "course",StartDate = "12.08", FinishWorkId = 77}, 
                typeof(ExceptionTypes.ControlEventNotExistException), "Не существует контрольного мероприятия с указанным id курса!"},
            
        };
        
        
        
        [Test]
        public async Task GetCourseAsync_CreatesMailingAndReturnsIt()
        {
            //arrange 
            var model = new GetMailingModel()
            {
                Name   = "probability theory"
            };

           //act
           var mail1 = await _service.GetCourseAsync(1);
           var mail2 = await _service.GetCourseAsync(model);

            // assert
           Assert.IsTrue( _mailingsDb.ContainsValue(mail1));
           Assert.IsTrue( _mailingsDb.ContainsKey(mail1.MailingId));
           Assert.IsTrue( _mailingsDb.ContainsValue(mail2));
           Assert.IsTrue( _mailingsDb.ContainsKey(mail2.MailingId));
           } 
        
        [Test, TestCaseSource("GetCourseAsync_ThrowsExpected_Data")]      
        public async Task GetCourseAsync_ThrowsExpected(GetMailingModel getModel, 
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.GetCourseAsync(getModel));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }

        
        [Test, TestCaseSource("GetCourseAsyncById_ThrowsExpected_Data")]      
        public async Task GetCourseAsyncDyId_ThrowsExpected(int id, 
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.GetCourseAsync(id));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] GetCourseAsync_ThrowsExpected_Data = new object[]
        {
            new object[] {new GetMailingModel() {Id = -1},
                typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            
            new object[] { new GetMailingModel() {}, 
                typeof(ExceptionTypes.NullValueException), "Пустой запрос!"},

        };
        private static readonly object[] GetCourseAsyncById_ThrowsExpected_Data = new object[]
        {
            new object[] { -1, 
                typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
        };
        
        
        
               
        [Test]
        public async Task DeleteMailingAsync_CreatesMailingAndReturnsIt()
        {
            //arrange 
            
            //act
            await _service.DeleteMailingAsync(1);

            // assert
            // Assert.AreEqual("lab 2", ce.Name);
           Assert.IsFalse( _mailingsDb.ContainsKey(1));
           } 
        
        [Test, TestCaseSource("DeleteMailingAsync_ThrowsExpected_Data")]      
        public async Task DeleteMailingAsync_ThrowsExpected(int id, 
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.DeleteMailingAsync(id));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }


        private static readonly object[] DeleteMailingAsync_ThrowsExpected_Data = new object[]
        {  
            
            new object[] { 0, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            new object[] { -10, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"}
        };
        
        
        
        // Olga:
        [Test]
        public async Task HasWorkByIdAsync_ReturnsTrueIfItIsWorkByIdInMailingById()
        {
            //arrange 
            
            //act
            var has = await _service.HasWorkByIdAsync(1, 3);
            var hasnot = await _service.HasWorkByIdAsync(1, 4);

            // assert
               Assert.IsFalse( hasnot);
               Assert.IsTrue( has);
           } 
        // Olga:
        [Test, TestCaseSource("HasWorkByIdAsync_ThrowsExpected_Data")]      
        public async Task HasWorkByIdAsync_ThrowsExpected(int mailingId, int workId, 
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.HasWorkByIdAsync(mailingId, workId));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }
        // Olga:
        private static readonly object[] HasWorkByIdAsync_ThrowsExpected_Data = new object[]
        {  
            
            new object[] { 0, 3, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            new object[] { 3, 0, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            new object[] { 16, 1, typeof(ExceptionTypes.MailingNotExistException),
                "Не существует рассылки с указанным id курса!"},
            new object[] { 1, 78, typeof(ExceptionTypes.ControlEventNotExistException), 
                "Не существует контрольного мероприятия с указанным id курса!"}
        };
          
        
        
        // Olga:
        [Test]
        public async Task HasTextByIdAsync_ReturnsTrueIfItIsTextByIdInMailingById()
        {
            //arrange 
            
            //act
            var has = await _service.HasTextByIdAsync(1, 3);
            var hasnot = await _service.HasTextByIdAsync(1, 1);

            // assert
               Assert.IsFalse( hasnot);
               Assert.IsTrue( has);
           } 
        // Olga:
        [Test, TestCaseSource("HasTextByIdAsync_ThrowsExpected_Data")]      
        public async Task HasTextByIdAsync_ThrowsExpected(int mailingId, int textId, 
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.HasTextByIdAsync(mailingId, textId));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }
        // Olga:
        private static readonly object[] HasTextByIdAsync_ThrowsExpected_Data = new object[]
        {  
            
            new object[] { 0, 3, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            new object[] { 3, 0, typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            new object[] { 16, 1, typeof(ExceptionTypes.MailingNotExistException),
                "Не существует рассылки с указанным id курса!"},
            new object[] { 1, 78, typeof(ExceptionTypes.TextNotExistException), 
                "Не существует текста с указанным id курса!"}
        };        
        
        // Olga:
        [Test]
        public async Task GetCommonNumberOfControlEventsAsync_ReturnsCountOfControlEventInMailing()
        {
            //arrange 
            
            //act
            var count = await _service.GetCommonNumberOfControlEventsAsync(4);

            // assert
               Assert.IsTrue( count == 2);
           } 
        // Olga:
        [Test, TestCaseSource("GetCommonNumberOfControlEventsAsync_ThrowsExpected_Data")]      
        public async Task GetCommonNumberOfControlEventsAsync_ThrowsExpected(int mailingId,  
            Type type, string message)
        {
            //arrange 
            //act
            var exception = Assert.ThrowsAsync(type, () => _service.GetCommonNumberOfControlEventsAsync(mailingId));
            
            // assert
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(exception.GetType(), type);
           
        }
        // Olga:
        private static readonly object[] GetCommonNumberOfControlEventsAsync_ThrowsExpected_Data = new object[]
        {  
            
            new object[] { 0,  typeof(ExceptionTypes.IncorrectIdException), "id должен быть не меньше нуля!"},
            new object[] { 16, typeof(ExceptionTypes.MailingNotExistException),
                "Не существует рассылки с указанным id курса!"},
        };
        
        

        
        
        
        
        
        
        
        
        
        
        
        
        
        private (Mock<IMailingRepository> repository, Mock<IControlEventRepository> ceRepository, 
        Mock<ITextRepository> tRepository,
        Dictionary<int, MailingsGeneratorDomain.Models.Mailing> dataBase) GetMock()
                {
                    var controlEventRepository = new Mock<IControlEventRepository>(MockBehavior.Strict);
                    var textRepository = new Mock<ITextRepository>(MockBehavior.Strict);
                    var repository = new Mock<IMailingRepository>(MockBehavior.Strict);
                    var controlEventsDb = CreateControlEventDataBase();
                    var textsDb                = CreateTextDataBase();
                    var mailingsDb           = CreateMailingDataBase(textsDb, controlEventsDb);
                    
                    // Anna 21.04:
                    repository.Setup(r => r.HasWorkByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                              .ReturnsAsync((int mailingId, int workId) =>
                              {
                                  foreach (var work in _mailingsDb[mailingId].Works)
                                  {
                                      if (work.ControlEventId == workId)
                                      {
                                          return true;
                                      }
                                  }
                                  return false;
                              });
                    
                    // Anna 21.04:
                    controlEventRepository.Setup(r => r.ExistAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => controlEventsDb.ContainsKey(id));
                    
                    // Anna 21.04:
                    repository.Setup(r => r.HasTextByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                              .ReturnsAsync((int mailingId, int textId) =>
                              {
                                  foreach (var text in _mailingsDb[mailingId].MailText)
                                  {
                                      if (text.TextId == textId)
                                      {
                                          return true;
                                      }
                                  }
                                  return false;
                              });
                    
                    // Anna 21.04:
                    textRepository.Setup(r => r.ExistAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => textsDb.ContainsKey(id));
                    
                    // Anna 21.04:
                    textRepository.Setup(r => r.CreateTextAsync(It.IsAny<Text>())).ReturnsAsync((Text t) =>
                    {
                        textsDb.Add(newTextCount++ + 4, t);
                        return t;
                    });
                    
                    // Anna 21.04:
                    repository.Setup(r => r.GetCommonNumberOfControlEventsAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) =>
                        {
                            var mailing = mailingsDb[id];
                            return mailing.FinishWork == null ? mailing.Works.Count : mailing.Works.Count + 1;
                        });
                    
                    
                    repository.Setup( r =>  r.GetCourseAsync(It.IsAny<int>()))
                              .ReturnsAsync((int id) =>
                              {
                                  if (mailingsDb.ContainsKey(id))
                                  {
                                      return mailingsDb[id];
                                  }
        
                                  return null;
                              });
                    
                    repository.Setup( r =>  r.GetCourseAsync(It.IsAny<GetMailingModel>()))
                              .ReturnsAsync((GetMailingModel model) =>
                              {
                                  if (model.Id.HasValue && mailingsDb.ContainsKey(model.Id.Value))
                                  {
                                      return mailingsDb[model.Id.Value];
                                  }

                                  foreach (var data in mailingsDb)
                                  {
                                      if (data.Value.CourseName == model.Name)
                                      {
                                          return data.Value;
                                      }
                                  }
                                  return null;
                              });
        
                    repository.Setup(r => r.UpdateAsync(It.IsAny<UpdateMailingModel>()))
                              .Returns((UpdateMailingModel ce) =>
                                  {
                                      mailingsDb[ce.MailingId] =  CreateUpdated(mailingsDb[ce.MailingId], ce);
                                      return Task.CompletedTask;
                                  });        
                    
                    repository.Setup(r => r.DeleteMailingAsync(It.IsAny<int>()))
                              .Returns((int id) =>
                              {
                                  mailingsDb.Remove(id);
                                  return Task.CompletedTask;
                              });
                    
                    repository.Setup(r => r.ExistAsync(It.IsAny<int>())).ReturnsAsync((int id) => mailingsDb.ContainsKey(id));
                    
                    
                    repository.Setup(r => r.CreateMailingAsync(
                        It.IsAny<MailingsGeneratorDomain.Models.Mailing>())).ReturnsAsync((MailingsGeneratorDomain.Models.Mailing m) =>
                            {
                                mailingsDb.Add(m.MailingId, m);
                                return m;
                            });
                    
                    
                    textRepository.Setup( r =>  r.GetTextsAsyncById(It.IsAny<List<int>>()))
                        .ReturnsAsync(( List<int>idList) =>
                        {
                            List<Text> texts = new List<Text>();
                            foreach (var id in idList)
                            {
                                if (textsDb.ContainsKey(id))
                                {
                                    texts.Add(textsDb[id]);
                                }
                            }

                            return texts;
                        });   
                    
                    controlEventRepository.Setup( r =>  r.GetControlEventsAsyncById(It.IsAny<List<int>>()))
                        .ReturnsAsync(( List<int>idList) =>
                        {
                            List<ControlEvent> controlEvents = new List<ControlEvent>();
                            foreach (var id in idList)
                            {
                                if (controlEventsDb.ContainsKey(id))
                                {
                                    controlEvents.Add(controlEventsDb[id]);
                                }
                            }

                            return controlEvents;
                        });
                    
                    controlEventRepository.Setup( r =>  r.GetControlEventAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) =>
                        {
                            if (controlEventsDb.ContainsKey(id))
                            {
                                return controlEventsDb[id];
                            }

                            return null;
                        });
                    
                    return (repository, controlEventRepository, textRepository, mailingsDb); 
                }

        private Dictionary<int, ControlEvent> CreateControlEventDataBase()
                {
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
                    return dataBase;
                }
                
        private Dictionary<int, Text> CreateTextDataBase()
                {
                    var dataBase = new Dictionary<int, Text>()
                    {
                        [1] = new Text()
                        {
                            TextId = 1,
                            InfomationPart = "info 1"
                        },
                        [2] = new Text()
                        {
                            TextId = 2,
                            InfomationPart = "info 2",
                            RememberAboutDeadline = "deadline tomorrow"
                        },
                        [3] = new Text()
                        {
                            TextId = 3,
                            InfomationPart = "info 3"
                        },
                    };
                    return dataBase;
                }
                
        private Dictionary<int, MailingsGeneratorDomain.Models.Mailing> CreateMailingDataBase(Dictionary<int, Text>tDb,
            Dictionary<int, ControlEvent> ceDb)
        {
            var a = new MailingsGeneratorDomain.Models.Mailing()
            {
                MailingId = 1,
                StartDate = "12.01",
                CourseName = "probability theory",
                HelloText = "hello",
                Works = new List<ControlEvent>(),
                MailText = new List<Text>()
            };
            a.Works.Add(ceDb[3]);
            a.MailText.Add(tDb[3]);
            
            var d = new MailingsGeneratorDomain.Models.Mailing()
            {
                MailingId = 4,
                StartDate = "14.04",
                CourseName = "dot net",
                HelloText = "hello",
                Works = new List<ControlEvent>(),
                MailText = new List<Text>(),
                FinishWork = ceDb[5]
            };
            d.Works.Add(ceDb[4]);
            d.MailText.Add(tDb[3]);
            
                    var dataBase = new Dictionary<int, MailingsGeneratorDomain.Models.Mailing>()
                    {
                        [1] = a,
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
                        }, 
                        [4] = d
                    };

                    return dataBase;
                }


        private MailingsGeneratorDomain.Models.Mailing CreateUpdated(MailingsGeneratorDomain.Models.Mailing mailing,
            UpdateMailingModel model)
        {
            if (model.CourseName!= null)
            {
                mailing.CourseName = model.CourseName;
            }
            
            if (model.FinishId.HasValue)
            {
                mailing.FinishWork = model.FinishWork;
            }

            if (model.HelloText != null)
            {
                mailing.HelloText = model.HelloText;
            }

            if (model.StartDate != null)
            {
                mailing.StartDate = model.StartDate;
            }
            
            if (model.Works!= null)
            {
                foreach (var work in model.Works)
                {
                    mailing.Works.Add(work);
                }
            }
            if (model.MailText!= null)
            {
                foreach (var text in model.MailText)
                {
                    mailing.MailText.Add(text);
                }
            }
            return mailing;
        }
    }
}