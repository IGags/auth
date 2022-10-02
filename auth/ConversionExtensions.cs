using System;
using System.Collections.Generic;
using System.Linq;
using Api.Areas.Api.Models;
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

        public static UserDal AuthDtoToUserDal(this LoginRequest request)
        {
            return new UserDal()
            {
                Phone = request.Phone,
                Password = request.Password
            };
        }

        public static UserResponse ClaimsToResponse(this IEnumerable<System.Security.Claims.Claim> claims)
        {
            var claimList = claims.ToList();
            var response = new UserResponse()
            {
                Phone = claimList[0].Value,
                FIO = claimList[1].Value,
                Email = claimList[2].Value,
                LastLogin = claimList[3].Value,
            };
            return response;
        }
	}
}
