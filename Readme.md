# HashiCorp Vault Asp.Net Core 2.0 Netstandard implementation
The goal of this project is to provide a way to read encrypted values from HashiCorp Vault thru environment variables inside the
Kubernetes pods.

- ConfigMap - store non-secure information i.e HashiCorp Vault url with port number and keys for the secure data to be retrieved.
- Secrets - store HashiCorp Vault credentials.

-

# Tools
- Visual Studio.NET 2017
- Visual Studio Code

# Resources

# Configurations

1. Json format
```
{
        "VaultOptions": {
        "Server": "http://localhost:8300",
        // "RoleId": "",
        // "SecretId": "",
        "TokenId": "root_dev_token",
        "Prefix": "secret",
        "Secrets": [
            "connectionString",
            "option1"
        ]
    }
}
```
2. YAML format
```
VaultOptions:
  Server: http://localhost:8300
  TokenId: root_dev_token
  Prefix: secret
  Secrets:
  - connectionString
  - option1
```

# Docker Communication
In order to troubleshoot connection between the Docker container, log into one of the containers and install `ping` [utility](https://stackoverflow.com/questions/39901311/docker-ubuntu-bash-ping-command-not-found):
```
apt-get update && apt-get install -y iputils-ping
```