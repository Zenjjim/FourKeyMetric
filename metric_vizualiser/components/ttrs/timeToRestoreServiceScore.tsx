import { Score } from "components/score";
import { COLORS } from "const";
import { ITimeToRestoreService } from "types";
import { getHoursfromSeconds } from "utils";
type TimeToRestoreServiceProps = {
  data: ITimeToRestoreService;
};
export function TimeToRestoreServiceScore({ data }: TimeToRestoreServiceProps) {
  const title = "Time To Restore Service";
  if (!data) {
    if (!data) {
      return <Score category={"No Data"} color={COLORS.RED} title={title} />;
    }
  }
  const [category, color] = ttrs_category(data?.medianRestoreServiceTime);
  return <Score category={category} color={color} title={title} />;
}

export function ttrs_category(median: number) {
  const medianHours = getHoursfromSeconds(median);
  if (medianHours <= 1) {
    return ["One Hour", COLORS.PURPLE];
  } else if (medianHours <= 8) {
    return ["One Day", COLORS.GREEN];
  } else if (medianHours <= 8 * 5) {
    return ["One Week", COLORS.YELLOW];
  } else if (medianHours <= 8 * 5 * 4.5) {
    return ["One Month", COLORS.YELLOW];
  } else {
    return ["One Year", COLORS.RED];
  }
}
