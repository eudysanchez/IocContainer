using System;
using Xunit;
using IoCContainer.ContainerFolder;
using IocContainer.Tests.Helpers;
using IoCContainer.Exceptions;
using IoCContainer.ValueObjects;
using System.Reflection.Emit;

namespace IocContainer.Tests
{
    
    public class ContainerTest
    {
        [Fact]
        public void RegisterType_CallContainer_TypeRegistered()
        {
            //Arrange
            IContainer container = new Container();
            IHelper1 help1 = new Helper1();

            //Act
            container.Register<IHelper1, Helper1>();
            var actualType1 = container.Resolve<IHelper1>();

            //Assert
            Assert.IsType(help1.GetType(), actualType1);

        }

        [Fact]
        public void RegisterCompositeType_CallContainer_TypeRegistered()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();
            ICompositeItem compositeItem = new CompositeItem(help1, help2);


            //Act
            container.Register<IHelper1, Helper1>();
            container.Register<IHelper2, Helper2>();
            container.Register<ICompositeItem, CompositeItem>();
            var actualType = container.Resolve<ICompositeItem>();

            //Assert
            Assert.IsType(compositeItem.GetType(), actualType);

        }

        [Fact]
        public void RegisterType_TwoDifferentImplementationsTwoDifferentInterfaces_TypesAreRegistered()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();

            //Act
            container.Register<IHelper1, Helper1>();
            container.Register<IHelper2, Helper2>();
            var actualType1 = container.Resolve<IHelper1>();
            var actualType2 = container.Resolve<IHelper2>();

            //Assert
            Assert.IsType(help1.GetType(), actualType1);
            Assert.IsType(help2.GetType(), actualType2);

        }

        [Fact]
        public void RegisterType_OneInterfaceTwoDifferentImplementations_TypesAreRegistered()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();

            //Act
            container.Register<IHelper1, Helper1>();
            container.Register<IHelper1, Helper2>();
            var actualType1 = container.ResolveAll<IHelper1>();

            //Assert
            Assert.IsType(help1.GetType(), actualType1);

        }

        [Fact]
        public void RegisterType_CallContainerRegisterMethodLifeCycleSingleton_TypeRegisteredSucessful()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();

            //Act
            container.Register<IHelper1, Helper1>(LifeCycle.Singleton);
            var actualType1 = container.Resolve<IHelper1>();

            //Assert
            Assert.IsType(help1.GetType(), actualType1);

        }

        [Fact]
        public void RegisterCompositeType_CallContainerResolveMethod_ShouldThrowTypeNotRegisteredException()
        {
            //Arrange
            IContainer container = new Container();
            IHelper1 help1 = new Helper1();

            //Act
            Exception ex = Assert.Throws<TypeNotRegisteredException>(() => container.Resolve<IHelper1>());


            //Assert
            Assert.Equal("The type IHelper1 has not been registered", ex.Message);
        }

        [Fact]
        public void CompositeClassOneDependencyNotRegistered_CallContainerResolveMethod_ShouldThrowFailedToCreateInstanceException()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();
            ICompositeItemWrong compositeItem = new CompositeItemWrong(help1, help2);

            container.Register<IHelper2, Helper2>();
            container.Register<ICompositeItemWrong, CompositeItemWrong>();

            //Act
            Exception ex = Assert.Throws<FailedToCreateInstanceException>(() => container.Resolve<ICompositeItemWrong>());

            //Assert
            Assert.Equal("Can't create instance of type CompositeItemWrong", ex.Message);
        }

        [Fact]
        public void CompositeClassNoDependenciesRegistered_CallContainerResolveMethod_ShouldThrowFailedToCreateInstanceException()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();
            ICompositeItemWrong compositeItem = new CompositeItemWrong(help1, help2);
            container.Register<ICompositeItemWrong, CompositeItemWrong>();

            //Act
            Exception ex = Assert.Throws<FailedToCreateInstanceException>(() => container.Resolve<ICompositeItemWrong>());

            //Assert
            Assert.Equal("Can't create instance of type CompositeItemWrong", ex.Message);
        }

        [Fact]
        public void NoDependencyTagOnClass_CallContainerRegisterMethod_ShouldThrowFailedToCreateInstanceException()
        {
            //Arrange
            IContainer container = new Container();
            Helper1 help1 = new Helper1();
            Helper2 help2 = new Helper2();
            ICompositeItemWrong compositeItem = new CompositeItemWrong(help1, help2);

            container.Register<IHelper1, Helper1>();
            container.Register<IHelper2, Helper2>();
            container.Register<ICompositeItemWrong, CompositeItemWrong>();

            //Act
            Exception ex = Assert.Throws<FailedToCreateInstanceException>(() => container.Resolve<ICompositeItemWrong>());

            //Assert
            Assert.Equal("Can't create instance of type CompositeItemWrong", ex.Message);
        }
    }
}
