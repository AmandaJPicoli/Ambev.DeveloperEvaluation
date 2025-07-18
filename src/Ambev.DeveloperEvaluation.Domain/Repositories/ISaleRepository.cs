using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Sale aggregate operations.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Creates a new sale.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale with generated ID.</returns>
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a sale by its identifier.
        /// </summary>
        /// <param name="id">The sale identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all sales.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Collection of all sales.</returns>
        Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing sale.
        /// </summary>
        /// <param name="sale">The sale to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale.</returns>
        Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a sale by its identifier.
        /// </summary>
        /// <param name="id">The sale identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if deleted successfully, false if not found.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a sale exists by sale number.
        /// </summary>
        /// <param name="saleNumber">The sale number.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if exists, false otherwise.</returns>
        Task<bool> ExistsAsync(string saleNumber, CancellationToken cancellationToken = default);
    }
}
