using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public interface IService<TRequest,TEntityResponse, TResponse, TListResponse, TId>
    {
        TEntityResponse Insert(TRequest Entity);
        TEntityResponse Update(TRequest Entity);
        TResponse Delete(TRequest Entity);
        TResponse Delete(TId Id);
        TListResponse FindAll(TRequest Entity);
        TEntityResponse FindById(TId Id);
   
    }
}
