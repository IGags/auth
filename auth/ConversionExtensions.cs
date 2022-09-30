using System;
using Api.Models;
using DAL.Models;

namespace Api
{
	public static class ConversionExtensions
	{
        public static UserDal RegisterDtoToUserDal(this RegisterRequest request)
        {
            return new UserDal()
            {
                Email = request.Email,
                Password = request.Password,
                FIO = request.FIO,
                LastLogin = null,
                Phone = request.Phone
            };
        }

        public static UserDal AuthDtoToUserDal(this AuthenticationRequest request)
        {
            return new UserDal()
            {
                Phone = request.Phone,
                Password = request.Password
            };
        }
	}
}
