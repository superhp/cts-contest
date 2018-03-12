using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CtsContestWeb.Db.Repository;
using CtsContestWeb.Db.Entities;
using CtsContestWeb.Dto;

namespace CtsContestWeb.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private readonly IContactInfoRepository _contactInfoRepository;

        public QuizController(IContactInfoRepository contactInfoRepository)
        {
            _contactInfoRepository = contactInfoRepository;
        }

        [HttpPost("[action]")]
        public OkResult AddContact([FromBody] ContactInfoDto contactInfoDto)
        {
            var contactInfoEntity = new ContactInfo
            {
                Email = contactInfoDto.Email,
                Surname = contactInfoDto.Surname,
                Name = contactInfoDto.Name,
                Phone = contactInfoDto.Phone,
                Answer = contactInfoDto.Answer,
                CourseName = contactInfoDto.CourseName,
                CourseNumber = contactInfoDto.CourseNumber,
                Degree = contactInfoDto.Degree,
                Created = DateTime.Now
            };
            _contactInfoRepository.InsertIfNotExists(contactInfoEntity);

            return Ok();
        }
    }
}