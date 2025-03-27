# Content Hub Asset Importer

This Azure Function app is used to bulk import assets to Content Hub. It contains two functions, one which executes on a timer, every 15 minutes, and another which is executed using a HTTP trigger. 

## Getting Started

First, you must create an OAuth client in your Content Hub instance for this app to use. This can be done by going to /en-us/admin/oauthclients.

Next, create a user that will be used for the actual uploading of the assets. It is best to have a dedicated user for this, so that assets uploaded via this function can be differentiated from others. It is recommended to give this user the minimum permissions required to upload assets by creating a new user group with the following permissions:

|       Definition      |                         Conditions                         |  Permissions |
|:---------------------:|:----------------------------------------------------------:|:------------:|
| M.UploadConfiguration |                                                            | Read         |
| M.Asset               | M.Final.Lifecycle.Status = Created Created by current user | Update       |
| M.Asset, M.File       | Created by current user                                    | Read, Create |

Then configure the environment variables for this app via your preferred method:

|          Variable         |               Example Value               |                             Description                            |
|:-------------------------:|:-----------------------------------------:|:------------------------------------------------------------------:|
| ContentHubUrl             | https://my-instance.sitecoresandbox.cloud | The base url of your Content Hub instance                          |
| ContentHubClientId        | AzureImporter                             | The ClientId of the OAuth client you configured in Content Hub     |
| ContentHubClientSecret    | MySup3rS3cr3tK3y!                         | The ClientSecret of the OAuth client you configured in Content Hub |
| ContentHubUsername        | PublicUploadUser                          | The Username of the user you created in Content Hub                |
| ContentHubPassword        | MySup3rS3cr3tP455w0rd!                    | The Password of the user you created in Content Hub                |
| AzureStorageContainerName | asset-import                              | The name of the container to use within the Azure storage account  |