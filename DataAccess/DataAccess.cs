using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using System.Runtime.Remoting.Contexts;

namespace DataAccessLayer
{
    public class DataAccess
    {
        
        DataClasses1DataContext context = new DataClasses1DataContext(ConfigurationManager.ConnectionStrings["AuthContext"].ToString());

        
    }


}
