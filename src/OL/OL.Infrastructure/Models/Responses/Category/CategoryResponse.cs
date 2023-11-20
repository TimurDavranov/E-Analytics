using EAnalytics.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OL.Infrastructure.Models.Responses.Category;

public record CategoryResponse(Guid Id, long SystemId, long? ParentId, IReadOnlyList<TranslationDto> Translations);
