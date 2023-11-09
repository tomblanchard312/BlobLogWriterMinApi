# BlobLogWriterMinApi

BlobLogWriterMinApi is a .NET Core 6 Minimal API project that allows you to log messages to Azure Blob Storage and provides functionality to list and download log files from the storage.
This is a sample project that has an api that will log to Azure Blog Storage, which would be called by the python clients on a RaspberryPi or other IOT device.
## Setup

To set up the project with actual Azure Blob Storage, follow these steps:

1. Ensure you have the necessary Azure Blob Storage connection string.
2. Make sure your managed identity has the required permissions:
   - Add the managed identity to the Blob Contributor role.
   - Add the managed identity to the Queue Contributor role (if required).

## Usage

The project includes a `BlobService` class that provides methods for interacting with Azure Blob Storage. It includes the following features:

- Logging messages to append blobs.
- Listing blob files in a container, sorted by creation date.
- Downloading the content of a blob.
- Reading blob data in a serialized format (e.g., CSV).

## Licensing

This project is licensed under the [Apache License 2.0](https://www.apache.org/licenses/LICENSE-2.0) and may contain components or libraries licensed under [Microsoft Open Source Public License](https://opensource.org/licenses/ms-pl-html/) or other open-source licenses.

Please refer to the respective license files for detailed information.

---

Have questions or need assistance? Feel free to reach out!

