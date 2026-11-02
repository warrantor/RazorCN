# RazorCN

## Restoring packages from GitHub Packages

`Nuget.config` reads GitHub Packages credentials from environment variables.
Never commit a token to this repository.

1. Create a token with the `read:packages` scope only (classic, or fine-grained with packages read access).
2. Export the variables before running `dotnet restore`/`dotnet build`:

   ```sh
   export GITHUB_PACKAGES_USER=<your-github-username>
   export GITHUB_PACKAGES_TOKEN=<token>
   ```

Alternatively, store the credentials in your user-level `~/.nuget/NuGet/NuGet.Config`
(outside the repo) via `dotnet nuget update source github --username <user> --password <token> --store-password-in-clear-text`.
