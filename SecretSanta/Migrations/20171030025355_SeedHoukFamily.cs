using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SecretSanta.Migrations
{
    public partial class SeedHoukFamily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Group] (Name, LeaderName, LeaderEmail) VALUES ('Houk Family Christmas', 'Marcy', 'pmhouk@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Evan', 'Lundell', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Karla', 'Lundell', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Kenn', 'Houk', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Kari', 'Houk', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Kyle', 'Houk', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Shela', 'Houk', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Kevin', 'Houk', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Cristy', 'Verduzci', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO Person (FirstName, LastName, Email) VALUES ('Nina', 'Verduzci', 'evan.lundell@gmail.com')");
            migrationBuilder.Sql("INSERT INTO PersonGroup SELECT Person.Id, [Group].Id FROM [Group] CROSS JOIN Person");
            migrationBuilder.Sql("INSERT INTO ExceptionGroup (GroupId, Name) VALUES ((SELECT Id FROM [Group] WHERE Name = 'Houk Family Christmas'), 'EvanKarla')");
            migrationBuilder.Sql("INSERT INTO ExceptionGroup (GroupId, Name) VALUES ((SELECT Id FROM [Group] WHERE Name = 'Houk Family Christmas'), 'KyleShela')");
            migrationBuilder.Sql("INSERT INTO ExceptionGroup (GroupId, Name) VALUES ((SELECT Id FROM [Group] WHERE Name = 'Houk Family Christmas'), 'KevinCristy')");
            migrationBuilder.Sql("INSERT INTO ExceptionGroup (GroupId, Name) VALUES ((SELECT Id FROM [Group] WHERE Name = 'Houk Family Christmas'), 'KennKari')");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'EvanKarla'), (SELECT Id FROM Person WHERE FirstName = 'Evan'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'EvanKarla'), (SELECT Id FROM Person WHERE FirstName = 'Karla'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KyleShela'), (SELECT Id FROM Person WHERE FirstName = 'Kyle'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KyleShela'), (SELECT Id FROM Person WHERE FirstName = 'Shela'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KevinCristy'), (SELECT Id FROM Person WHERE FirstName = 'Kevin'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KevinCristy'), (SELECT Id FROM Person WHERE FirstName = 'Cristy'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KevinCristy'), (SELECT Id FROM Person WHERE FirstName = 'Nina'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KennKari'), (SELECT Id FROM Person WHERE FirstName = 'Kenn'))");
            migrationBuilder.Sql("INSERT INTO PersonExceptionGroup (ExceptionGroupId, PersonId) VALUES ((SELECT Id FROM ExceptionGroup WHERE Name = 'KennKari'), (SELECT Id FROM Person WHERE FirstName = 'Kari'))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM PersonExceptionGroup");
            migrationBuilder.Sql("DELETE FROM ExceptionGroup");
            migrationBuilder.Sql("DELETE FROM PersonGroup");
            migrationBuilder.Sql("DELETE FROM Person");
            migrationBuilder.Sql("DELETE FROM Group");
        }
    }
}
