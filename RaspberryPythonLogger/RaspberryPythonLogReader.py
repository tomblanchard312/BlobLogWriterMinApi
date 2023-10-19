#pip install pandas
import requests
import json
import pandas as pd

# Set the URL of the API to read log data
url = "https://localhost:7064/readlogserialized?blobName=2023-10-19.csv"  # Replace with the actual API URL and blob name

try:
    # Send a GET request to the API to read log data
    session = requests.Session()
    session.verify = False

    response = session.get(url)

    if response.status_code == 200:
        # Parse the JSON response
        log_data = response.json()

        # Create a DataFrame from the log data
        df = pd.DataFrame(log_data)

        # Display the log data as a table
        print(df)
    else:
        print(f"Failed to read log data. Status code: {response.status_code}")
except requests.exceptions.RequestException as e:
    print(f"An error occurred: {e}")
