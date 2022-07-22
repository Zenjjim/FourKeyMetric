import { Score } from "components/score";
import { COLORS } from "const";
import { ILeadTimeChange } from "types";
import { getHoursfromSeconds } from "utils";
type LeadTimeChangeProps = { data: ILeadTimeChange,   textSize: number };
export function LeadTimeChangeScore({ data, textSize }: LeadTimeChangeProps) {

  const [category, color] = ltc_category(data.medianLeadTimeChange);
  return (
    <Score
      category={category}
      color={color}
      title={"Median Lead Time to Change"}
      textSize={textSize}
    />
  );
}

export function ltc_category(median: number) {
  median = getHoursfromSeconds(median);
  if (median <= 24) {
    return ["One Day", COLORS.PURPLE];
  } else if (median <= 24 * 7) {
    return ["One Week", COLORS.GREEN];
  } else if (median <= 24 * 7 * 4) {
    return ["One Month", COLORS.YELLOW];
  } else if (median <= 24 * 7 * 4 * 6) {
    return ["Six Months", COLORS.YELLOW];
  } else {
    return ["One Year", COLORS.RED];
  }
}
