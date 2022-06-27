from utils import get_back_track_months
import pandas as pd

def lead_change_time(pr_data, build_data, back_track_months):
    pr_data = get_back_track_months(pr_data, pr_data.creationDate, back_track_months).sort_values("closedDate")
    build_data = get_back_track_months(build_data, build_data.queueTime, back_track_months).sort_values("queueTime")
    merged_pr_build_data = pd.merge_asof(pr_data, build_data, left_on="closedDate", right_on="queueTime", direction="forward").sort_values("closedDate", ascending=False)
    merged_pr_build_data = merged_pr_build_data[merged_pr_build_data['finishTime'].notna()]
    merged_pr_build_data["lead_change_time_delta"] = merged_pr_build_data['finishTime']-merged_pr_build_data['creationDate']
    merged_pr_build_data.sort_values("lead_change_time_delta")
    median = merged_pr_build_data["lead_change_time_delta"].median()
    std = merged_pr_build_data["lead_change_time_delta"].std()
    return median, std 