# Contributing to Multilinks Identity

:clap::tada: First off, thanks for taking the time to contribute! :tada::clap:

The following is a set of guidelines for contributing to Multilinks Identity. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request.

## Code of Conduct

This project and everyone participating in it is governed by the [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to [support@multilinks.io](mailto:support@multilinks.io).

## Asking Questions

> **Note:** Please don't file an issue to ask a question. You'll get faster results by using the methods below.

* Send us an email: [support@multilinks.io](mailto:support@multilinks.io)
* Join our chat channels over on Slack: [Multilinks Workspace](https://join.slack.com/t/multilinks/shared_invite/enQtNzQxODE0NzMzMjgzLWU0ZjM1MjZiNzU1YTc1OWFjNWRlZWJmNmY0YTJmOGIzMDM1ZWJhYTliNjU3ZjM4NDMxZjc0MzY5NDNjYjllZWI)
   + Use the `#general` channel for general questions or discussion
   + Use the `#multilinks-identity` channel for questions or discussion about Multilinks Idenity project
   + Use the `#multilinks-core` channel for questions or discussion about Multilinks Core project
   + Use the `#multilinks-portal` channel for questions or discussion about Multilinks Portal project

## How Can You Contribute?

### Reporting Bugs

This section guides you through submitting a bug report. Following these guidelines helps maintainers and the community understand your report :pencil:, reproduce the behavior :computer: :computer:, and find related reports :mag_right:.

Before creating bug reports, please check the list of existing issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible.

> **Note:** If you find a **Closed** issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one.

#### How Do I Submit A (Good) Bug Report?

Bugs are tracked as [GitHub issues](https://guides.github.com/features/issues/). After you've determined which repository your bug is related to, create an issue on that repository.

Explain the problem and include additional details to help maintainers reproduce the problem:

* **Use a clear and descriptive title** for the issue to identify the problem.
* **Describe the exact steps which reproduce the problem** in as many details as possible.
* **Provide specific examples to demonstrate the steps**. Include links to files or GitHub projects, or copy/pasteable snippets, which you use in those examples. If you're providing snippets in the issue, use [Markdown code blocks](https://help.github.com/articles/markdown-basics/#multiple-lines).
* **Describe the behavior you observed after following the steps** and point out what exactly is the problem with that behavior.
* **Explain which behavior you expected to see instead and why.**

Provide more context by answering these questions:

* **Did the problem start happening recently**
* If the problem started happening recently, what's the most recent version in which the problem doesn't happen?
* **Can you reliably reproduce the issue?** If not, provide details about how often the problem happens and under which conditions it normally happens.

### Suggesting Enhancements

This section guides you through submitting an enhancement suggestion, including completely new features and minor improvements to existing functionality. Following these guidelines helps maintainers and the community understand your suggestion :pencil: and find related suggestions :mag_right:.

Before creating enhancement suggestions, please check the list of existing issues as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible.

#### How Do I Submit A (Good) Enhancement Suggestion?

Enhancement suggestions are tracked as [GitHub issues](https://guides.github.com/features/issues/). After you've determined which repository your enhancement suggestion is related to, create an issue on that repository and provide the following information:

* **Use a clear and descriptive title** for the issue to identify the suggestion.
* **Provide a step-by-step description of the suggested enhancement** in as many details as possible.
* **Provide specific examples to demonstrate the steps**. Include copy/pasteable snippets which you use in those examples, as [Markdown code blocks](https://help.github.com/articles/markdown-basics/#multiple-lines).
* **Describe the current behavior** and **explain which behavior you expected to see instead** and why.
* **Explain why this enhancement would be useful**

### Your First Code Contribution

Unsure where to begin contributing to Multilinks Identity? You can start by looking through these `help-wanted` issues:

* [Help wanted issues][help-wanted].

#### Local development

Multilinks Identity can be developed locally:

   * Getting Things Up & Running (Windows environment with Visual Studio):
      + Install [Git](https://git-scm.com/)
      + Create a [Sendgrid Account](https://sendgrid.com/)

      + Setting up a SendGrid API key for email service
         - Go to the [API Keys](https://app.sendgrid.com/settings/api_keys) section
         - Click on `Create API Key`
         - Provide an API key name
         - Set API key permissions `Restricted Access`, allow full access to Mail Send
         - Click on `Create & View`
         - Securely store the key for later use

      + Install [Visual Studio 2019 (as of this writing)](https://visualstudio.microsoft.com/vs/)
         - Required Workloads `.NET Core cross-platform development`

      + Fork this MultilinksIdentity repository
      + Clone your forked repository locally `git clone https://github.com/<your-github-account>/MultilinksIdentity.git`
      + Change to the root folder of your cloned repository
      + Double-click on `Multilinks.Identity.sln` to open in Visual Studio

      + In the Solution Explorer, right-click on the Multilinks.Identity project and select `Manage User Secrets`
         - This will create `secrets.json` in `<your-home-folder>\AppData\Roaming\Microsoft\UserSecrets\<project-UserSecretsId>`
         - The `secrets.json` file allows you to store sensitive data e.g. `api keys` as well as machine specific configuration data
         - The `secrets.json` does not get version controlled.

      + Enter the following key/value pairs into `secrets.json`
         - "EmailService:Name": `<your-name>`
         - "EmailService:Email": `<your-email-address>`
         - "EmailService:ApiKey": `<your-sendgrid-api-key-created-earlier>`

         - "ConnectionStrings:IdentityDb": `Server=(localdb)\\mssqllocaldb;Database=Multilinks-Identity;Trusted_Connection=True;MultipleActiveResultSets=true`

         - "Cors:Identity": `https://localhost:44300`
         - "Cors:Core": `https://localhost:44301`
         - "Cors:Portal": `https://localhost:44302`

         - "CoreConfig:Name": `CoreService`
         - "CoreConfig:DisplayName": `Multilinks Core Service`

         - "PortalConfig:AllowedCorsOriginsIdp": `https://localhost:44300`
         - "PortalConfig:AllowedCorsOriginsApi": `https://localhost:44301`
         - "PortalConfig:LoginRedirectUri": `https://localhost:44302/identity-signin-callback`
         - "PortalConfig:SilentLoginRedirectUri": `https://localhost:44302/identity-silent-renew-callback`
         - "PortalConfig:LogoutRedirectUri": `https://localhost:44302/identity-signout-callback`
         - "PortalConfig:RegistrationConfirmedRedirectUri": `https://localhost:44302/identity-registration-confirmed`
         - "PortalConfig:ResetPasswordSuccessfulRedirectUri": `https://localhost:44302/identity-password-reset-confirmed`

         - "SystemOwnerConfig:Email": `<your-email-address>`
         - "SystemOwnerConfig:FirstName": `<your-first-name>`
         - "SystemOwnerConfig:LastName": `<your-last-name>`
         - "SystemOwnerConfig:DefaultPassword": `<your-default-password>`

      + Handling self-signed certificate (if required)
         - Open the Package Manager Console `ALT + T, N, O`
         - Change to project directory `[LOCAL_REPO_FOLDER]/Multilinks.Identity`
         - Enable "dev-certs" trust `dotnet dev-certs https --trust`
         - A dialog will ask for confirmation
         - Close Package Manager Console `SHFT + ESC`

      + Press `F5` to start `Multilinks Identity` or `Ctrl + F5` without the debugger attached

   * Occasionally you will want to merge changes in the upstream repository (the original MultilinksIdentity repository) with your fork:
      + Change to the root folder of your cloned repository
      + Change to the master branch `git checkout master`
      + Pull any changes from the original MultilinksIdentity repository to your local master branch `git pull https://github.com/ChrisDinhNZ/MultilinksIdentity.git master`
      + Manage any merge conflicts and commit your changes `git commit -m <your-commit-message>`
      + Push your changes to your forked repository `git push`

   * Creating a pull request
      + You can follow this [guide](https://help.github.com/en/articles/creating-a-pull-request-from-a-fork) to create a pull request

[help-wanted]:https://github.com/ChrisDinhNZ/MultilinksIdentity/labels/help%20wanted
