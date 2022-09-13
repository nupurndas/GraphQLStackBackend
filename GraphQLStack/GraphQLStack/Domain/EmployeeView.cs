using HotChocolate.AspNetCore.Authorization;

namespace GraphQLStack.Domain
{
    [Authorize]
    public class EmployeeView
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SupervisorName { get; set; }
    }
}

/*
 https://github.com/ChilliCream/graphql-workshop
 mutation {
    login(email: "test", password: "test") {
      myToken
    } 
  }

query {
    employeeDetail {
      id
      name
      supervisorName
    }
  }
 */ 