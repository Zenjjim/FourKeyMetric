from pprint import pprint
import requests
import os
import pandas as pd
from datetime import datetime
from dateutil.relativedelta import relativedelta
from enum import Enum

def get_median(dataframe):
    return dataframe['count'].median()

def three_working_months_generator(back_track_months):
    # Get three months of working days
    today = datetime.today()
    three_months_ago = today + relativedelta(months=-back_track_months)
    dates = pd.bdate_range(three_months_ago, today)
    dates = pd.DataFrame(dates, columns=['date'])
    
    # Remove holidays
    year = datetime.today().year
    country = os.environ["COUNTRY"]
    holiday = pd.DataFrame(assert_ok(requests.get(f"https://date.nager.at/api/v3/publicholidays/{year}/{country}")))
    holiday['date'] = pd.to_datetime(holiday['date'])
    
    dates = pd.merge(dates,holiday, indicator=True, how='left', on='date').query('_merge=="left_only"').drop('_merge', axis=1)
    dates = dates['date']
    dates = pd.DataFrame(dates)
    return dates

def get_back_track_months(data, date_variable, back_track_months):
    months = datetime.today() + relativedelta(months=-back_track_months)
    return data[pd.to_datetime(date_variable, errors='coerce') >= months]


def get_auth_header(token):
    return {"Authorization": f"Basic {token}"}

def assert_ok(http_response):
    status = http_response.status_code
    if status != 200:
        raise Exception(f"HTTP status: {status}. Body: {http_response.text}")
    return http_response.json()

def get_data(url, token, json_url=None):
    if json_url:
        import json
        with open(json_url) as json_file:
            data = json.load(json_file)
    
        # Print the type of data variable
        print("Type:", type(data))
        data = pd.DataFrame(data["builds"])
        data = data[["number", "result", "displayName", "timestamp", "duration", "culprits"]]
        data = data[data['result'] == "SUCCESS"]
        data["queueTime"] = pd.to_datetime(data["timestamp"], unit='ms')
        data["startTime"] = pd.to_datetime(data["timestamp"], unit='ms')
        data["finishTime"] = pd.to_datetime(data["timestamp"]+data["duration"], unit='ms')
        data["id"] = data['number']
        
        
    else:
        headers = get_auth_header(token)

        r = requests.get(
            url,
            headers=headers,
        )
        data = assert_ok(r)
    return data

class Color(Enum):
    green = "forestgreen"
    yellow = "orange"
    red = "#ee0000"