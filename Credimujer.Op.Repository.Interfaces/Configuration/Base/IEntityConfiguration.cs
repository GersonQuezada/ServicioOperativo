using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Credimujer.Op.Repository.Interfaces.Configuration.Base
{
    public interface IEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {

    }
}
