﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Renci.SshNet.Messages.Authentication;
using WebService.DTO;
using WebService.Entities;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DBContext DBContext;

        public UserController( DBContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            var List = await DBContext.User.Select(
                s => new UserDTO
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Username = s.Username,
                    Password = s.Password,
                    EnrollmentDate = s.EnrollmentDate
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int Id)
        {
            UserDTO User = await DBContext.User.Select(
                    s => new UserDTO
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Username = s.Username,
                        Password = s.Password,
                        EnrollmentDate = s.EnrollmentDate
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpPost("InsertUser")]
        public async Task<HttpStatusCode> InsertUser(UserDTO User)
        {
            var entity = new User()
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Username = User.Username,
                Password = User.Password,
                EnrollmentDate = User.EnrollmentDate
            };

            DBContext.User.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut ("UpdateUser")]
        public async Task<HttpStatusCode> UpdateUser(UserDTO User)
        {
            var entity = await DBContext.User.FirstOrDefaultAsync(s => s.Id == User.Id);

            entity.FirstName = User.FirstName;
            entity.LastName = User.LastName;
            entity.Username = User.Username;
            entity.Password = User.Password;
            entity.EnrollmentDate = User.EnrollmentDate;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteUser/{Id}")]
        public async Task<HttpStatusCode> DeleteUser(int Id)
        {
            var entity = new User()
            {
                Id = Id
            };
            DBContext.User.Attach(entity);
            DBContext.User.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
