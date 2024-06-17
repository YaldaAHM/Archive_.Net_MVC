using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YA.Infrastructure.Service
{
    public abstract class ViewModelBase<T>
    {
        [ScaffoldColumn(false)]

        public T Id { get; set; }
    }
}
