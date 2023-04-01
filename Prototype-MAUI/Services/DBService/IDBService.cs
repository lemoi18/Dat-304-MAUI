using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.DBService
{
    public interface IDBService
    {
        Realm GetRealmInstance();
    }
}
