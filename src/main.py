import os
from dotenv import load_dotenv
from pathlib import Path
import pandas as pd
from lead_change_time import lead_change_time
from utils import get_data
from urls import get_build_url, get_pr_url
from deployment_frequency import calculate_deployment_frequency
import matplotlib.pyplot as plt


def main():
    pd.options.mode.chained_assignment = None 
    env_path = Path('..', '.env')
    load_dotenv(dotenv_path=env_path)
    global CONFIG
    if True:
        CONFIG = {
            'branch': "refs/heads/master",
            'repo_id': 65,
            'repo_name': 'car-care-web',
            'json': '../car-care-web-jenkins.json',
            'project': "mollerdigital/carcare",
            'token': os.environ["AZURE_TOKEN"],
            'back_track_months': 3
        }
    else:
        CONFIG = {
            'branch': "refs/heads/main",
            'repo_id': 82,
            'repo_name': 'workshop-booking',
            'project': "mollerdigital/carcare",
            'json': None,
            'token': os.environ["AZURE_TOKEN"],
            'back_track_months': 3
        }
    
    size = 1
    
    plt.rcParams["figure.figsize"] = [16*size, 9*size]
    plt.rcParams["figure.autolayout"] = True
    
    fig = plt.figure(constrained_layout=True)
    fig.suptitle(f"{CONFIG['project']}/{CONFIG['repo_name']}", weight="bold")
    gs = fig.add_gridspec(4, 5)

    print(type(CONFIG['json']))
    # GET DATA
    build_data = get_data(get_build_url(CONFIG['project'], CONFIG['repo_id'], CONFIG['branch']), CONFIG['token'], CONFIG['json'])
    # build_data = pd.DataFrame(build_data["value"])
    
    build_data["queueTime"] = pd.to_datetime(build_data['queueTime']).dt.tz_localize(None)
    build_data["startTime"] = pd.to_datetime(build_data['startTime']).dt.tz_localize(None)
    build_data["finishTime"] = pd.to_datetime(build_data['finishTime']).dt.tz_localize(None)
    
    pr_data = get_data(get_pr_url(CONFIG['project'], CONFIG['repo_name'], CONFIG['branch']), CONFIG['token'])
    pr_data = pd.DataFrame(pr_data["value"])
    pr_data["creationDate"] = pd.to_datetime(pr_data['creationDate']).dt.tz_localize(None)
    pr_data["closedDate"] = pd.to_datetime(pr_data['closedDate']).dt.tz_localize(None)
    
    
    deployment_frequency_score = calculate_deployment_frequency(build_data, CONFIG['back_track_months'], (fig.add_subplot(gs[0:2, 3:]), fig.add_subplot(gs[1, 2])))
    lead_change_time_median, lead_change_time_std = lead_change_time(pr_data, build_data, CONFIG['back_track_months'], (fig.add_subplot(gs[0:2, :2]), fig.add_subplot(gs[0, 2])))
    
    print(f"Deployment frequency: {deployment_frequency_score}")
    print(f"Lead change time: \n Median: {lead_change_time_median}\n STD: {lead_change_time_std}")
    
    plt.subplots_adjust(left=0.1,
                    bottom=0.1, 
                    right=0.9, 
                    top=0.9, 
                    wspace=0.4, 
                    hspace=0.4)
    plt.show()
    
if __name__ == "__main__":
    main()
