﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL
{
	public interface IUserRepository
	{
        void CreateAsync(UserDal user);
        Task<UserDal> GetByPhoneAsync(string phone);
        Task<bool> IsUserExistsAsync(UserDal user);
        Task UpdateLastLoginAsync(string phone, DateTime loginTime);
    }
}
