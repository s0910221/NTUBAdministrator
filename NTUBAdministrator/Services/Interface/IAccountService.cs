using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTUBAdministrator.Models;

namespace NTUBAdministrator.Services.Interface
{
    public interface IAccountService
    {
        UserAccount Login();
        UserAccount RegisterAndLogin();
    }
}
