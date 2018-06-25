using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration.HashiCorpVault
{
    /// <summary>
    ///  HashiCorp Vault basic configurations settings.
    /// </summary>
    public class VaultOptions
    {
        /// <summary>
        /// Uri with port of the vault server.
        /// </summary>
        public string Server { get; set; }

        #region AppRole Authentication Schema
        /// <summary>
        /// Role id of the app role for this specific client.
        /// <see cref="!:https://www.hashicorp.com/blog/authenticating-applications-with-vault-approle"/>
        /// or
        /// <see cref="!:https://www.vaultproject.io/docs/auth/approle.html"/>
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// Secret id corresponding to the role id.
        /// <see cref="!:https://www.hashicorp.com/blog/authenticating-applications-with-vault-approle"/>
        /// or
        /// <see cref="!:https://www.vaultproject.io/docs/auth/approle.html"/>
        /// </summary>
        public string SecretId { get; set; }
        #endregion

        /// <summary>
        /// The token method is built-in and automatically available at /auth/token. 
        /// It allows users to authenticate using a token, as well to create new tokens, revoke secrets by token, and more.
        /// <see cref="!:https://www.vaultproject.io/docs/auth/token.html"/>
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// Prefix as secret/ or any custom path like secret/group1
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Secrets Keys to retrieve
        /// </summary>
        public IEnumerable<string> Secrets { get; set; }
    }
}
