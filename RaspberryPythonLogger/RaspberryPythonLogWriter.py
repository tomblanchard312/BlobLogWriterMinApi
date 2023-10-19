#this is a sample python file that I am assuming would be run on a raspberrypi that writes logs to azure blobs.
# if this is your first time running python on this instance, be sure to run the following first: pip install requests
#to run this open command line 
#python [path to file]\RaspberryPythonLogger.py

import requests
import json
import pdb
# Set the URL of the API
api_url = "https://localhost:7064/writelog"  # Replace with the actual API URL this is the Visual Studio debug uri
# Create a session and set the verify parameter to False
session = requests.Session()
session.verify = False
# Define the log message
log_message = {
    "Message": "Hello Raspberry Pi!",
    "MessageType": "INFO",
    "MessageDateTime": "2023-10-18T14:30:00"
}

# Convert the log message to JSON
log_json = json.dumps(log_message)

try:
    # Send a GET request to the service without SSL certificate verification
    response = session.post(api_url, data=log_json, headers={'Content-Type': 'application/json'})
    if response.status_code == 201:
        print("Log entry created successfully.")
    else:
        print(f"Request failed with status code: {response.status_code}")
except requests.exceptions.RequestException as e:
    print(f"An error occurred: {e}")
    
