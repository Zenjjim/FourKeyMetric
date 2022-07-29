import { Score } from "components/score";
import { COLORS } from "const";
import { IDeploymentFrequency } from "types";
type DeploymentFrequencyProps = {
  data: IDeploymentFrequency;
};
export function DeploymentFrequencyScore({ data }: DeploymentFrequencyProps) {
  const title = "Deployment Frequency";
  if (!data) {
    return <Score category={"No Data"} color={COLORS.RED} title={title} />;
  }
  const [category, color] = df_category(data);
  return <Score category={category} color={color} title={title} />;
}

export function df_category({
  dailyMedian,
  weeklyMedian,
  monthlyMedian,
}: IDeploymentFrequency) {
  if (dailyMedian >= 1) {
    return [DeploymentFrequencyEnums.DAILY, COLORS.PURPLE];
  } else if (weeklyMedian >= 3) {
    return [DeploymentFrequencyEnums.DAILY, COLORS.GREEN];
  } else if (weeklyMedian >= 1) {
    return [DeploymentFrequencyEnums.WEEKLY, COLORS.GREEN];
  } else if (monthlyMedian >= 1) {
    return [DeploymentFrequencyEnums.MONTHLY, COLORS.YELLOW];
  } else {
    return [DeploymentFrequencyEnums.YEARLY, COLORS.RED];
  }
}

export enum DeploymentFrequencyEnums {
  DAILY = "Daily",
  WEEKLY = "Weekly",
  MONTHLY = "Monthly",
  YEARLY = "Yearly",
}
