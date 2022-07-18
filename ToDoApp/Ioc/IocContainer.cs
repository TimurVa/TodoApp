using Ninject;
using ToDoApp.Database;
using ToDoApp.Helpers.Services;
using ToDoApp.Interfaces;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Ioc
{
    public static class IocContainer
    {
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        public static IRepository<TodoModel> TodoRepository => Get<IRepository<TodoModel>>();

        public static IRepository<PasswordModel> PasswordRepository => Get<IRepository<PasswordModel>>();

        public static ISettings Settings => Get<ISettings>();

        public static ISettingsProvider SettingsProvider => Get<ISettingsProvider>();

        /// <summary>
        /// Sets up the IoC container, binds all information required and is ready for use
        /// NOTE: Must be called as soon as your application starts up to ensure all 
        ///       services can be found
        /// </summary>
        public static void Setup()
        {
            Kernel.Bind<IRepository<TodoModel>>().To<TodoRepository>().InSingletonScope();
            Kernel.Bind<IRepository<PasswordModel>>().To<PasswordRepository>().InSingletonScope();
            Kernel.Bind<ISettings>().ToConstant(new SettingsViewModel());
            Kernel.Bind<ISettingsProvider>().To<SettingsProvider>().InSingletonScope();
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
