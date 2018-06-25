using System;
using System.Linq;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.AppRole;
using VaultSharp.Backends.Authentication.Models.Token;

namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    /// Abstracts dependency on VaultSharp nuget package. 
    /// This class can be updated to use any other open-source library to communication with HashiCorp Vault server.
    /// </summary>
    public class HashiCorpVaultClientWrapper : IHashiCorpVaultClient
    {
        private readonly VaultOptions _options;
        private readonly IVaultClient _vaultClientImpl;

        #region ctor(s)
        /// <summary>
        ///  Initializes a new instance with server and tokenId
        /// </summary>
        /// <param name="server">HashiCorp Vault url with port number.</param>
        /// <param name="tokenId">Authetnication token id</param>
        public HashiCorpVaultClientWrapper(string server, string tokenId) 
            : this(new VaultOptions { Server = server, TokenId = tokenId })
        {
        }

        /// <summary>
        /// Initializes a new instance with server and approle authetnicatio method
        /// </summary>
        /// <param name="server">HashiCorp Vault url with port number.</param>
        /// <param name="roleId">App Role Id to be used for authentication</param>
        /// <param name="roleSecret">App Role Secret Id for authentication</param>
        public HashiCorpVaultClientWrapper(string server, string roleId, string roleSecret) 
            : this(new VaultOptions { Server = server, RoleId = roleId, SecretId = roleSecret })
        {
        }

        /// <summary>
        /// Initializes a new instance with <see cref="VaultOptions"/>
        /// </summary>
        /// <param name="options"></param>
        public HashiCorpVaultClientWrapper(VaultOptions options)
        {
            _options = options;

            IAuthenticationInfo authInfo;

            // token present so authetnication with token
            if (!string.IsNullOrWhiteSpace(_options.TokenId))
            {
                authInfo = new TokenAuthenticationInfo(_options.TokenId);
            }
            else
            {
                authInfo = new AppRoleAuthenticationInfo("approle", _options.RoleId, _options.SecretId);
            }

            _vaultClientImpl = VaultClientFactory.CreateVaultClient(new Uri(_options.Server), authInfo);
        } 
        #endregion

        /// <inheritdoc/>
        public async Task<string> GetSecretAsync(string storagePath)
        {
            var data = await _vaultClientImpl.ReadSecretAsync(storagePath);
            return data.Data.Values.First().ToString();
        }

    }
}
