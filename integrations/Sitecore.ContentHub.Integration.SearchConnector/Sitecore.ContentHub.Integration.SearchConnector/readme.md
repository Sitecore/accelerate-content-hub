# Content Hub to Sitecore Search Connector

**Content Hub Search Connector** is a reference implementation for synchronising data between [Sitecore Content Hub](https://www.sitecore.com/products/content-hub) and [Sitecore Search](https://www.sitecore.com/products/sitecore-search). Built with Azure Functions and designed for extensibility, this open-source solution provides a solid foundation for implementing custom integrations tailored to your data model and business rules.

It supports both API-based and queue-based ingestion, with flexible field mapping via a JSON configuration.

## Project Overview

This solution includes three key projects:

* **`Sitecore.ContentHub.Integration.SearchConnector`**
  The core Azure Function App that retrieves data from Content Hub and pushes it into Sitecore Search.

* **`Sitecore.ContentHub.Integration.SearchConnector.AppHost`**
  A [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) host for local development and orchestration.

* **`Sitecore.ContentHub.Integration.SearchConnector.ServiceDefaults`**
  Default service configurations shared across the Aspire project.

## Prerequisites

To run or deploy this connector, you’ll need:

* A Sitecore Content Hub instance
* A Sitecore Search instance with an **API Push source**
* An OAuth client in Content Hub with appropriate read access (see permissions below)
* A user account in Content Hub for authentication
* Azure Service Bus (for asynchronous message handling)

### Minimum Permissions for Content Hub User

Following the principle of least privilege, the Content Hub user should only have **read** access to the entity definitions and related entities that are included in the configuration. This ensures data can be retrieved without the user having broader access than necessary.

## Configuration

The following environment variables must be set (in `local.settings.json` or your Azure Function App settings):

| Variable                     | Description                                      |
| ---------------------------- | ------------------------------------------------ |
| `ContentHub:BaseUrl`         | Base URL of your Content Hub instance            |
| `ContentHub:ClientId`        | OAuth Client ID                                  |
| `ContentHub:ClientSecret`    | OAuth Client Secret                              |
| `ContentHub:Username`        | Username of the import user                      |
| `ContentHub:Password`        | Password for the import user                     |
| `Search:BaseUrl`             | Base URL of the Sitecore Search API              |
| `Search:AuthToken`           | API key for the Search Push source               |
| `Search:DomainId`            | Domain ID for Sitecore Search                    |
| `Search:SourceId`            | Source ID for the Search Push source             |
| `ServiceBus:UpsertQueueName` | Azure Service Bus queue name for upsert messages |
| `ServiceBus:DeleteQueueName` | Azure Service Bus queue name for delete messages |

### `config.json` Schema

A `config.json` file must be provided to define the mapping logic between Content Hub entities and Sitecore Search schema.

#### Root Structure

```json
{
  "CultureMaps": [/* CultureMap */],
  "DefinitionMaps": [/* DefinitionMap */]
}
```

#### CultureMap

```json
{
  "ContentHubCulture": "en-US",
  "SearchCulture": "en_US"
}
```

#### DefinitionMap

```json
{
  "ContentHubEntityDefinition": "M.Asset",
  "SearchEntity": "digital-asset",
  "FieldMaps": [ /* FieldMap types */ ]
}
```

##### PropertyFieldMap

```json
{
  "Type": "property",
  "SearchAttributeName": "title",
  "ContentHubPropertyName": "Title"
}
```

##### OptionListFieldMap

```json
{
  "Type": "optionlist",
  "SearchAttributeName": "category",
  "ContentHubPropertyName": "AssetCategory",
  "ContentHubOptionListName": "AssetCategoryList" /* optional /*
}
```

##### RelationFieldMap

```json
{
  "Type": "relation",
  "SearchAttributeName": "related-items",
  "ContentHubRelations": [ /* optional /*
    {
      "Name": "AssetToProduct",
      "Role": "Child"
    }
  ]
}
```

##### PublicLinkFieldMap

```json
{
  "Type": "publiclink",
  "SearchAttributeName": "download-url",
  "ContentHubRelations": [], /* optional /*
  "ContentHubResourceName": "DownloadOriginal",
  "CreateLinkIfNotExists": true
}
```

##### ExtractedContentFieldMap

```json
{
  "Type": "extractedcontent",
  "SearchAttributeName": "full-text",
  "ContentHubRelations": [] /* optional /*
}
```

## Functions

The Azure Function App provides both HTTP and Service Bus-triggered functions for flexibility in how data is pushed to Search.

### HTTP Endpoints

| Function           | Purpose                                                                           |
| ------------------ | --------------------------------------------------------------------------------- |
| `HealthCheck`      | Returns `"OK"` – basic availability check                                         |
| `RunUpsert`        | Processes an incoming Content Hub Action Message and upserts the entity to Search |
| `RunDelete`        | Processes an Action Message and removes the entity from Search                    |
| `AddUpsertMessage` | Places the message on the upsert Service Bus queue                                |
| `AddDeleteMessage` | Places the message on the delete Service Bus queue                                |

### Service Bus Listeners

| Function    | Trigger      | Purpose                                 |
| ----------- | ------------ | --------------------------------------- |
| `RunUpsert` | Upsert queue | Upserts an entity from a queued message |
| `RunDelete` | Delete queue | Deletes an entity from a queued message |

All functions expect a [Content Hub Action Message](https://doc.sitecore.com/ch/en/developers/content-hub/index.html#Actions) JSON payload.

## Usage

1. Deploy the Azure Function App or run locally via Aspire.
2. Configure a **config.json** file with appropriate field mappings and entity definitions.
3. In Content Hub:

   * Create upsert and delete actions using either:

     * **API Call Actions** (pointing to HTTP functions), or
     * **Azure Service Bus Actions** (pointing to upsert/delete queues)
   * Configure appropriate triggers to invoke those actions based on your publishing workflow.