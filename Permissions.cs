using OrchardCore.Security.Permissions;
using System.Collections.Generic;

namespace BCS.AdminSearch
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageAdminSearchSettings = new Permission("ManageAdminSearchSettings", "Manage Admin Search Settings");

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ManageAdminSearchSettings
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageAdminSearchSettings }
                }
            };
        }
    }
}
