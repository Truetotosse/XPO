using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Linq;
using XafSolution.Module.BusinessObjects;

namespace XamarinFormsDemo {
    public static class XpoHelper {
        public static SecuredObjectSpaceProvider objectSpaceProvider;
        public static AuthenticationStandard authentication; 
        public static SecurityStrategyComplex security; 
        public static void InitXpo(string connectionString, string login, string password) {
            RegisterEntities();
            authentication = new AuthenticationStandard();
            security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();
            objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);

            authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(login, password));
            IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
            security.Logon(loginObjectSpace);

            var space = objectSpaceProvider.CreateObjectSpace() as XPObjectSpace;
            //XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, objectSpaceProvider);
            XpoDefault.Session = space.Session;

            
        }
        public static UnitOfWork CreateUnitOfWork() {
            var space = objectSpaceProvider.CreateObjectSpace() as XPObjectSpace;
            return space.Session as UnitOfWork;
        }
        

        private static void RegisterEntities() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}
