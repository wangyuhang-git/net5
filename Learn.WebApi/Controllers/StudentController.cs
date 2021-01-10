﻿using AutoMapper;
using Learn.Interface;
using Learn.Models.Business;
using Learn.Models.Common;
using Learn.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mongodb.Service;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.WebApi.Controllers
{
    [ApiController]
    [Route("api/Student")]
    public class StudentController : ControllerBase
    {
        //IStudent studentBusiness = new StudentBusiness();
        private static List<Student> Students = new List<Student>() {
            new Student(){ StudentId= Guid.NewGuid().ToString(), FirstName="Wang", LastName="Yuhang" , Birthday=Convert.ToDateTime("1989-10-28")},
            new Student(){ StudentId= Guid.NewGuid().ToString(), FirstName="Chen", LastName="Qiujin" , Birthday=Convert.ToDateTime("1989-09-03")},
            new Student(){ StudentId= Guid.NewGuid().ToString(), FirstName="Zhang", LastName="Yu" , Birthday=Convert.ToDateTime("1980-03-03")},
        };

        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly IStudent _student;

        public StudentController(ILogger<StudentController> Logger, IMapper Mapper, IStudent Student)
        {
            this._logger = Logger;
            this._mapper = Mapper;
            this._student = Student;
        }

        [HttpGet]
        public IActionResult Get(string firstName)
        {
            List<StudentDto> list = new List<StudentDto>();
            if (!string.IsNullOrEmpty(firstName))
            {
                list = _mapper.Map<List<StudentDto>>(Students.Where(c => c.FirstName == firstName));
            }
            else
            {
                list = _mapper.Map<List<StudentDto>>(Students);
            }
            return Ok(list);
        }
        [HttpPost]
        public IActionResult Post([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            StudentParam StudentParam = @object.ToObject<StudentParam>();
            //连接数据库
            MongoClient client = new MongoClient("mongodb://127.0.0.1");
            //获取database，没有则创建
            IMongoDatabase mydb = client.GetDatabase("mydb");
            //获取collection
            IMongoCollection<Student> collection = mydb.GetCollection<Student>("student");

            //插入一条数据
            StudentParam.Student.StudentId = Guid.NewGuid().ToString();
            collection.InsertOne(StudentParam.Student);

            return Ok();

            /*
            //查询添加的数据
            mydb = client.GetDatabase("mydb");
            //获取collection
            collection = mydb.GetCollection<Student>("student");

            var filter = Builders<Student>.Filter;
            var students= collection.Find(filter.Where(c=>c.FirstName!=""));
            List<StudentDto> list = _mapper.Map<List<StudentDto>>(students);
            return Ok(list);
            */
        }


        [HttpPost("AddMany")]
        public IActionResult AddMany([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            StudentList students = @object.ToObject<StudentList>();

            MongodbService<Learn.Models.Entity.Student> mongodbService = new MongodbService<Learn.Models.Entity.Student>("student");
            mongodbService.AddMany(students.Students);
            _logger.LogInformation($"增加了{students.Students.Count}条记录");
            return Ok();
        }

        [HttpPost("AddManyAsync")]
        public async Task AddManyAsync([FromBody] dynamic values)
        {
            JObject @object = JObject.Parse(values.ToString());
            StudentList students = @object.ToObject<StudentList>();
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(students));
            await _student.AddManyAsync(students.Students);
        }

        [HttpGet("GetStudent/{guid?}")]
        public IActionResult GetStudent(string guid = "")
        {
            MongoClient mongoClient = new MongoClient("mongodb://127.0.0.1");
            IMongoDatabase mydb = mongoClient.GetDatabase("mydb");
            IMongoCollection<Student> studentList = mydb.GetCollection<Student>("student");


            FindOptions<Student> options = new FindOptions<Student>();

            List<Student> list = studentList.FindAsync(
                Builders<Student>.Filter.Where(c => string.IsNullOrEmpty(guid) || c.StudentId == guid)
                , options).Result.ToList();

            //List<StudentDto> listDto = _mapper.Map<List<StudentDto>>(list);
            //this._logger.LogInformation($"数量：{listDto.Count}");
            //return Ok(listDto);

            return Ok(list);
        }

        [HttpGet("GetStudentsAsync")]
        public async Task<IEnumerable<Learn.Models.Entity.Student>> GetStudentsAsync([FromBody] dynamic studentSearch)
        {
            //MongodbService<Learn.Models.Entity.Student, StudentSearch> service = new MongodbService<Learn.Models.Entity.Student, StudentSearch>("student");
            //return await service.GetListAsync(c => string.IsNullOrEmpty(name) || c.FirstName == name);

            //Learn.Models.Business.StudentSearch search = new Learn.Models.Business.StudentSearch()
            //{
            //    Name = name,
            //    Birthday = birthday
            //};

            JObject @object = JObject.Parse(studentSearch.ToString());
            var search = @object.ToObject<StudentSearch>();
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(search));

            return await _student.GetStudentsAsync(search);
        }

        [HttpPost("GetPageStudentsAsync")]
        public async Task<BaseResultModel<Learn.Models.Entity.Student>> GetPageStudentsAsync([FromBody] dynamic studentSearch)
        {
            JObject @pageObject = JObject.Parse(studentSearch.ToString());
            var pageSearch = @pageObject.ToObject<StudentPageSearch>();
            _logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(pageSearch.StudentSearch));
            //Dictionary<string, string> sortDic = studentSearch.SortDic;
            Dictionary<string, string> sortDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(pageSearch.SortDic.ToString());
            return await _student.GetPageStudentsAsync(pageSearch.PageIndex, pageSearch.PageSize, sortDic, pageSearch.StudentSearch);
        }

        [HttpGet("GetStudents/{name?}")]
        public IEnumerable<Learn.Models.Entity.Student> GetStudents([FromServices] IStudent student, string name = "")
        {
            //return _student.GetStudents(new StudentSearch() { Name = name }); 
            return student.GetStudents(new StudentSearch() { Name = name });
        }
    }
}
