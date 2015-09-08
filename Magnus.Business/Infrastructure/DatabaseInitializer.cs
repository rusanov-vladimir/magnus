namespace Magnus.Business.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using Domain;
	using Domain.DynamicFields;

	public class DatabaseInitializer : CreateDatabaseIfNotExists<Repository>
	{
		
		protected override void Seed(Repository context)
		{

			var templates = new List<DynamicFieldTemplate>();

			var documentCodeFieldTemplate = new DynamicFieldTemplate
			{
				Code = "DOCCODE",
				IsEnabled = true,
				Label = "Code",
				Length = 13,
				Type = DynamicFieldType.String,
				Weight = 5
			};
			templates.Add(documentCodeFieldTemplate);


			var yearFieldTemplate = new DynamicFieldTemplate
			{
				Code = "YEAR",
				IsEnabled = true,
				Label = "Year",
				Length = 4,
				Type = DynamicFieldType.Integer,
				Weight = 10
			};
			templates.Add(yearFieldTemplate);

			var cardNumberFieldTemplate = new DynamicFieldTemplate
			{
				Code = "CARDNUMB",
				IsEnabled = true,
				Label = "Card Number",
				Length = 6,
				Type = DynamicFieldType.Integer,
				Weight = 15
			};
			templates.Add(cardNumberFieldTemplate);

			var nameFieldTemplate = new DynamicFieldTemplate
			{
				Code = "NAME",
				IsEnabled = true,
				Label = "Name",
				Length = 30,
				Type = DynamicFieldType.String,
				Weight = 20
			};
			templates.Add(nameFieldTemplate);

			var surnameFieldTemplate = new DynamicFieldTemplate
			{
				Code = "SURNAME",
				IsEnabled = true,
				Label = "Surname",
				Length = 30,
				Type = DynamicFieldType.String,
				Weight = 25
			};
			templates.Add(surnameFieldTemplate);


			var dossierFieldTemplate = new DynamicFieldTemplate
			{
				Code = "DOSS",
				IsEnabled = true,
				Label = "Dossier",
				Length = 6,
				Type = DynamicFieldType.String,
				Weight = 30
			};
			templates.Add(dossierFieldTemplate);

			context.DynamicFieldTemplates.AddOrUpdate(x => x.Code, templates.ToArray());

			var project1 = new Project
			{
				FieldTemplates = new Collection<DynamicFieldTemplate>(),
				Name = "FirstCase",
				WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Source\",
				State = StateType.InProgress
			};
			project1.FieldTemplates.Add(documentCodeFieldTemplate);
			project1.FieldTemplates.Add(yearFieldTemplate);
			project1.FieldTemplates.Add(cardNumberFieldTemplate);
			project1.FieldTemplates.Add(nameFieldTemplate);
			project1.FieldTemplates.Add(surnameFieldTemplate);



			context.Projects.AddOrUpdate(x => x.Id, project1);


			var project2 = new Project
			{
				FieldTemplates = new Collection<DynamicFieldTemplate>(),
				Name = "SecondCase",
				WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"..\..\Source\",
				State = StateType.Planning
			};
			project2.FieldTemplates.Add(documentCodeFieldTemplate);
			project2.FieldTemplates.Add(dossierFieldTemplate);
			context.Projects.AddOrUpdate(x => x.Id, project2);



			var user = new User
			{
				Username = "Marian",
				Password = "123123"
			};
			context.Users.AddOrUpdate(x=>x.Id, user);

			var admin = new User
			{
				Username = "admin",
				Password = "admin"
			};
			context.Users.AddOrUpdate(x => x.Id, admin);

			var team = new Team
			{
				Name = "Default team",
				Project = project1,
				Users = new List<User>
				{
					user,
					admin
				}
			};

			context.Teams.AddOrUpdate(x => x.Id, team);


			base.Seed(context);
		}

		
	}
}