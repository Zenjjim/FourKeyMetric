from deployment_frequency import fill_working_days
from utils import Color, get_back_track_months
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

def median_category(median):
    if (median < 24 * 60):
        return ('One Day', Color.purple.value)
    elif (median < 24 * 7 * 60):
        return ('One Week', Color.green.value)
    elif (median < 24 * 30 * 60):
        return ('One Month', Color.yellow.value)
    elif (median < 24 * 30 * 6 * 60):
        return ('Six Months', Color.red.value)
    else:
        return ('One year', Color.red.value)

def plot_lead_change(merged_pr_build_data_week,merged_pr_build_data, median, std, axes):
    median_int = median.total_seconds() / 60
    x_week = merged_pr_build_data_week['week_start_date']
    y_week = merged_pr_build_data_week["lead_change_time_delta"].dt.days
    x = merged_pr_build_data['finishTime']
    y = merged_pr_build_data["lead_change_time_delta"].dt.days
    
    mask = np.isfinite(y_week)
    line, = axes[0].plot(x_week[mask], y_week[mask], ls="--", lw=1)
    axes[0].plot(x_week, y_week, color=line.get_color(), lw=1.5)
    axes[0].scatter(x, y)
    axes[0].tick_params(labelrotation=45)
    axes[0].set_title(f"Median Lead Time to Change")
    axes[0].set_xlabel("Deployment Date")
    axes[0].set_ylabel("Days to Change")

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
    axes[1].set_title("Median Lead Time to Change")

def plot_lead_bucket(median, std):
    median_category = median_category(median)


def get_release_per_week(release_per_day):
    release_per_day['week_start_date'] =  pd.to_datetime(release_per_day['date']) - pd.to_timedelta(7, unit='d')
    release_days_per_week = release_per_day.groupby(['date', pd.Grouper(key='week_start_date', freq='W-MON')]).sum().reset_index()
    release_days_per_week = release_days_per_week.groupby('week_start_date').sum().reset_index()
    return release_days_per_week

def lead_change_time(pr_data, build_data, back_track_months, axes):
    pr_data = get_back_track_months(pr_data, pr_data.creationDate, back_track_months).sort_values("closedDate")
    build_data = get_back_track_months(build_data, build_data.queueTime, back_track_months).sort_values("queueTime")
    
    merged_pr_build_data = pd.merge_asof(pr_data, build_data, left_on="closedDate", right_on="queueTime", direction="forward").sort_values("closedDate", ascending=False)
    merged_pr_build_data = merged_pr_build_data[merged_pr_build_data['finishTime'].notna()]
    merged_pr_build_data["lead_change_time_delta"] = merged_pr_build_data['finishTime']-merged_pr_build_data['creationDate']
    plot_data =  merged_pr_build_data
    merged_pr_build_data['date'] =  pd.to_datetime(merged_pr_build_data['finishTime']).dt.date
    merged_pr_build_data['week_start_date'] =  pd.to_datetime(merged_pr_build_data['date']).dt.to_period('W').apply(lambda r: r.start_time)
    
    non_merged_days = fill_working_days(merged_pr_build_data, back_track_months)
    non_merged_days['week_start_date'] =  pd.to_datetime(non_merged_days['date']).dt.to_period('W').apply(lambda r: r.start_time)
    non_merged_days =  non_merged_days[['week_start_date', 'lead_change_time_delta']]
    non_merged_days = non_merged_days.groupby('week_start_date').median().reset_index()
    non_merged_days['lead_change_time_delta'] = pd.NaT
    
    merged_pr_build_data = merged_pr_build_data[['week_start_date', 'lead_change_time_delta']]
    merged_pr_build_data_week = merged_pr_build_data.groupby('week_start_date').median().reset_index()
    merged_pr_build_data_week = pd.merge(merged_pr_build_data_week, non_merged_days, on="week_start_date", how="outer").sort_values('week_start_date')
    merged_pr_build_data_week.rename(columns = {"lead_change_time_delta_x": "lead_change_time_delta"}, inplace = True)
    median = merged_pr_build_data_week["lead_change_time_delta"].median()
    std = merged_pr_build_data_week["lead_change_time_delta"].std()
    plot_lead_change(merged_pr_build_data_week, plot_data , median, std, axes)
    merged_pr_build_data_week.sort_values("lead_change_time_delta")
    return median, std