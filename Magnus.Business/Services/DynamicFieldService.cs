namespace Magnus.Business.Services
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Threading.Tasks;
	using Domain;
	using Domain.DynamicFields;
	using Dtos;
	using Dtos.DynamicFields;
	using Infrastructure;
	using Interfaces;

	public class DynamicFieldService : IDynamicFieldService, IDisposable
	{
		private readonly IRepository _repository;

		public DynamicFieldService(IRepository repository)
		{
			_repository = repository;
		}

		public Dictionary<string, string> Export(bool useStaticExport)
		{
			var export = new Dictionary<string, string>();
			var completedTasks = _repository.Query<Domain.Task>().Where(x => x.Project.State == StateType.Finished);
			foreach (var completedTask in completedTasks)
			{
				var exportString = string.Empty;
				foreach (Document document in completedTask.Documents)
				{
					exportString += useStaticExport ? ExtractFieldsStatic(document) : ExtractFieldsDynamic(document);
				}
				export[completedTask.CurrentUser.Username + " Task-" + completedTask.Name] = exportString;
			}
			return export;
		}


		public string ExtractFieldsDynamic(Document image)
		{
			string result = string.Empty;
			var imageFields = image.Fields.OrderBy(y => y.Configuration.Weight).ThenBy(z => z.Configuration.Label);
			result += imageFields.Aggregate(string.Empty, (res, field) => res + field.GetValue() + ";") +
					Environment.NewLine;
			return result;
		}

		public string ExtractFieldsStatic(Document image)
		{
			string result = string.Empty;
			var imageFields = image.Fields.OrderBy(y => y.Configuration.Weight).ThenBy(z => z.Configuration.Label);
			result += imageFields.Aggregate(string.Empty, (res, field) => res + field.GetValue()
				.ToString().PadRight(field.Configuration.Length.Value)) + Environment.NewLine;
			return result;
		}

		public IEnumerable<DynamicFieldTemplateDto> GetDynamicFieldConfiguration(Guid projectId)
		{
			var dynamicFieldTemplateDtos =
				_repository.Query<Project>()
							.Where(x => x.Id == projectId)
							.SelectMany(x => x.FieldTemplates)
							.OrderBy(x=>x.Weight)
							.Select(x => new DynamicFieldTemplateDto
							{
								Id = x.Id,
								Code = x.Code,
								IsEnabled = x.IsEnabled,
								Label = x.Label,
								Length = x.Length,
								Weight = x.Weight,
								Type = x.Type
							});

			var temp = _repository.Query<Project>().Where(x => x.Id == projectId);
			return dynamicFieldTemplateDtos;
		}


		public async Task<string> ExtractFields()
		{
			string result = string.Empty;
			await _repository.Query<Document>().ForEachAsync(x => result += x.Fields.Aggregate(string.Empty,(res,field) => res + field.GetValue() + ";") + "\n" );
			return result;
		}

		public IEnumerable<DynamicFieldDto> SaveFields(IEnumerable<DynamicFieldDto> fieldDtos, TaskDto taskDto)
		{

			var document = _repository.Query<Document>().SingleOrDefault(x => x.Id == taskDto.CurrentDocument.Id);

			if (document == null)
			{
				throw new ArgumentException("Task should have this document");
			}

			var fieldToReturn = new List<DynamicField>();
			foreach (var dynamicFieldDto in fieldDtos)
			{
				if (dynamicFieldDto.Id == Guid.Empty)
				{
					var newField = new DynamicField
					{
						Configuration = _repository.Query<DynamicFieldTemplate>().FirstOrDefault(x => x.Id == dynamicFieldDto.Configuration.Id)
					};

					var newFieldValue = dynamicFieldDto.GetValue();
					if (newFieldValue != null)
					{
						newField.SetValue(newFieldValue);
						document.Fields.Add(newField);
					}
					else
					{
						newField.Id = Guid.Empty;
					}
					fieldToReturn.Add(newField);
				}
				else
				{
					var dynamicFieldToUpdate = _repository.Query<DynamicField>().FirstOrDefault(x => x.Id == dynamicFieldDto.Id);
					if (dynamicFieldToUpdate == null)
						throw new ArgumentException("Field to update is not found");

					var newFieldValue = dynamicFieldDto.GetValue();
					dynamicFieldToUpdate.SetValue(newFieldValue);

					fieldToReturn.Add(dynamicFieldToUpdate);
				}
			}
			_repository.Commit();
			

			var result = new List<DynamicFieldDto>();
			foreach (var dynamicField in fieldToReturn)
			{
				var dynamicFieldDto = new DynamicFieldDto
				{
					Id = dynamicField.Id,
					Configuration = new DynamicFieldTemplateDto
					{
						Id = dynamicField.Configuration.Id,
						Code = dynamicField.Configuration.Code,
						IsEnabled = dynamicField.Configuration.IsEnabled,
						Label = dynamicField.Configuration.Label,
						Length = dynamicField.Configuration.Length,
						Type = dynamicField.Configuration.Type
					}

				};
				dynamicFieldDto.CreateValue();
				dynamicFieldDto.Value.Id = dynamicField.Value.Id;
				var newValue = dynamicField.GetValue();
				if (newValue != null)
					dynamicFieldDto.SetValue(newValue);
				result.Add(dynamicFieldDto);
				
			}

			return result;
		}

		public void Dispose()
		{
			_repository.Dispose();
		}
	}
}