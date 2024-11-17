using Coretexia.FileManagement.Abstraction.Models.DeleteServices;
using Cortexia.FileManagement.Endpoint.WebAPI.Translators;
using FluentValidation;
using Zamin.Extensions.Translations.Abstractions;

namespace Cortexia.FileManagement.Endpoint.WebAPI.Models.DeleteServices;

public class FileManagementDeleteServiceValidator : AbstractValidator<FileManagementDeleteServiceModel>
{
    public FileManagementDeleteServiceValidator(ITranslator translator)
    {
        RuleFor(p => p.Key)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementDeleteServiceInput.Key)]);

        RuleFor(p => p.ServiceName)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementDeleteServiceModel.ServiceName)])
            .MaximumLength(100).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_MAX_LENGTH, nameof(FileManagementDeleteServiceModel.ServiceName)]);
    }
}