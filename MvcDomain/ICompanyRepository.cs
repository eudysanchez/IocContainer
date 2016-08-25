using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public interface ICompanyRepository 
    {
        IEnumerable<Company> GetAll();
        Company Get(int id);
        Company Add(Company item);
        bool Update(Company item);
        bool Delete(int id);
    }
}
