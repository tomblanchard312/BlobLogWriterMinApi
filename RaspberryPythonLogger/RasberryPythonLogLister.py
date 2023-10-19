import requests

# Set the URL of the API's "list" endpoint
url = "https://localhost:7064/listlogs?orderByDescending=true"  # Replace with the actual API URL

try:
    # Send a GET request to the API to list blob files
    session = requests.Session()
    session.verify = False

    response = session.get(url)

    if response.status_code == 200:
        # Parse the JSON response
        blob_list = response.json()

        # Display the list of blob files
        for blob in blob_list:
            print(f"Blob Name: {blob['Name']}")
            print(f"Creation Date: {blob['CreationDate']}")
            print("-----")
    else:
        print(f"Failed to list blob files. Status code: {response.status_code}")
except requests.exceptions.RequestException as e:
    print(f"An error occurred: {e}")

