using Ninject;
using ToDoApp.Database;
using ToDoApp.Models;

namespace ToDoApp.Ioc
{
    public static class IocContainer
    {
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        public static IRepository<TodoModel> TodoRepository => Get<IRepository<TodoModel>>();

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as your application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            Kernel.Bind<IRepository<TodoModel>>().To<TodoRepository>().InSingletonScope();
        }


        #region Shortcut

        /// <summary>
        /// Get's a service from the IoC, of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>() => Kernel.Get<T>();

        #endregion
    }
}
