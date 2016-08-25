﻿using System;
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
            Add(new Company { Name = "TIMKEN Eng", Category = "Engenering" });
            Add(new Company { Name = "Wipro", Category = "software" });
            Add(new Company { Name = "HSBC", Category = "Bank" });
        }

        public IEnumerable<Company> GetAll()
        {
            return products;
        }
        public Company Get(int id)
        {
            return products.Find(p => p.Id == id);
        }
        public Company Add(Company item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Id = _nextId++;
            products.Add(item);
            return item;
        }
        public bool Update(Company item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int index = products.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            products.RemoveAt(index);
            products.Add(item);
            return true;
        }
        public bool Delete(int id)
        {
            products.RemoveAll(p => p.Id == id);
            return true;
        }
    }
}
