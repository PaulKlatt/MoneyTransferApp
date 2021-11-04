
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserDao userDao;
        public UsersController(IUserDao _userDao)
        {
            userDao = _userDao;
        }

        [HttpGet]
        public ActionResult<List<UserInfo>> GetAllUsers()
        {
            List<UserInfo> allUsers = userDao.GetUserNameAndId();
            if(allUsers.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(allUsers);
            }
        }
    }
}
