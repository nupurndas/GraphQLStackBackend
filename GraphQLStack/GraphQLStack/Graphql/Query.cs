using GraphQLStack.Domain;
//using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using HotChocolate.AspNetCore.Authorization;

namespace GraphQLStack.Graphql
{
    public class Query
    {
        public Query(Context context)
        {
            context.Database.EnsureCreated();
        }
        [UseFiltering]
        public IQueryable<Rocket> GetRockets([Service] Context context)
        {
            return context.Rockets;
        }
        [UseFiltering]
        public IQueryable<Engine> GetEngines([Service] Context context)
        {
            return context.Engines;
        }

        [UseFiltering]
        public IQueryable<Employee> GetEmployees([Service] Context context)
        {
            return context.Employees;
        }

        [Authorize]
        public string HelloWorld([Service] Context context)
        {
            return "Hello World";

        }

        public IQueryable<EmployeeView> GetEmployeeDetail([Service] Context context)
        {
            IEnumerable<EmployeeView> employees =
            from m in context.Employees
            join e1 in context.Employees on m.SupervisorId equals e1.Id 
            select new EmployeeView
            {
                Id = m.Id,
                Name = m.FirstName,
                SupervisorName = m.SupervisorId == 0 ? "" : e1.FirstName,
            };

            IQueryable<EmployeeView> empViews = employees.AsQueryable();
            //GenericMethod.Invoke(this, new object[] { employees });
            return empViews;
           //return context.Employees.Where(item => item.Id == supervisorid);
        }
        private void Test(object myListObject)
        {
            Type myGenericType = myListObject.GetType().GetGenericArguments().First();

            MethodInfo methodToCall = typeof(Employee).GetMethods().Single(
                method => method.Name.Equals("GenericMethod") && method.GetParameters().First().Name.Equals("myEnumerableArgument"));

            MethodInfo genericMethod = methodToCall.MakeGenericMethod(myGenericType);

            genericMethod.Invoke(this, new object[] { myListObject });
        }


        public void GenericMethod<T>(IQueryable<T> myQueryableArgument)
        {
        }

        public void GenericMethod<T>(IEnumerable<T> myEnumerableArgument)
        {
            GenericMethod<T>(myEnumerableArgument.AsQueryable());
        }
    }

}
