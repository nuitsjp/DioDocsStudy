using System;
using ConsoleTables;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly string Query = @"
declare @MaxBusinessEntitiID int;

select
	@MaxBusinessEntitiID = max(BusinessEntityID)
from
	AdventureWorks.Person.Person

select top 5
	BusinessEntityID,
	FirstName,
	LastName
from
	AdventureWorks.Person.Person";

        static void Main(string[] args)
        {
            var connectionString = 
                new SqlConnectionStringBuilder
                {
                    DataSource = "localhost",
                    UserID = "sa",
                    Password = "P@ssw0rd!"
                }.ToString();
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var persons = connection.Query<Person>(Query);
            
            ConsoleTable
                .From(persons)
                .Write();
        }
    }

    public class Person
    {
        public Person(int businessEntityId, string firstName, string lastName)
        {
            BusinessEntityID = businessEntityId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int BusinessEntityID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
