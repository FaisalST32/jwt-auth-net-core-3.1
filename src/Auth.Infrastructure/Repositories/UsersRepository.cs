using Auth.Common.Data;
using AutoMapper;
using Auth.Application.Repositories;
using Auth.Domain.Exceptions;
using Auth.Domain.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UsersRepository(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        public Task<User> GetUserByCredentials(string username, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var userFound = await _uow.Context.Set<Entities.User>().AsNoTracking()
                .Include(user => user.Roles)
                .ThenInclude(role => role.Role)
                .Include(user => user.UserProfile)
                .SingleOrDefaultAsync(user => user.Email.ToLower() == username.ToLower());

            if (userFound == null)
            {
                return null;
            }

            return _mapper.Map<User>(userFound);
        }
        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var userFound = await  _uow.Context.Set<Entities.User>().AsNoTracking()
                .Include(user => user.Roles)
                .ThenInclude(role => role.Role)
                .Include(user => user.UserProfile)
                .SingleOrDefaultAsync(user => user.RefreshToken == refreshToken);

            if (userFound == null)
            {
                return null;
            }

            return _mapper.Map<User>(userFound);
        }

        public Task<UserProfile> GetUserProfile(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> AddUser(User user)
        {
            var newUser = _mapper.Map<Entities.User>(user);
            newUser.DateCreatedUTC = DateTime.UtcNow;
            await  _uow.Context.Set<Entities.User>().AddAsync(newUser);
            await  _uow.SaveChangesAsync();
            return _mapper.Map<UserProfile>(newUser.UserProfile);
        }


        public async Task<UserProfile> UpdateUser(int userId, User userToUpdate)
        {
            var userFound = await  _uow.Context.Set<Entities.User>().AsNoTracking().Include(user => user.Roles).SingleOrDefaultAsync(user => user.Id == userToUpdate.Id);
            if (userFound == null)
            {
                throw new NotFoundException($"cannot find user with userId {userId}");
            }

            var updatedUser = _mapper.Map<Entities.User>(userToUpdate);

            updatedUser = ClearAnyDuplicateProperties(updatedUser, userFound);

            updatedUser.DateModifiedUTC = DateTime.UtcNow;
             _uow.Context.Set<Entities.User>().Update(updatedUser);
             _uow.Context.Entry(updatedUser).Property(x => x.DateCreatedUTC).IsModified = false;
             _uow.Context.Entry(updatedUser).Property(x => x.IsDeleted).IsModified = false;

            await  _uow.SaveChangesAsync();
            return _mapper.Map<UserProfile>(updatedUser.UserProfile);
        }

        private Entities.User ClearAnyDuplicateProperties(Entities.User newUser, Entities.User existingUser)
        {
            var currentRoles = existingUser?.Roles;
            var newRoles = newUser?.Roles;

            var rolesAdded = newRoles?.Where(role => !currentRoles.Any(currRole => currRole.RoleId == role.RoleId));
            newUser.Roles = rolesAdded.ToList();
            return newUser;
        }
    }
}
