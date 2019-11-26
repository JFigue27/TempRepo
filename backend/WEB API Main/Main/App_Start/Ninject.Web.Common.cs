[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ReusableWebAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ReusableWebAPI.App_Start.NinjectWebCommon), "Stop")]

namespace ReusableWebAPI.App_Start
{
    using System;
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Web;
    using BusinessSpecificLogic;
    using BusinessSpecificLogic.Logic;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Reusable;
    using ReusableWebAPI.Controllers;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(typeof(DbContext)).To(typeof(MainContext)).InRequestScope();
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InRequestScope();
            kernel.Bind(typeof(IReadOnlyRepository<>)).To(typeof(ReadOnlyRepository<>)).InRequestScope();
            kernel.Bind(typeof(IDocumentRepository<>)).To(typeof(DocumentRepository<>)).InRequestScope();
            kernel.Bind<LoggedUser>().ToMethod(ctx =>
            {
                return new LoggedUser((ClaimsIdentity)HttpContext.Current.User?.Identity);
            }).InRequestScope();
            kernel.Bind(typeof(ReadOnlyLogic<>)).ToSelf().InRequestScope();
            kernel.Bind(typeof(Logic<>)).ToSelf().InRequestScope();
            kernel.Bind(typeof(DocumentLogic<>)).ToSelf().InRequestScope();

            #region Specific App Bindings


            ///Start:Generated:DI<<<
            kernel.Bind<IShiftLogic>().To<ShiftLogic>();
            kernel.Bind<ICertificationLogic>().To<CertificationLogic>();
            kernel.Bind<IJobPositionLogic>().To<JobPositionLogic>();
            kernel.Bind<ILevel1Logic>().To<Level1Logic>();
            kernel.Bind<ILevel2Logic>().To<Level2Logic>();
            kernel.Bind<ILevel3Logic>().To<Level3Logic>();
            kernel.Bind<ILevel4Logic>().To<Level4Logic>();
            kernel.Bind<ILevel5Logic>().To<Level5Logic>();
            kernel.Bind<IEmployeeLogic>().To<EmployeeLogic>();
            kernel.Bind<ITrainingLogic>().To<TrainingLogic>();
            kernel.Bind<ITrainingScoreLogic>().To<TrainingScoreLogic>();
            kernel.Bind<IFormatoDC3Logic>().To<FormatoDC3Logic>();
            kernel.Bind<ISkillLogic>().To<SkillLogic>();
            ///End:Generated:DI<<<
            #endregion

            kernel.Bind<IRoleLogic>().To<RoleLogic>();
            kernel.Bind<IApplicationLogic>().To<ApplicationLogic>();
            kernel.Bind<IUserLogic>().To<UserLogic>();
            kernel.Bind<IWorkflowLogic>().To<WorkflowLogic>();
            kernel.Bind<IStepLogic>().To<StepLogic>();
            kernel.Bind<IStepOperationLogic>().To<StepOperationLogic>();
            kernel.Bind<ITrackLogic>().To<TrackLogic>();
            kernel.Bind<ITokenLogic>().To<TokenLogic>();
            kernel.Bind(typeof(BaseController<>)).ToSelf().InRequestScope();
            kernel.Bind(typeof(ReadOnlyBaseController<>)).ToSelf().InRequestScope();
            kernel.Bind(typeof(DocumentController<>)).ToSelf().InRequestScope();
        }
    }
}