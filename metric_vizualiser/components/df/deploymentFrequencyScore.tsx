import { Score } from "components/score";
import { COLORS } from "const";
import { IDeploymentFrequency } from "types";
type DeploymentFrequencyProps = {
  data: IDeploymentFrequency;
  textSize: number;
};
export function DeploymentFrequencyScore({
  data,
  textSize,
}: DeploymentFrequencyProps) {
  const [category, color] = df_category(data);
  return (
    <Score
      category={category}
      color={color}
      textSize={textSize}
      title={"Deployment Frequency"}
    />
  );
}

export function df_category({
  dailyMedian,
  weeklyMedian,
  monthlyMedian,
}: IDeploymentFrequency) {
  if (dailyMedian >= 1) {
    return ["Daily", COLORS.PURPLE];
  } else if (weeklyMedian >= 3) {
    return ["Daily", COLORS.GREEN];
  } else if (weeklyMedian >= 1) {
    return ["Weekly", COLORS.GREEN];
  } else if (monthlyMedian >= 1) {
    return ["Monthly", COLORS.YELLOW];
  } else {
    return ["Yearly", COLORS.RED];
  }
}
