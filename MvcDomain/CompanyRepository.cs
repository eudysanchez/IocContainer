using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public class CompanyRepository : ICompanyRepository
    {
        private List<Company> products = new List<Company>();
        private int _nextId = 1;

        public CompanyRepository()
        {
            // Add products for the Demonstration  
            Add(new Company { Name = "Qualcomm", Category = "Engenering" });
            Add(new Company { Name = "Uber", Category = "software" });
            Add(new Company { Name = "Bank Of America", Category = "Bank" });
            Add(new Company { Name = "Toyota", Category = "Car" });

        }

        public IEnumerable<Company> GetAll()
        {
            return products;
        }

        public Company Get(int id)
        {
            return products.Find(p => p.Id == id);
        }

        public Company Add(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException("The parameter company Can't be null");
            }

            company.Id = _nextId++;
            products.Add(company);
            return company;
        }

        public bool Update(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException("The parameter company Can't be null");
            }

            int index = products.FindIndex(p => p.Id == company.Id);
            if (index == -1)
            {
                return false;
            }
            products[index] = company;
            return true;
        }

        public bool Delete(int id)
        {
            products.RemoveAll(p => p.Id == id);
            return true;
        }
    }
}
