using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    /// Client class to perform cryptographic key operations and vault operations
    /// against the HashiCorp Vault service.
    /// Thread safety: This class is thread-safe.
    /// </summary>
    public interface IHashiCorpVaultClient
    {
        /// <summary>
        ///  Get all of the secrets
        /// </summary>
        /// <param name="storagePath"></param>
        /// <returns></returns>
        Task<string> GetSecretAsync(string storagePath);
    }
}
