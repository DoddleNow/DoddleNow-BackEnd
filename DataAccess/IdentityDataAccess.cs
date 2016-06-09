using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DataAccessLayer
{
    public class IdentityDataAccess
    {
        AppIdentityDataContext context = new AppIdentityDataContext(ConfigurationManager.ConnectionStrings["DoddleNowConnectionString"].ToString());

        public List<AspNetRole> GetRoles()
        {
            var items = from roles in context.AspNetRoles select roles;
            return items.ToList();
        }

        public List<usp_GetUsersBySchoolRoleResult> GetUsersBySchoolRole(int schoolId, int? roleId)
        {
            return context.usp_GetUsersBySchoolRole(schoolId, roleId).ToList();
        }
        
    }




}
