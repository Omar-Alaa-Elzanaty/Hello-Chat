using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hello.Domain.Interfaces
{
    public interface IUserNameBloomServices
    {
        Task AddUserNameAsync(string userName);
        Task EnsureCreatedAsync();
        Task<bool> MightContainUserNameAsync(string userName);
    }
}
