# Welcome\!

#### This repository contains the source code for:

   * The Multilinks Idenity Provider Service

# Project Overview

Please take a few minutes to review the overview below before diving into the code.

## The Multilinks Platform (The Big Picture)

## The Multilinks Idenity Provider Service

# Dev Environment

## Prerequisite

   * [Git](https://git-scm.com/)
   * [Visual Studio 2019 (as of this writing)](https://visualstudio.microsoft.com/vs/)
      + Required Workloads => .NET Core cross-platform development
   * [Sendgrid Account](https://sendgrid.com/)

## Getting Things Up & Running

   * Clone repository => git clone https://multilinks.visualstudio.com/MultilinksProject/_git/MultilinksTokenService
   * Set up a SendGrid API key for application email service
      + Go to the [API Keys](https://app.sendgrid.com/settings/api_keys) section
      + Click on "Create API Key"
      + Provide an API key name => API Key Name = "MDS Multilinks"
      + Select API key permissions => API Key Permissions = Restricted Access
         - Allow full access to Mail Send
      + Click on "Create & View"
      + Store the key securely for later use
   * Open the Multilinks Idenity Provider Service solution in Visual Studio => [PATH_TO_REPO_FOLDER]/Multilinks.TokenService.sln
   * Using the Secret Manager Tool to store application secrets
      + Right-click on "Multilinks.TokenService" project and select "Manage User Secrets" from the context menu
      + The "secrets.json" will opened
      + Enter Key/Value secrets into "secrets.json" or via the [command line](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows#set-a-secret)
         - "EmailService:ApiKey": "[SENDGRID_API_KEY_CREATED_EARLIER]",
         - "EmailService:Email": "[EMAIL_ADDRESS_FOR_DEV_ENVIRONMENT]",
         - "EmailService:Name": "[NAME_ASSOCIATED_WITH_EMAIL_ADDRESS]"
