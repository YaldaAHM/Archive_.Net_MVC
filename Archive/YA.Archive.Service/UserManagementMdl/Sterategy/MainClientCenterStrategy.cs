using YA.Archive.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using YA.Archive.Model;
using YA.Archive.Model.UserManagement;
using YA.Archive.Model.UserManagement.Configuration;
using YA.Archive.Service.UserManagementMdl.Mapping;
using YA.Infrastructure.Service;
using YA.Localization.MessageLocalize;
using YA.Archive.Service.UserManagementMdl.VMs;
using YA.Archive.Service.UserManagementMdl.Messaging;
using Microsoft.AspNet.Identity.EntityFramework;

namespace YA.Archive.Service.UserManagementMdl.Sterategy
{
    public class MainClientCenterStrategy : IClientCenterStrategy
      
    {
        private ArchiveDataContext _db;

        public MainClientCenterStrategy()
        {
            _db = new ArchiveDataContext();
        }
        public BaseListResponse<UserVM> ApplyFindByExpressionTo(UserFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            var m = UserManager.Users.Where(expression).OrderBy(p => p.UserName).ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }

       
        public BaseListResponse<UserVM> ApplyFindAllTo(UserFindAllRequest request)
        {
            var m = UserManager.Users.OrderBy(p => p.UserName).ToList();
            var ents = m.ToViewModelList().ToList();
            
            return new UserFindAllResponse
            {
                IsSuccess = true, 
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents
            };
        }
       

        public BaseListResponse<UserVM> ApplyFindByExpressionPagingTo(UserFindByExpressionExpRequest requestExp)
        {
            var expression = ToViewModel(requestExp.expression);
            var total = UserManager.Users.Where(expression).Count();
            var m = UserManager.Users.Where(expression).OrderBy(p => p.UserName).Skip(requestExp.Count * (requestExp.Index - 1)).Take(requestExp.Count).ToList();
            var ents = m.ToViewModelList().ToList();

            return new UserFindByExpressionResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
              
                Entities = ents,
                Index = requestExp.Index,
                Count = requestExp.Count,
                TotalRecords = total,

            };
        }
 public BaseListResponse<UserVM> ApplyFindAllPagingTo(UserFindAllRequest request)
        {
            var total = UserManager.Users.Count();
            var m = UserManager.Users.OrderByDescending(p => p.UserName).Skip(request.Count * (request.Index - 1)).Take(request.Count).ToList(); 
            var ents = m.ToViewModelList().ToList();
           
            return new UserFindAllResponse
            {
                IsSuccess = true,
                Message = MessageResource.FindSuccess,
                ResponseType = ResponseType.Success,
                Entities = ents,
                Index = request.Index,
                Count = request.Count,
                TotalRecords = total,
            };
        }
        private AppUserManager UserManager
        {
            get
            {
                return new AppUserManager(new AppUserStore(_db));
            }
        }


        public Expression<Func<UserModel, bool>> ToViewModel
            (Expression<Func<UserExpVM, bool>> User)
        {

              return Mapper.Map<Expression<Func<UserExpVM, bool>>,
                Expression<Func<UserModel, bool>>>(User);
        }


       

        

       
    }
}
