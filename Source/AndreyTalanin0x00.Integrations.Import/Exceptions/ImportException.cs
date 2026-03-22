using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using AndreyTalanin0x00.Integrations.Import.Responses;

namespace AndreyTalanin0x00.Integrations.Import.Exceptions;

[Serializable]
public class ImportException : AggregateException
{
    private readonly ReadOnlyCollection<ImportResponseMessage> m_importResponseMessages;

    public ImportException()
    {
        m_importResponseMessages = new ReadOnlyCollection<ImportResponseMessage>([]);
    }

    public ImportException(string message, IEnumerable<ImportResponseMessage>? importResponseMessages = null)
        : base(message)
    {
        m_importResponseMessages = new ReadOnlyCollection<ImportResponseMessage>([.. importResponseMessages ?? []]);
    }

    public ImportException(string message, Exception innerException, IEnumerable<ImportResponseMessage>? importResponseMessages = null)
        : base(message, innerException)
    {
        m_importResponseMessages = new ReadOnlyCollection<ImportResponseMessage>([.. importResponseMessages ?? []]);
    }

    public ReadOnlyCollection<ImportResponseMessage> ImportResponseMessages => m_importResponseMessages;
}
