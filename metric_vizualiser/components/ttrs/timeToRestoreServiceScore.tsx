import { Score } from "components/score";
import { COLORS } from "const";
import { ITimeToRestoreService } from "types";
import { getHoursfromSeconds } from "utils";
type TimeToRestoreServiceProps = {
  data: ITimeToRestoreService;
};
export function TimeToRestoreServiceScore({ data }: TimeToRestoreServiceProps) {
  const [category, color] = ttrs_category(data?.medianRestoreServiceTime);
  return (
    <Score
      category={category}
      color={color}
      title={"Time To Restore Service"}
    />
  );
}

export function ttrs_category(median: number) {
  median = getHoursfromSeconds(median);
  if (median <= 1) {
    return ["One Hour", COLORS.PURPLE];
  } else if (median <= 24) {
    return ["One Day", COLORS.GREEN];
  } else if (median <= 24 * 7) {
    return ["One Week", COLORS.YELLOW];
  } else if (median <= 24 * 7 * 4) {
    return ["One Month", COLORS.YELLOW];
  } else {
    return ["One Year", COLORS.RED];
  }
}
