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
                FirstName = request.firstName,
                LastName = request.lastName,
                BirthDate = request.birthDate,
                Addr = request.addr,
                City = request.city,
                Country = request.country,
                Email = request.email,
                Phone = request.phone,
                Pswrd = request.pswrd,
                GDPRFlag = request.gdprFlag,
                SignInKey = request.signInKey,
                EmailVerify = request.emailVerify
            };

            return await _usersRepo.InsertAsync(item);
        }
    }
}
