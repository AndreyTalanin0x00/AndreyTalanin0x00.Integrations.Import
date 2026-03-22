using System;

using Microsoft.Extensions.DependencyInjection;

namespace AndreyTalanin0x00.Integrations.Import.DependencyInjection;

internal class ImportPipelineChannelServiceDescriptor
{
    public required ImportPipelineChannelKey? ImportPipelineChannelKey { get; set; }

    public required Action<IServiceCollection> AddImportPipelineChannelServiceCollectionVisitor { get; set; }
}
