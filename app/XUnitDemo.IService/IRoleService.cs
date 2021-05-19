using System;
using System.Threading.Tasks;

namespace XUnitDemo.IService
{
    public interface IRoleService
    {
        public Task<bool> AddRoleAsync(string roleName);
        public Task<bool> ModifyRoleAsync(Guid roleId, string newName);
        public Task<bool> DeleteRoleAsync(Guid roleId);
    }
}
