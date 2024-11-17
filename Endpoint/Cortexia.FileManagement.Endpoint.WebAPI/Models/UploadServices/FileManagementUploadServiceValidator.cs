using Cortexia.FileManagement.Endpoint.WebAPI.Translators;
using FluentValidation;
using Zamin.Extensions.Translations.Abstractions;

namespace Cortexia.FileManagement.Endpoint.WebAPI.Models.UploadServices;

public class FileManagementUploadServiceValidator : AbstractValidator<FileManagementUploadServiceModel>
{
    public FileManagementUploadServiceValidator(ITranslator translator)
    {
        RuleFor(p => p.File)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementUploadServiceModel.File)])
            .Must(file => file.Length != 0).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementUploadServiceModel.File)]);

        RuleFor(p => p.ServiceName)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementUploadServiceModel.ServiceName)])
            .MaximumLength(100).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_MAX_LENGTH, nameof(FileManagementUploadServiceModel.ServiceName)]);

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementUploadServiceModel.Name)])
            .MaximumLength(200).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_MAX_LENGTH, nameof(FileManagementUploadServiceModel.Name)]);

        RuleFor(p => p.Content)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementUploadServiceModel.Content)])
            .MaximumLength(50).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_MAX_LENGTH, nameof(FileManagementUploadServiceModel.Content)]);

        RuleFor(p => p.Extension)
            .NotEmpty().WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_NOT_EMPTY, nameof(FileManagementUploadServiceModel.Extension)])
            .MaximumLength(50).WithMessage(translator[TranslatorKeys.VALIDATION_ERROR_MAX_LENGTH, nameof(FileManagementUploadServiceModel.Extension)]);
    }
}
