from deployment_frequency import fill_working_days
from utils import Color, get_back_track_months
import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

def time_to_restore_service(pr_data, build_data, back_track_months, axes):
    
    pd.set_option("display.max_rows", None, "display.max_columns", None, "display.max_colwidth", None)
    pr_data = get_back_track_months(pr_data, pr_data.creationDate, back_track_months).sort_values("closedDate")
    build_data = get_back_track_months(build_data, build_data.queueTime, back_track_months).sort_values("queueTime")

    pr_data = pr_data[pr_data.title.str.contains("fix|bug|error|crash|issue", case=False)]
    
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
    #plot_lead_change(merged_pr_build_data_week, plot_data , median, std, axes)
    merged_pr_build_data_week.sort_values("lead_change_time_delta")
    return median, std