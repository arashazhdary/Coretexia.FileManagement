using Cortexia.FileManagement.Endpoint.WebAPI.Translators;
using FluentValidation;
using Zamin.Extensions.Translations.Abstractions;

namespace Cortexia.FileManagement.Endpoint.WebAPI.Models.DownloadServices;

public class FileManagementDownloadServiceValidator : AbstractValidator<FileManagementDownloadServiceModel>
{
    public FileManagementDownloadServiceValidator(ITranslator translator)
    {
        RuleFor(p => p.Key)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementDownloadServiceModel.Key)]);

        RuleFor(p => p.ServiceName)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementDownloadServiceModel.ServiceName)])
            .MaximumLength(100).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_MAX_LENGTH, nameof(FileManagementDownloadServiceModel.ServiceName)]);
    }
}