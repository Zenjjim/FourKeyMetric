import os
from deployment_frequency import fill_working_days
from urls import get_jira_ticket_url
from utils import Color, assert_ok, get_back_track_months, get_ticket_data
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np
from datetime import datetime, timezone


def median_category(median):
    if (median < 1 * 60):
        return ('One Hour', Color.purple.value)
    elif (median < 24 * 60):
        return ('One Day', Color.green.value)
    elif (median < 24 * 7 * 60):
        return ('One Week', Color.yellow.value)
    elif (median < 24 * 30 * 60):
        return ('One Month', Color.red.value)
    elif (median < 24 * 30 * 6 * 60):
        return ('Six Months', Color.red.value)
    else:
        return ('One year', Color.red.value)


def plot_time_to_restore_service(merged_pr_build_data_week,merged_pr_build_data, median, axes):
    median_int = median.total_seconds() / 60
    x_week = merged_pr_build_data_week['week_start_date']
    y_week = merged_pr_build_data_week["time_to_restore_service"].dt.days
    merged_pr_build_data["time_to_restore_service"] = merged_pr_build_data["time_to_restore_service"].dt.days
    
    mask = np.isfinite(y_week)
    line, = axes[0].plot(x_week[mask], y_week[mask], ls="--", lw=1)
    axes[0].plot(x_week, y_week, color=line.get_color(), lw=1.5)
    groups = merged_pr_build_data.groupby('jira_time')
    for name, group in groups:
        axes[0].plot(group.finishTime, group.time_to_restore_service, marker='o', linestyle='', color=("C0" if name else Color.red.value), label=("JIRA ticket not found" if not name else None))
    axes[0].legend()
    axes[0].tick_params(labelrotation=45)
    axes[0].set_title(f"Median Time to Restore Service")
    axes[0].set_xlabel("Deployment Date")
    axes[0].set_ylabel("Days to Restore")

    median_category_var, median_color = median_category(median_int)
    circle = plt.Circle((0, 0), 1, color=median_color)
    axes[1].add_patch(circle)
    axes[1].set_xlim([-1, 1])
    axes[1].set_ylim([-1, 1])
    axes[1].text(0.5,0.5,median_category_var,horizontalalignment='center',
     verticalalignment='center', transform = axes[1].transAxes, fontsize=14)
    axes[1].text(0.5,0.3,f"*{median.days} day(s)",horizontalalignment='center',
     verticalalignment='center', transform = axes[1].transAxes, fontsize=8)
    axes[1].axis('off')
    axes[1].set_box_aspect(1)
    axes[1].set_title("Median Time to Restore Service")


def time_to_restore_service(pr_data, build_data, back_track_months, ticket_project, axes):
    
    now = datetime.now()

    pr_data = get_back_track_months(pr_data, pr_data.creationDate, back_track_months).sort_values("closedDate")
    build_data = get_back_track_months(build_data, build_data.queueTime, back_track_months).sort_values("queueTime")

    pr_data = pr_data[pr_data.title.str.contains("fix|bug|error|crash|issue", case=False)]

    pr_data["issue_time"] = 0
    pr_data["jira_time"] = False
    for index, row in pr_data.to_dict('index').items():
        try :
            jira = row["title"].split(" ")[0]
            data = assert_ok(get_ticket_data(get_jira_ticket_url(project=ticket_project, ticket=jira), os.environ["JIRA_TOKEN"]))
            pr_data.at[index,'issue_time'] = pd.to_datetime(data['fields']['created'], utc=True)
            pr_data.at[index,'jira_time'] = True

        except Exception as e:
            pr_data.at[index,'issue_time'] = pd.to_datetime(row.get('creationDate'), utc=True)
            pr_data.at[index,'jira_time'] = False

    merged_pr_build_data = pd.merge_asof(pr_data, build_data, left_on="closedDate", right_on="queueTime", direction="forward").sort_values("closedDate", ascending=False)
    merged_pr_build_data = merged_pr_build_data[merged_pr_build_data['finishTime'].notna()]
    merged_pr_build_data["time_to_restore_service"] = pd.to_datetime(merged_pr_build_data['finishTime'], utc=True)-pd.to_datetime(merged_pr_build_data['issue_time'], utc=True)
    plot_data =  merged_pr_build_data
    merged_pr_build_data['date'] =  pd.to_datetime(merged_pr_build_data['finishTime']).dt.date
    merged_pr_build_data['week_start_date'] =  pd.to_datetime(merged_pr_build_data['date']).dt.to_period('W').apply(lambda r: r.start_time)
    
    non_merged_days = fill_working_days(merged_pr_build_data, back_track_months)
    non_merged_days['week_start_date'] =  pd.to_datetime(non_merged_days['date']).dt.to_period('W').apply(lambda r: r.start_time)
    non_merged_days =  non_merged_days[['week_start_date', 'time_to_restore_service']]
    non_merged_days = non_merged_days.groupby('week_start_date').median().reset_index()
    non_merged_days['time_to_restore_service'] = pd.NaT
    
    merged_pr_build_data = merged_pr_build_data[['week_start_date', 'time_to_restore_service']]
    merged_pr_build_data_week = merged_pr_build_data.groupby('week_start_date').median().reset_index()
    merged_pr_build_data_week = pd.merge(merged_pr_build_data_week, non_merged_days, on="week_start_date", how="outer").sort_values('week_start_date')
    merged_pr_build_data_week.rename(columns = {"time_to_restore_service_x": "time_to_restore_service"}, inplace = True)
    median = merged_pr_build_data_week["time_to_restore_service"].median()
    plot_time_to_restore_service(merged_pr_build_data_week, plot_data, median, axes)
    merged_pr_build_data_week.sort_values("time_to_restore_service")
    return median