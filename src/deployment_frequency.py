import pandas as pd
import math

from utils import get_back_track_months, get_median, three_working_months_generator

def get_release_per_day(build_data):
    release_per_day = []
    for item in build_data.to_dict('records'):
        try:
            index = release_per_day.index(next(filter(lambda x: x.get('date') == item.get('date'), release_per_day)))
            release_per_day[index]['count'] += 1
        except:
            release_per_day.append({'date': item.get('date'), 'count': int(not math.isnan(item.get('id')))})
    return pd.DataFrame(sorted(release_per_day, key=lambda d: d['date']))

def get_release_per_week(release_per_day):
    release_per_day['week_start_date'] =  pd.to_datetime(release_per_day['date']) - pd.to_timedelta(7, unit='d')
    release_days_per_week = release_per_day.groupby(['date', pd.Grouper(key='week_start_date', freq='W-MON')]).sum().reset_index()
    release_days_per_week = release_days_per_week.groupby('week_start_date').sum().reset_index()
    return release_days_per_week

def get_release_per_month(release_days_per_week):
    releases_per_month = release_days_per_week.groupby(pd.Grouper(key='week_start_date', freq='MS')).sum().reset_index()
    releases_per_month = releases_per_month.rename(columns={"week_start_date": "month_start_date"})
    return releases_per_month

def deployment_frequency(frequency_week, frequency_month):
    # Find median of deployment days in week.
    # If more or equals then 3 then daily
    # If atleast ones each week then weekly
    # If atleast ones each month then monthly
    # Else yearly
    if (frequency_week >= 3):
        return 'Daily'
    elif (frequency_week >= 1):
        return 'Weekly'
    elif (frequency_month >= 1):
        return 'Monthly'
    else:
        return 'Yearly'
    
def fill_working_days(data, back_track_months):
    working_days = three_working_months_generator(back_track_months)
    data = data[["id", "project", "repository", "sourceBranch",  "status", "result", "startTime", "finishTime", ]]
    data['date'] = data['finishTime'].dt.normalize()
    data = pd.merge(data, working_days, how="outer", on="date")
    return get_back_track_months(data, data.date, back_track_months)

def calculate_deployment_frequency(build_data, back_track_months):
    build_data = fill_working_days(build_data, back_track_months)
    release_per_day = get_release_per_day(build_data)
    releases_per_week = get_release_per_week(release_per_day)
    releases_per_month = get_release_per_month(releases_per_week)
    frequency_week = get_median(releases_per_week)
    frequency_month = get_median(releases_per_month)
    score = deployment_frequency(frequency_week, frequency_month)
    return score
    