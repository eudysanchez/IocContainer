using MvcDomain;
using MvcIoC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MvcIoC.Tests.Controllers
{
    public class CompanyControllerTest
    {
        [Fact]
        public void CompanyController_CallIndex_ReturnTypeisString()
        {
            // Arrange
            ICompanyRepository repo = new CompanyRepository();
            CompanyController controller = new CompanyController(repo);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<string>(result);
        }

        [Fact]
        public void CompanyController_CallIndex_ReturnCorrectSubstrings()
        {
            // Arrange
            ICompanyRepository repo = new CompanyRepository();
            CompanyController controller = new CompanyController(repo);
            var str = "Bank Of America";
            var str1 = "Engenering";
            var str2 = "Toyota";

            // Act
            var result = controller.Index();

            // Assert
            Assert.Contains(str, result);
            Assert.Contains(str1, result);
            Assert.Contains(str2, result);

        }
    }
}
