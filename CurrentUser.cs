using Uchebka.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uchebka
{
    public static class CurrentUser
    {
        public static Logins User { get; set; }
        public static string UserRole => User?.Employee?.Role?.NameRole;
    }
}
