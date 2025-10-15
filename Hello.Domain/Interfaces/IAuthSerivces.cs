using Hello.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Domain.Interfaces
{
    public interface IAuthSerivces
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
