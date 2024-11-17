using Coretexia.FileManagement.Abstraction.Models.DeleteServices;
using Coretexia.FileManagement.Abstraction.Models.DownloadServices;
using Coretexia.FileManagement.Abstraction.Models.UploadServices;
using Coretexia.FileManagement.Abstraction.Services;
using Cortexia.FileManagement.Endpoint.WebAPI.Models.DeleteServices;
using Cortexia.FileManagement.Endpoint.WebAPI.Models.DownloadServices;
using Cortexia.FileManagement.Endpoint.WebAPI.Models.UploadServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Cortexia.FileManagement.Endpoint.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
public class FileManagementController : ControllerBase
{
    private readonly IFileManagement _fileManagement;

    public FileManagementController(IFileManagement fileManagement)
    {
        _fileManagement = fileManagement;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromServices] IValidator<FileManagementUploadServiceModel> validator,
                                            [FromForm] FileManagementUploadServiceModel model)
    {
        var validationResult = validator.Validate(model);
        var errors = validationResult.Errors.Select(q => q.ErrorMessage);
        if (errors.Any())
            return BadRequest(errors);

        using var memoryStream = new MemoryStream();
        await model.File.CopyToAsync(memoryStream);
        var input = new FileManagementUploadServiceInput(file: memoryStream.ToArray(),
                                                         serviceName: model.ServiceName,
                                                         name: model.Name,
                                                         content: model.Content,
                                                         extension: model.Extension);
        var result = await _fileManagement.Upload(input);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromServices] IValidator<FileManagementDeleteServiceModel> validator,
                                            [FromBody] FileManagementDeleteServiceModel model)
    {
        var validationResult = validator.Validate(model);
        var errors = validationResult.Errors.Select(q => q.ErrorMessage);
        if (errors.Any())
            return BadRequest(errors);

        var input = new FileManagementDeleteServiceInput(key: model.Key,
                                                         serviceName: model.ServiceName);
        var result = await _fileManagement.Delete(input);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Download([FromServices] IValidator<FileManagementDownloadServiceModel> validator,
                                              [FromQuery] FileManagementDownloadServiceModel model)
    {
        var validationResult = validator.Validate(model);
        var errors = validationResult.Errors.Select(q => q.ErrorMessage);
        if (errors.Any())
            return BadRequest(errors);

        var input = new FileManagementDownloadServiceInput(key: model.Key,
                                                           serviceName: model.ServiceName);
        var result = await _fileManagement.Download(input);
        return Ok(result);
    }
}
