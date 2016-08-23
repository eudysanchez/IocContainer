using System;
using Xunit;
using IoCContainer.Container;
using IocContainer.Tests.Helpers;

namespace IocContainer.Tests
{
    
    public class ContainerTest
    {
        [Fact]
        public void RegisterType_CallContainer_TypeRegistered()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();

            //Act
            container.Register<IHelper1, Helper1>();
            container.Register<IHelper1, Helper2>();
            var actualType1 = container.Resolve<Helper1>();
            var actualType2 = container.Resolve<Helper2>();

            //Assert
            Assert.IsType(help1.GetType(), actualType1);
            Assert.IsType(help2.GetType(), actualType2);

        }
    }
}
