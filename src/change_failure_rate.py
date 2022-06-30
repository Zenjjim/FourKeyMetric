from itertools import count
from deployment_frequency import fill_working_days, get_release_per_day, get_release_per_week
from utils import Color, get_back_track_months
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

def change_failure_rate_category(total_rate):
    if (total_rate < 1.15):
        return ('0 - 15%', Color.green.value)
    elif (total_rate < 1.45):
        return ('16 - 45%', Color.yellow.value)
    else:
        return ('> 45%', Color.red.value)

def plot_change_failure_rate(change_failure_rate_by_week, total_rate, axes):
    week = change_failure_rate_by_week['week_start_date']
    count = change_failure_rate_by_week['count']

    mask = np.isfinite(count)
    line, = axes[0].plot(week[mask], count[mask], ls="--", lw=1)
    axes[0].plot(week, count, color=line.get_color(), lw=1.5)
    axes[0].tick_params(labelrotation=45)
    axes[0].set_title(f"Change Failure Rate")
    axes[0].set_xlabel("Deployment Date")
    axes[0].set_ylabel("Failure rate per week")

    failure_rate_var, failure_rate_color = change_failure_rate_category(total_rate)
    circle = plt.Circle((0, 0), 1, color=failure_rate_color)
    axes[1].add_patch(circle)
    axes[1].set_xlim([-1, 1])
    axes[1].set_ylim([-1, 1])
    axes[1].text(0.5,0.5,failure_rate_var,horizontalalignment='center',
     verticalalignment='center', transform = axes[1].transAxes, fontsize=14)
    axes[1].axis('off')
    axes[1].set_box_aspect(1)
    axes[1].set_title("Change Failure Rate")


def change_failure_rate(pr_data, build_data, back_track_months, axes):
    
    #pd.set_option("display.max_rows", None, "display.max_columns", None, "display.max_colwidth", None)
    pd.set_option("display.max_rows", None)
    pr_data = get_back_track_months(pr_data, pr_data.creationDate, back_track_months).sort_values("closedDate")
    build_data = get_back_track_months(build_data, build_data.queueTime, back_track_months).sort_values("queueTime")

    total_rate = pr_data.shape[0]/build_data.shape[0]

    merged_pr_build_data = pd.merge_asof(pr_data, build_data, left_on="closedDate", right_on="queueTime", direction="forward").sort_values("closedDate", ascending=False)
    merged_pr_build_data = merged_pr_build_data[merged_pr_build_data.title.str.contains("fix|bug|error|crash|issue", case=False)]
    merged_pr_build_data['date'] =  pd.to_datetime(merged_pr_build_data['finishTime']).dt.date
    merged_pr_build_data = fill_working_days(merged_pr_build_data, back_track_months)
    merged_pr_build_data['week_start_date'] =  pd.to_datetime(merged_pr_build_data['date']).dt.to_period('W').apply(lambda r: r.start_time)
    merged_pr_build_data = merged_pr_build_data[["week_start_date", "finishTime", "title", "queueTime"]].sort_values("week_start_date")
    merged_pr_build_data = merged_pr_build_data.drop_duplicates()
    merged_pr_build_data["count"] = 1
    

    #merged_pr_build_data["count"] = merged_pr_build_data["week_start_date"].value_counts(dropna=False) #["week_start_date"])
    merged_pr_build_data = (merged_pr_build_data.groupby("week_start_date").sum()-1).reset_index()
    #print(merged_pr_build_data["count"])

    #merged_pr_build_data["count"] = merged_pr_build_data.groupby("week_start_date")["week_start_date"].count() #["week_start_date"])
    #merged_pr_build_data  = merged_pr_build_data.groupby("week_start_date").count().reset_index()
    #print(merged_pr_build_data)
    build_data_all = fill_working_days(build_data, back_track_months)
    builds_per_day = get_release_per_day(build_data_all)
    builds_per_week = get_release_per_week(builds_per_day)
    #failures_per_week = merged_pr_build_data
    #failures_per_week = failures_per_week.groupby('week_start_date')

    all_deploys_and_fails = merged_pr_build_data.merge(builds_per_week, left_on='week_start_date', right_on='week_start_date')
    change_failure_rate_df = all_deploys_and_fails
    change_failure_rate_df["count"] = all_deploys_and_fails['count_x'].div(all_deploys_and_fails['count_y'].values)
    plot_change_failure_rate(change_failure_rate_df, total_rate, axes)

    return total_rate