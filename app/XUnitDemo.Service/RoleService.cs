using System;
using System.Threading.Tasks;
using XUnitDemo.IService;

namespace XUnitDemo.Service
{
    public class RoleService : IRoleService
    {

        public async Task<bool> AddRoleAsync(string roleName)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> ModifyRoleAsync(Guid roleId, string newName)
        {
            return await Task.FromResult(true);
        }
    }
}
