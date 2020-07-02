using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Foo.Web.Api
{
    [Route("Api/User")]
    public class UserApi : BaseFooApi
    {
        [HttpGet("GetUsers")]
        public IEnumerable<dynamic> GetUsers([FromQuery]GetUsersArgs args)
        {
            var users = new List<User>();
            users.Add(new Teacher() { Username = "T1", Bar = "Bar"});
            users.Add(new Student() { Username = "S1", Foo = "Foo"});

            if (args == null || string.IsNullOrEmpty(args.UserType))
            {
                return users;
            }

            var query = users.AsEnumerable();
            query = query.Where(x => args.UserType == x.UserType);
            return query.ToList();
        }
    }

    public class GetUsersArgs
    {
        public string UserType { get; set; }
    }

    public class User
    {
        public User()
        {
            UserType = this.GetType().Name;
        }
        public string Username { get; set; }
        public string UserType { get; set; }
    }

    public class Student : User
    {
        public string Foo { get; set; }
    }
    public class Teacher : User
    {
        public string Bar { get; set; }
    }
}
