using AdminManagementDSL.Application.Common.Interfaces;
using AdminManagementDSL.Application.User.Commands;
using AdminManagementDSL.Core.Entities;
using MediatR;

namespace AdminManagementDSL.Application.User.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUsersRepo _usersRepo;

        public CreateUserHandler(IUsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            //pre-checks and validations;

            var item = new Users
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Addr = request.Addr,
                City = request.City,
                Country = request.Country,
                Email = request.Email,
                Phone = request.Phone,
                Pswrd = request.Pswrd,
                GDPRFlag = request.GdprFlag,
                SignInKey = request.SignInKey,
                EmailVerify = request.EmailVerify
            };

            return await _usersRepo.InsertAsync(item);
        }
    }
}
