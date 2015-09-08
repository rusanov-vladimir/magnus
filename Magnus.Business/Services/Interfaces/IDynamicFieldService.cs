namespace Magnus.Business.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Domain;
	using Dtos;
	using Dtos.DynamicFields;

	public interface IDynamicFieldService: IDisposable
	{
		Dictionary<string, string> Export(bool useStaticExport);
		string ExtractFieldsDynamic(Document image);
		string ExtractFieldsStatic(Document image);
		IEnumerable<DynamicFieldTemplateDto> GetDynamicFieldConfiguration(Guid projectId);
		IEnumerable<DynamicFieldDto> SaveFields(IEnumerable<DynamicFieldDto> fieldDtos, TaskDto taskDto);
	}
}