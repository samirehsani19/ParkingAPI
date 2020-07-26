using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Services
{
    public class UserRepository:Repository, IUserRepo
    {
        public UserRepository(DataContext context, ILogger<UserRepository> logger) : base(context, logger)
        {

        }
    }
}
