using Application.DTO;

namespace Application.Service;

public interface ISnapshotService
{
    /// <summary>
    /// Разбирает JSON-снапшот из потока и заменяет им содержимое базы.
    /// </summary>
    Task<SnapshotImportResultDto> ReplaceFromJsonAsync(Stream json, CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверяет уже разобранный снапшот и заменяет им содержимое базы.
    /// </summary>
    Task<SnapshotImportResultDto> ReplaceAsync(SnapshotDto snapshot, CancellationToken cancellationToken = default);
}
